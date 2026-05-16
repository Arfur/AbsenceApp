<?php

namespace Database\Seeders;

use Illuminate\Database\Seeder;
use Illuminate\Support\Facades\DB;
use Illuminate\Support\Facades\File;
use Carbon\Carbon;

/**
 * =========================================================
 * Seeder    : A02_DepartmentSeeder
 * Project   : ki-admin - v1.0.0
 * Author    : Michael Battle
 * Created On: 07/Aug/2025
 *
 * Description:
 *   Seeds the `departments` table using a centralized CSV source.
 *   Removes legacy `school_id` linkage for modular design.
 *
 * CSV Location:
 * database/seeders/data/departments.csv
 *
 * Notes:
 * - Assumes header structure: name,code,description,status,created_at,updated_at
 * - Skips blank rows and guards against missing fields
 * =========================================================
 */
class DepartmentSeeder extends Seeder
{
    public function run(): void
    {
        // Prefer exported 'ki_' CSV if present, otherwise fall back to Data/departments.csv
        $kiPath = base_path('database/seeders/Data Export/ki_departments.csv');
        $csvPath = File::exists($kiPath)
            ? $kiPath
            : base_path('database/seeders/Data/departments.csv');

        if (!File::exists($csvPath)) {
            $this->command->error("❌ CSV file missing: {$csvPath}");
            return;
        }

        // Open CSV and read header
        $file = fopen($csvPath, 'r');
        if ($file === false) {
            $this->command->error("❌ Unable to open CSV: {$csvPath}");
            return;
        }

        $header = fgetcsv($file);
        if (empty($header)) {
            $this->command->error("❌ CSV appears empty or missing header: {$csvPath}");
            fclose($file);
            return;
        }

        $count = 0;

        // Process rows and insert using header mapping when available
        while (($row = fgetcsv($file)) !== false) {
            // Skip empty rows
            if (empty(array_filter($row))) {
                continue;
            }

            // Map row to associative array when header is present
            $data = count($header) === count($row) ? array_combine($header, $row) : null;

            // Fallback mapping for older CSVs (attempt sensible positions)
            if ($data === null) {
                // expected order: name,code,description,status,created_at,updated_at
                $data = [
                    'name' => $row[0] ?? null,
                    'code' => $row[1] ?? null,
                    'description' => $row[2] ?? null,
                    'status' => $row[3] ?? null,
                    'created_at' => $row[4] ?? null,
                    'updated_at' => $row[5] ?? null,
                ];
            }

            if (empty($data['name']) || empty($data['code'])) {
                continue; // skip if required fields missing
            }

            DB::table('departments')->updateOrInsert(
                ['code' => $data['code']],
                [
                    'name' => $data['name'],
                    'code' => $data['code'],
                    'description' => $data['description'] ?? null,
                    'created_at' => (empty($data['created_at']) || strtolower($data['created_at']) == 'null') ? Carbon::now() : $data['created_at'],
                    'updated_at' => (empty($data['updated_at']) || strtolower($data['updated_at']) == 'null') ? Carbon::now() : $data['updated_at'],
                ]
            );

            $count++;
        }

        fclose($file);
        $this->command->info("✅ Seeded {$count} departments from {$csvPath}.");
    }
}
