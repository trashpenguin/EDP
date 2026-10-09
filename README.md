# EDP

A Windows Forms customer/product application written in Visual Basic.

## Windows application

Requirements: Windows, Visual Studio 2022 with the .NET desktop workload and
.NET 6 SDK, MySQL 8, and Python 3.10 or newer for password provisioning.
Excel must be installed to use Excel export; it is not required to build.
mysqldump must be on PATH to use database backup.

Open EDP/EDP.sln and build, or run:

```powershell
dotnet build EDP/EDP/EDP.vbproj --configuration Release
```

### Database setup

For a **new, disposable database**, import EDP/EDP/database/database.sql as a
database administrator. This dump drops and recreates tables; do not import
it over a database containing records you need to keep.

For an **existing database**, back it up first, then apply
EDP/EDP/database/migrations/001_secure_login.sql as an administrator.
This migration preserves customer/product/order records, widens password storage,
and disables existing plaintext passwords. Each affected user needs a password
reset before signing in.

Inventory triggers and GetCustomerDiscount were removed because this application
and schema have no stock or discount fields. The migration drops these invalid
routines instead of inventing stock quantities. GetCategoryRevenue and the report
views remain available.

No default login accounts or passwords are included. On a fresh database,
create an account as the administrator (choose an unused numeric ID):

```sql
INSERT INTO db.users (idUsers, username, password) VALUES (1, 'admin', NULL);
```

Generate password reset SQL. The password is prompted twice and never appears in
command-line arguments:

```powershell
python scripts/set_user_password.py 1
```

Execute the resulting UPDATE statement as the database administrator.
The application verifies salted PBKDF2-SHA256 hashes with 600,000 iterations;
it does not accept plaintext passwords.

Create a restricted application database account using a unique password:

```sql
CREATE USER 'edp_app'@'localhost' IDENTIFIED BY 'replace-with-a-unique-password';
GRANT SELECT ON db.users TO 'edp_app'@'localhost';
GRANT SELECT, INSERT ON db.customers TO 'edp_app'@'localhost';
GRANT SELECT, INSERT ON db.products TO 'edp_app'@'localhost';
GRANT SELECT ON db.ordersbycustomer TO 'edp_app'@'localhost';
```

Set EDP_DB_CONNECTION in the environment of the application/Visual Studio before
launching it. For example, in PowerShell (replace the placeholder):

```powershell
$env:EDP_DB_CONNECTION = 'Server=localhost;Port=3306;Database=db;User ID=edp_app;Password=your-unique-password;'
```

Backups use the configured account and report failures without replacing an
existing backup. A backup account needs additional read/SHOW VIEW/TRIGGER and,
depending on MySQL's configuration, PROCESS and routine metadata privileges.
Use an account provisioned by your database administrator for backups.
mysqldump receives its password through the child process environment, not its
command line. Protect exported SQL files: they contain customer records and
password hashes.

Change the previously committed database/login passwords wherever they were
reused. Changing current source files does not remove them from Git history.
Previously committed executables/installers contain the old implementation;
build from current source rather than running those artifacts.

## Checks

```powershell
python -m unittest discover -s tests -v
```

GitHub Actions checks the password provisioning script, builds the Windows application,
verifies password hashing, and runs a MySQL 8 schema/migration smoke test.
GUI interaction, installed Excel automation, and backup permissions still require
manual verification.
