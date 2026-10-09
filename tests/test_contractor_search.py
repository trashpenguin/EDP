import contextlib
import io
import os
from pathlib import Path
import subprocess
import sys
import tempfile
import unittest
from unittest.mock import patch
from urllib.parse import parse_qs

sys.path.insert(0, str(Path(__file__).resolve().parents[1] / "scripts"))
import contractor_search as search


class ContractorSearchTests(unittest.TestCase):
    def invoke(self, args):
        with patch.object(sys, "argv", ["contractor_search.py", *args]), \
             contextlib.redirect_stdout(io.StringIO()), \
             contextlib.redirect_stderr(io.StringIO()):
            return search.main()

    def test_missing_stdin_exits_without_traceback(self):
        launcher = Path(__file__).resolve().parents[1] / "scripts" / "contractor_gui.py"
        result = subprocess.run(
            [sys.executable, str(launcher)], input="", text=True, capture_output=True
        )
        self.assertEqual(result.returncode, 2)
        self.assertIn("location is required", result.stderr)
        self.assertNotIn("Traceback", result.stderr)

    def test_empty_prompt(self):
        with patch("builtins.input", return_value=""):
            self.assertEqual(self.invoke([]), 2)

    def test_invalid_numeric_options_do_not_search(self):
        for option in ("--per-category", "--radius-m"):
            for value in ("0", "-1", "bad"):
                with self.subTest(option=option, value=value), \
                     patch.object(search, "collect") as collect:
                    with self.assertRaises(SystemExit) as raised:
                        self.invoke(["Detroit", option, value])
                    self.assertEqual(raised.exception.code, 2)
                    collect.assert_not_called()

    def test_failed_search_preserves_existing_output(self):
        with tempfile.TemporaryDirectory() as directory:
            target = Path(directory) / "contractors.csv"
            target.write_text("existing leads", encoding="utf-8")
            with patch.object(search, "collect", side_effect=RuntimeError("offline")):
                self.assertEqual(self.invoke(["Detroit", "--output", str(target)]), 1)
            self.assertEqual(target.read_text(encoding="utf-8"), "existing leads")

    def test_partial_failure_also_preserves_output(self):
        with tempfile.TemporaryDirectory() as directory:
            target = Path(directory) / "leads.csv"
            target.write_text("old", encoding="utf-8")
            row = search.Contractor("HVAC contractor", "Acme", "", "", "", "", "osm:node:1")
            with patch.object(search, "collect", side_effect=[[row], RuntimeError("offline")]):
                code = self.invoke(["Detroit", "--categories", "HVAC contractor",
                                    "Electrical contractor", "--output", str(target)])
            self.assertEqual(code, 1)
            self.assertEqual(target.read_text(encoding="utf-8"), "old")

    def test_write_failure_preserves_output_and_cleans_temp(self):
        with tempfile.TemporaryDirectory() as directory:
            target = Path(directory) / "leads.csv"
            target.write_text("old", encoding="utf-8")
            with patch.object(search.os, "replace", side_effect=OSError("locked")):
                with self.assertRaises(OSError):
                    search.write_csv([], str(target))
            self.assertEqual(target.read_text(encoding="utf-8"), "old")
            self.assertEqual(list(Path(directory).iterdir()), [target])

    def test_successful_output_quotes_commas(self):
        import csv
        with tempfile.TemporaryDirectory() as directory:
            target = Path(directory) / "leads.csv"
            row = search.Contractor("Electrical contractor", "Acme, Inc", "1 St, Detroit",
                                    "123", "a@acme.test", "", "osm:node:1")
            with patch.object(search, "collect", return_value=[row]):
                self.assertEqual(self.invoke(["Detroit", "--categories",
                    "Electrical contractor", "--output", str(target)]), 0)
            with target.open(newline="", encoding="utf-8") as handle:
                rows = list(csv.DictReader(handle))
            self.assertEqual(rows[0]["name"], "Acme, Inc")
            self.assertEqual(rows[0]["address"], "1 St, Detroit")

    def test_category_filters(self):
        expected = {
            "HVAC contractor": '["shop"="hvac"]',
            "Electrical contractor": '["craft"="electrician"]',
            "Excavating contractor": '["craft"="excavator"]',
        }
        for category, tag in expected.items():
            with self.subTest(category=category), \
                 patch.object(search, "http_post_text", return_value={"elements": []}) as post:
                search.overpass_search(42, -83, 1000, search.CATEGORY_QUERIES[category])
                query = parse_qs(post.call_args.args[1])["data"][0]
                self.assertIn(tag, query)
                self.assertNotIn("plumber", query)
                if category != "HVAC contractor":
                    self.assertNotIn('["shop"="hvac"]', query)
                if category != "Electrical contractor":
                    self.assertNotIn('["craft"="electrician"]', query)

    def test_enrichment_stops_at_limit_and_skips_duplicates(self):
        elements = [
            {"type": "node", "id": value, "tags": {"name": str(value)}}
            for value in (1, 1, 2, 3)
        ]
        with patch.object(search, "geocode_location", return_value=(42, -83)), \
             patch.object(search, "overpass_search", return_value=elements), \
             patch.object(search, "tags_to_contractor", side_effect=lambda category, el:
                 search.Contractor(category, el["tags"]["name"], "", "", "", "",
                                   f"osm:node:{el['id']}")) as enrich:
            rows = search.collect("Detroit", "Electrical contractor", 2, 1000, False)
        self.assertEqual([row.name for row in rows], ["1", "2"])
        self.assertEqual(enrich.call_count, 2)


if __name__ == "__main__":
    unittest.main()
