-- Apply once to an existing db before running the updated application.
-- The GUI has no inventory or discount feature; remove routines that reference nonexistent columns.
USE db;
DROP TRIGGER IF EXISTS insert_orderdetails_trigger;
DROP TRIGGER IF EXISTS update_orderdetails_trigger;
DROP TRIGGER IF EXISTS delete_orderdetails_trigger;
DROP FUNCTION IF EXISTS GetCustomerDiscount;
ALTER TABLE users MODIFY password VARCHAR(255) DEFAULT NULL;
-- Disable legacy plaintext passwords. Reset each account using set_user_password.py.
UPDATE users SET password = NULL
WHERE password IS NOT NULL AND password NOT LIKE 'pbkdf2-sha256$%';
