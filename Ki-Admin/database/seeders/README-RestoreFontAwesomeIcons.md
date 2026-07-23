Restore FontAwesome Icons Seeder (chg0272)
=======================================

What this does
---------------
- Reads `database/seeders/Data Export/ki_menu_items-old.csv` which contains historical
  menu item rows with a column named `icon` that stores FontAwesome class strings.
- Updates the `menu_items.icon` column in the database for matching rows.

Safety and backups
------------------
- Before updating, the seeder writes a CSV backup of existing `menu_items.icon` values to:
  `storage/app/menu_items_icon_backup_<timestamp>.csv`.
- The seeder runs all updates inside a DB transaction. If an error occurs the transaction
  is rolled back.

How to run (Windows, cmd.exe)
-----------------------------
1. Open your project folder in a terminal at the repository root.
2. Run: php artisan db:seed --class=Database\\Seeders\\RestoreFontAwesomeIconsSeeder

Notes
-----
- The seeder matches records by `id` (menu_item_id) first, then by `slug` if `id` is not present.
- Review the backup CSV before proceeding to other steps or undoing changes.
- This is intended for local/dev usage. Verify on a staging copy before running in production.
