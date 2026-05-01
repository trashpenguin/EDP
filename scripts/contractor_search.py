#!/usr/bin/env python3
from __future__ import annotations
import argparse, csv, json, re, sys
from dataclasses import dataclass
from html.parser import HTMLParser
from typing import Iterable
from urllib.parse import quote_plus, urljoin, urlparse
from urllib.request import Request, urlopen

NOMINATIM_ENDPOINTS = [
    "https://nominatim.openstreetmap.org/search",
    "https://nominatim.geocoding.ai/search",
]
OVERPASS_ENDPOINTS = [
    "https://overpass-api.de/api/interpreter",
    "https://overpass.kumi.systems/api/interpreter",
    "https://maps.mail.ru/osm/tools/overpass/api/interpreter",
]
EMAIL_REGEX = re.compile(r"[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}", re.IGNORECASE)
USER_AGENT = "ContractorSearchBot/2.1 (contact: local-lead-tool)"
CONTACT_HINTS = ("contact", "about", "team", "staff", "support", "get-in-touch")

CATEGORY_QUERIES = {
    "HVAC contractor": ["heating", "air conditioning", "hvac"],
    "Electrical contractor": ["electrician", "electrical"],
    "Excavating contractor": ["excavating", "earthwork", "grading", "dirt work"],
}

class LinkParser(HTMLParser):
    def __init__(self) -> None:
        super().__init__()
        self.links: list[str] = []

    def handle_starttag(self, tag: str, attrs: list[tuple[str, str | None]]) -> None:
        if tag != "a":
            return
        for k, v in attrs:
            if k == "href" and v:
                self.links.append(v.strip())

@dataclass
class Contractor:
    category: str; name: str; address: str; phone: str; email: str; website: str; place_id: str

def http_get_json(url: str) -> dict | list:
    req = Request(url, headers={"User-Agent": USER_AGENT})
    with urlopen(req, timeout=40) as resp:
        return json.loads(resp.read().decode("utf-8"))

def http_post_text(url: str, body: str) -> dict:
    req = Request(url, data=body.encode("utf-8"), headers={"User-Agent": USER_AGENT, "Content-Type": "application/x-www-form-urlencoded"}, method="POST")
    with urlopen(req, timeout=60) as resp:
        return json.loads(resp.read().decode("utf-8"))

def http_get_text(url: str) -> str:
    req = Request(url, headers={"User-Agent": USER_AGENT})
    with urlopen(req, timeout=20) as resp:
        return resp.read().decode("utf-8", errors="ignore")

def geocode_location(location: str) -> tuple[float, float]:
    last_err = None
    for endpoint in NOMINATIM_ENDPOINTS:
        try:
            url = f"{endpoint}?q={quote_plus(location)}&format=jsonv2&limit=1&countrycodes=us"
            data = http_get_json(url)
            if data:
                return float(data[0]["lat"]), float(data[0]["lon"])
        except Exception as e:
            last_err = e
    raise RuntimeError(f"Geocoding failed across endpoints: {last_err}")

def overpass_search(lat: float, lon: float, radius_m: int, keywords: list[str]) -> list[dict]:
    regex = "|".join(re.escape(k) for k in keywords)
    query = f'''[out:json][timeout:60];
(
  nwr(around:{radius_m},{lat},{lon})["name"~"{regex}",i];
  nwr(around:{radius_m},{lat},{lon})["shop"~"hvac|trade",i];
  nwr(around:{radius_m},{lat},{lon})["craft"~"electrician|plumber",i];
);
out center tags;'''
    payload = f"data={quote_plus(query)}"
    last_err = None
    for endpoint in OVERPASS_ENDPOINTS:
        try:
            return http_post_text(endpoint, payload).get("elements", [])
        except Exception as e:
            last_err = e
    raise RuntimeError(f"Overpass failed across endpoints: {last_err}")

def normalize_email(email: str) -> str:
    return email.strip().strip(".,;:()[]{}<>").lower()

def pick_email(html: str) -> str:
    emails = [normalize_email(e) for e in EMAIL_REGEX.findall(html)]
    emails = [e for e in emails if "@" in e and not any(x in e for x in ["example.", "wixpress", "sentry"])]
    return sorted(set(emails), key=len)[0] if emails else ""

def candidate_contact_links(base_url: str, html: str) -> list[str]:
    parser = LinkParser(); parser.feed(html)
    base_domain = urlparse(base_url).netloc
    ranked: list[tuple[int, str]] = []
    for href in parser.links:
        if href.lower().startswith("mailto:"): continue
        absolute = urljoin(base_url, href); p = urlparse(absolute)
        if p.scheme not in {"http", "https"} or p.netloc != base_domain: continue
        path = (p.path or "").lower(); score = 10 if any(h in path for h in CONTACT_HINTS) else 0
        ranked.append((score, absolute))
    ranked.sort(key=lambda x: x[0], reverse=True)
    out=[]; seen=set()
    for _, link in ranked:
        if link in seen: continue
        seen.add(link); out.append(link)
        if len(out) >= 5: break
    return out

def extract_email(website: str) -> str:
    if not website: return ""
    try: homepage = http_get_text(website)
    except Exception: return ""
    email = pick_email(homepage)
    if email: return email
    for link in candidate_contact_links(website, homepage):
        try: html = http_get_text(link)
        except Exception: continue
        email = pick_email(html)
        if email: return email
    return ""

def tags_to_contractor(category: str, el: dict) -> Contractor:
    tags = el.get("tags", {})
    name = tags.get("name", "")
    phone = tags.get("phone") or tags.get("contact:phone", "")
    website = tags.get("website") or tags.get("contact:website", "")
    email = tags.get("email") or tags.get("contact:email", "")
    if not email and website:
        email = extract_email(website)
    address = ", ".join([tags.get("addr:housenumber", ""), tags.get("addr:street", ""), tags.get("addr:city", ""), tags.get("addr:state", ""), tags.get("addr:postcode", "")]).strip(", ").replace(", ,", ",")
    place_id = f"osm:{el.get('type','')}:{el.get('id','')}"
    return Contractor(category, name, address, phone, email, website, place_id)

def unique_by_place(rows: Iterable[Contractor]) -> list[Contractor]:
    seen=set(); out=[]
    for r in rows:
        if not r.place_id or r.place_id in seen: continue
        seen.add(r.place_id); out.append(r)
    return out

def collect(location: str, category: str, limit: int, radius_m: int) -> list[Contractor]:
    lat, lon = geocode_location(location)
    keywords = CATEGORY_QUERIES.get(category, [category])
    elements = overpass_search(lat, lon, radius_m, keywords)
    rows = [tags_to_contractor(category, e) for e in elements if e.get("tags", {}).get("name")]
    return unique_by_place(rows)[:limit]

def write_csv(rows: list[Contractor], path: str) -> None:
    with open(path, "w", newline="", encoding="utf-8") as f:
        w = csv.DictWriter(f, fieldnames=["category", "name", "phone", "email", "website", "address", "place_id"])
        w.writeheader(); [w.writerow(r.__dict__) for r in rows]

def main() -> int:
    ap = argparse.ArgumentParser(description="Contractor finder (free OSM/Nominatim/Overpass)")
    ap.add_argument("location")
    ap.add_argument("--categories", nargs="+", default=["HVAC contractor", "Electrical contractor", "Excavating contractor"])
    ap.add_argument("--per-category", type=int, default=30)
    ap.add_argument("--radius-m", type=int, default=30000)
    ap.add_argument("--output", default="contractors.csv")
    a = ap.parse_args()
    rows=[]
    for c in a.categories:
        print(f"Searching {c} ...")
        try:
            got = collect(a.location, c, a.per_category, a.radius_m)
        except Exception as e:
            print(f"  error: {e}", file=sys.stderr)
            got = []
        print(f"  found {len(got)}")
        rows.extend(got)
    write_csv(rows, a.output)
    print(f"Wrote {len(rows)} rows to {a.output}")
    return 0

if __name__ == "__main__":
    raise SystemExit(main())
