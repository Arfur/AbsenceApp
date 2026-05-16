<?php

/**
 * =========================================================
 * Seeder    : RoleTypesTableSeeder
 * Project   : ki-admin - v1.0.0
 * Author    : Michael Battle
 * Created On: 07/Aug/2025
 *
 * Description:
 *   Seeds the `role_types` table with the three core user roles:
 *   - user
 *   - admin
 *   - super_admin
 * Provides foundational role-type mapping for user onboarding
 * and access control.
 *
 * Origin:
 * Defined as the first seeder in the data population chain
 * to satisfy FK constraints in the `users` table.
 * =========================================================
 */

namespace Database\Seeders;

use Illuminate\Database\Seeder;
use Illuminate\Support\Facades\DB;
use Illuminate\Support\Facades\Log;

class RoleTypesTableSeeder extends Seeder
{
    public function run()
    {
        $csvPath = base_path('database/seeders/Data Export/ki_role_types.csv');
        if (!file_exists($csvPath)) {
            Log::error('CSV file not found: ' . $csvPath);
            return;
        }

        $handle = fopen($csvPath, 'r');
        if ($handle === false) {
            Log::error('Unable to open CSV file: ' . $csvPath);
            return;
        }

        $header = fgetcsv($handle);
        while (($row = fgetcsv($handle)) !== false) {
            $data = array_combine($header, $row);

            // Determine unique key for updateOrInsert: prefer id, fall back to name
            $key = [];
            if (!empty($data['id'])) {
                $key['id'] = (int) $data['id'];
            } elseif (!empty($data['name'])) {
                $key['name'] = $data['name'];
            } else {
                Log::warning('Skipping row in role types CSV with no id or name: ' . json_encode($row));
                continue;
            }

            // Ensure display_name is provided (required by schema) or generate from name
            $displayName = null;
            if (!empty($data['display_name'])) {
                $displayName = $data['display_name'];
            } elseif (!empty($data['name'])) {
                $displayName = ucwords(str_replace(['_', '-'], ' ', $data['name']));
            }

            // Parse optional boolean/integer fields with sensible defaults
            $isSystem = isset($data['is_system_role']) ? filter_var($data['is_system_role'], FILTER_VALIDATE_BOOLEAN) : false;
            $isDefault = isset($data['is_default']) ? filter_var($data['is_default'], FILTER_VALIDATE_BOOLEAN) : false;
            $priority = isset($data['priority']) && is_numeric($data['priority']) ? (int) $data['priority'] : 1;

            DB::table('role_types')->updateOrInsert($key, [
                'name' => $data['name'] ?? null,
                'display_name' => $displayName,
                'description' => $data['description'] ?? null,
                'is_system_role' => $isSystem,
                'is_default' => $isDefault,
                'priority' => $priority,
            ]);
        }
        fclose($handle);
    }
}
