<?php

namespace Database\Seeders;

use Illuminate\Database\Console\Seeds\WithoutModelEvents;
use Illuminate\Database\Seeder;
use Illuminate\Support\Facades\DB;
use Illuminate\Support\Facades\File;

class UserMenuItemsSeeder extends Seeder
{
    // No role mapping needed, will import directly from CSV

    /**
     * Run the database seeds.
     */
    public function run(): void
    {
        // Prefer ki_ CSV in Data Export, else fallback
        $preferred = database_path('seeders/Data Export/ki_user_menu_items.csv');
        $fallback = database_path('seeders/Data/user_menu_items.csv');
        if (file_exists($preferred)) {
            $csvFile = $preferred;
        } elseif (file_exists($fallback)) {
            $csvFile = $fallback;
        } else {
            $this->command->error("CSV file not found: checked {$preferred} and {$fallback}");
            return;
        }

        // Read CSV file
        $csvData = array_map('str_getcsv', file($csvFile));
        $header = array_shift($csvData); // Remove header row

        $count = 0;
        foreach ($csvData as $row) {
            // Skip empty rows
            if (empty(array_filter($row))) {
                continue;
            }
            // Build associative record by header name when possible
            $assoc = null;
            if (is_array($header) && count($header) === count($row)) {
                $assoc = array_combine($header, $row);
            }

            // Support both header-based and position-based CSVs
            if ($assoc !== null) {
                $id = isset($assoc['id']) && $assoc['id'] !== '' ? $assoc['id'] : null;
                $user_id = $assoc['user_id'] ?? null;
                $menu_item_id = $assoc['menu_item_id'] ?? null;
                $is_granted = ($assoc['is_granted'] ?? '0') == '1' ? 1 : 0;
                    $is_default = ($assoc['is_default'] ?? '0') == '1' ? 1 : 0;
                $custom_order = $assoc['custom_order'] ?? null;
                $is_custom = ($assoc['is_custom'] ?? '0') == '1' ? 1 : 0;
                $created_at = $this->parseDateTime($assoc['created_at'] ?? null) ?? now();
                $updated_at = $this->parseDateTime($assoc['updated_at'] ?? null) ?? now();
            } else {
                // Fallback to previous positional mapping when header mismatch
                $id = $row[0] ?: null;
                $user_id = $row[1] ?? null;
                $menu_item_id = $row[2] ?? null;
                $is_granted = ($row[3] ?? '0') == '1' ? 1 : 0;
                $custom_order = $row[4] ?? null;
                $is_custom = ($row[5] ?? '0') == '1' ? 1 : 0;
                $created_at = $this->parseDateTime($row[6] ?? null) ?? now();
                $updated_at = $this->parseDateTime($row[7] ?? null) ?? now();
            }

            $data = [
                'user_id' => is_numeric($user_id) ? (int)$user_id : $user_id,
                'menu_item_id' => is_numeric($menu_item_id) ? (int)$menu_item_id : $menu_item_id,
                'is_granted' => $is_granted,
                    'is_default' => $is_default ?? 0,
                'custom_order' => $custom_order,
                'is_custom' => $is_custom,
                'created_at' => $created_at,
                'updated_at' => $updated_at,
            ];

            if ($id) {
                DB::table('user_menu_items')->updateOrInsert(['id' => $id], $data);
            } else {
                DB::table('user_menu_items')->updateOrInsert([
                    'user_id' => $data['user_id'],
                    'menu_item_id' => $data['menu_item_id'],
                ], $data);
            }
            $count++;
        }

        $this->command->info("✅ Seeded {$count} user menu items from {$csvFile}.");
    }

    private function parseDateTime($value)
    {
        if ($value === null) return null;
        $value = trim($value);
        if ($value === '' || strtolower($value) === 'null') return null;

        // Try DD/MM/YYYY HH:MM
        $dt = \DateTime::createFromFormat('d/m/Y H:i', $value);
        if ($dt !== false) return $dt->format('Y-m-d H:i:s');

        // Try DD/MM/YYYY
        $dt = \DateTime::createFromFormat('d/m/Y', $value);
        if ($dt !== false) return $dt->format('Y-m-d') . ' 00:00:00';

        // Fallback to strtotime
        $ts = strtotime($value);
        if ($ts !== false) return date('Y-m-d H:i:s', $ts);

        return null;
    }
}
