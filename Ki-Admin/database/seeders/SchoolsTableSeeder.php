<?php

/**
 * =========================================================
 * Seeder     : SchoolsTableSeeder
 * Project    : ki-admin - v1.0.0
 * Author     : Michael Battle
 * Created On : 08/Aug/2025
 *
 * Description:
 *   Imports schools from CSV into the `schools` table,
 *   providing foundational data for school-based relationships.
 * 
 * Origin: Adapted from support-hub SchoolsTableSeeder
 * =========================================================
 */

namespace Database\Seeders;

use Illuminate\Database\Seeder;
use Illuminate\Support\Facades\DB;
// Use native CSV parsing to avoid external dependency

class SchoolsTableSeeder extends Seeder
{
    /**
     * =========================================================
     * Section: run()
     * Description: Parse CSV and upsert into schools table
     * =========================================================
     */
    public function run(): void
    {
        /**
         * =========================================================
         * Section: CSV File Location & Validation
         * Description: Ensure source CSV is present
         * =========================================================
         */
        $csvPath = base_path('database/seeders/Data/schools.csv');
        if (! file_exists($csvPath)) {
            $this->command->error("CSV file not found at {$csvPath}");
            return;
        }

        /**
         * =========================================================
         * Section: Load & Parse CSV
         * Description: Read header row and map records
         * =========================================================
         */
        // prefer exported ki_ CSV if present
        $kiPath = base_path('database/seeders/Data Export/ki_schools.csv');
        $csvPath = file_exists($kiPath) ? $kiPath : $csvPath;

        $file = fopen($csvPath, 'r');
        if ($file === false) {
            $this->command->error("Unable to open CSV: {$csvPath}");
            return;
        }

        $header = fgetcsv($file);
        $count = 0;
        while (($row = fgetcsv($file)) !== false) {
            if (empty(array_filter($row))) {
                continue;
            }
            $data = count($header) === count($row) ? array_combine($header, $row) : null;
            if ($data === null) {
                $data = [
                    'school_ref' => $row[0] ?? null,
                    'name' => $row[1] ?? null,
                    'code' => $row[2] ?? null,
                    'description' => $row[3] ?? null,
                    'status' => $row[4] ?? 'active',
                    'created_at' => $row[5] ?? null,
                    'updated_at' => $row[6] ?? null,
                ];
            }

            DB::table('schools')->updateOrInsert(
                ['code' => $data['code']],
                [
                    'school_ref' => $data['school_ref'],
                    'name' => $data['name'],
                    'code' => $data['code'],
                    'description' => $this->parseNullableString($data['description'] ?? null),
                    'status' => $data['status'] ?? 'active',
                    'created_at' => $this->parseNullableDate($data['created_at'] ?? null),
                    'updated_at' => $this->parseNullableDate($data['updated_at'] ?? null),
                ]
            );
            $count++;
        }
        fclose($file);

        /**
         * =========================================================
         * Section: Completion Report
         * Description: Log seeding outcome
         * =========================================================
         */
        $this->command->info("✅ Seeded {$count} school records into `schools` table.");
    }

    /**
     * =========================================================
     * Section: parseNullableDate()
     * Description: Convert CSV cell to datetime string or null
     * =========================================================
     */
    private function parseNullableDate(?string $value): ?string
    {
        $v = trim((string) $value);
        if ($v === '' || strtoupper($v) === 'NULL') {
            return null;
        }
        return $v;
    }

    /**
     * =========================================================
     * Section: parseNullableString()
     * Description: Convert CSV cell to string or null
     * =========================================================
     */
    private function parseNullableString(?string $value): ?string
    {
        $v = trim((string) $value);
        if ($v === '' || strtoupper($v) === 'NULL') {
            return null;
        }
        return $v;
    }
}
