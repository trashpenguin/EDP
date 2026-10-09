import base64
import contextlib
import hashlib
import io
from pathlib import Path
import sys
import unittest
from unittest.mock import patch

sys.path.insert(0, str(Path(__file__).resolve().parents[1] / "scripts"))
import set_user_password as provisioning


class PasswordProvisioningTests(unittest.TestCase):
    def invoke(self, identifier, passwords):
        output = io.StringIO()
        with patch.object(sys, "argv", ["set_user_password.py", identifier]), \
             patch.object(provisioning.getpass, "getpass", side_effect=passwords), \
             contextlib.redirect_stdout(output), contextlib.redirect_stderr(io.StringIO()):
            provisioning.main()
        return output.getvalue()

    def test_generates_verifiable_hash_without_plaintext(self):
        password = "correct horse battery staple"
        sql = self.invoke("7", [password, password])
        self.assertNotIn(password, sql)
        self.assertTrue(sql.endswith("WHERE idUsers=7;\n"))
        encoded = sql.split("'")[1]
        algorithm, iterations, salt, digest = encoded.split("$")
        self.assertEqual(algorithm, "pbkdf2-sha256")
        self.assertEqual(int(iterations), 600000)
        self.assertEqual(len(base64.b64decode(salt)), 16)
        self.assertEqual(base64.b64decode(digest), hashlib.pbkdf2_hmac(
            "sha256", password.encode("utf-8"), base64.b64decode(salt), int(iterations)
        ))

    def test_uses_random_salt_for_each_reset(self):
        password = "correct horse battery staple"
        self.assertNotEqual(
            self.invoke("7", [password, password]),
            self.invoke("7", [password, password]),
        )

    def test_rejects_invalid_ids(self):
        for identifier in ("0", "-1", "7; DROP TABLE users"):
            with self.subTest(identifier=identifier), self.assertRaises(SystemExit) as error:
                self.invoke(identifier, [])
            self.assertEqual(error.exception.code, 2)

    def test_rejects_short_or_mismatched_passwords(self):
        for passwords in (["short"], ["correct horse battery staple", "different"]):
            with self.subTest(passwords=passwords), self.assertRaises(SystemExit) as error:
                self.invoke("7", passwords)
            self.assertEqual(error.exception.code, 2)


if __name__ == "__main__":
    unittest.main()
