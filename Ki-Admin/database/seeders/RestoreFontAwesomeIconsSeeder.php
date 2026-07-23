<?php

namespace Database\Seeders;

use Illuminate\Database\Seeder;
use Illuminate\Support\Facades\DB;

/**
 * chg0272: Restore FontAwesome icons from legacy CSV
 * Date: 2025-08-29
 * Description:
 * - Reads `database/seeders/Data Export/ki_menu_items-old.csv` which contains
 *   historical FontAwesome class strings in the `icon` column.
 * - For each row, updates the `menu_items.icon` column (matching by
 *   `menu_item_id` or `id` or `slug`) to the legacy FontAwesome value.
 * - Creates a CSV backup of existing `menu_items.icon` values at
 *   `storage/app/menu_items_icon_backup_<timestamp>.csv` before applying changes.
 * Notes:
 * - This seeder is intentionally conservative: it writes a reversible backup
 *   and prints progress to the console. Run locally and verify before using in
 *   production. The path to the CSV contains spaces; we resolve it via base_path().
 */
class RestoreFontAwesomeIconsSeeder extends Seeder
{
    public function run()
    {
        $csvPath = base_path('database/seeders/Data Export/ki_menu_items-old.csv');
        if (!file_exists($csvPath)) {
            $this->command->error("CSV file not found: {$csvPath}");
            return;
        }

        // Prepare backup file
        $backupPath = storage_path('app/menu_items_icon_backup_' . date('Ymd_His') . '.csv');
        $backupHandle = fopen($backupPath, 'w');
        fputcsv($backupHandle, ['menu_item_id', 'slug', 'old_icon', 'new_icon', 'updated_at']);

        $handle = fopen($csvPath, 'r');
        $header = fgetcsv($handle);
        if ($header === false) {
            $this->command->error('CSV appears empty or unreadable.');
            fclose($handle);
            fclose($backupHandle);
            return;
        }

        // Normalize header keys to simple names
        $keys = array_map(function ($h) { return trim($h); }, $header);

        // Run updates inside a transaction for safety
        DB::beginTransaction();
        try {
            $updated = 0;
            while (($row = fgetcsv($handle)) !== false) {
                if (count($row) !== count($keys)) {
                    // skip malformed rows
                    continue;
                }
                $data = array_combine($keys, $row);

                // Determine identifiers and the legacy icon value
                $menuItemId = $data['menu_item_id'] ?? ($data['id'] ?? null);
                $slug = $data['slug'] ?? null;
                $legacyIcon = isset($data['icon']) ? trim($data['icon']) : null;

                if (is_null($legacyIcon) || $legacyIcon === '') {
                    // nothing to apply
                    continue;
                }

                // Lookup existing record (prefer ID match)
                $record = null;
                if (!empty($menuItemId)) {
                    $record = DB::table('menu_items')->where('id', $menuItemId)->first();
                }
                if (!$record && !empty($slug)) {
                    $record = DB::table('menu_items')->where('slug', $slug)->first();
                }
                if (!$record) {
                    // record not found; skip but log
                    $this->command->info("Skipping unknown menu item: id={$menuItemId} slug={$slug}");
                    continue;
                }

                $oldIcon = $record->icon;

                // Write backup row
                fputcsv($backupHandle, [$record->id, $record->slug ?? '', $oldIcon, $legacyIcon, date('c')]);

                // Only update if different
                if ($oldIcon !== $legacyIcon) {
                    DB::table('menu_items')->where('id', $record->id)->update([
                        'icon' => $legacyIcon,
                        'updated_at' => now(),
                    ]);
                    $updated++;
                    $this->command->info("Updated menu_items.id={$record->id} icon: '{$oldIcon}' -> '{$legacyIcon}'");
                }
            }

            DB::commit();
            $this->command->info("Done. Updated {$updated} records. Backup saved to: {$backupPath}");
        } catch (\Exception $e) {
            DB::rollBack();
            $this->command->error('Error updating icons: ' . $e->getMessage());
        } finally {
            fclose($handle);
            fclose($backupHandle);
        }
    }
}
