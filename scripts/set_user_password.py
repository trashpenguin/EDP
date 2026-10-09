#!/usr/bin/env python3
"""Generate SQL to reset an existing user's password; execute it as a database administrator."""
import argparse
import base64
import getpass
import hashlib
import secrets


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("user_id", type=int)
    args = parser.parse_args()
    if args.user_id <= 0:
        parser.error("user_id must be positive")
    password = getpass.getpass("New password: ")
    if len(password) < 12:
        parser.error("use at least 12 characters")
    if password != getpass.getpass("Confirm password: "):
        parser.error("passwords do not match")
    salt = secrets.token_bytes(16)
    iterations = 600000
    digest = hashlib.pbkdf2_hmac("sha256", password.encode("utf-8"), salt, iterations)
    encoded = "$".join([
        "pbkdf2-sha256", str(iterations),
        base64.b64encode(salt).decode("ascii"),
        base64.b64encode(digest).decode("ascii"),
    ])
    print(f"UPDATE db.users SET password='{encoded}' WHERE idUsers={args.user_id};")


if __name__ == "__main__":
    main()
