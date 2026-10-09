import base64
import hashlib
from pathlib import Path

salt = bytes(range(16))
digest = hashlib.pbkdf2_hmac("sha256", b"correct horse battery staple", salt, 600000)
Path("tests/password-fixture.txt").write_text("$".join([
    "pbkdf2-sha256", "600000",
    base64.b64encode(salt).decode("ascii"),
    base64.b64encode(digest).decode("ascii"),
]), encoding="ascii")
