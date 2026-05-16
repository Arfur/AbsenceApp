<?php

namespace Database\Seeders;

use Illuminate\Database\Seeder;
use Illuminate\Support\Facades\DB;

/**
 * =========================================================
 * Seeder    : A03_JobTitlesTableSeeder
 * Project   : ki-admin - v1.0.0
 * Author    : Michael Battle
 * Created On: 07/Aug/2025
 *
 * Description:
 *   Imports job titles from CSV into the `job_titles` table,
 *   providing foundational data for user job assignments.
 * =========================================================
 */
class JobTitlesTableSeeder extends Seeder
{
    public function run(): void
    {
        $kiPath = base_path('database/seeders/Data Export/ki_job_titles.csv');
        $csvPath = file_exists($kiPath) ? $kiPath : base_path('database/seeders/Data/job_titles.csv');
        if (! file_exists($csvPath)) {
            $this->command->error("CSV file not found at {$csvPath}");
            return;
        }

        // Use native CSV parsing to avoid external dependency
        $file = fopen($csvPath, 'r');
        if ($file === false) {
            $this->command->error("Unable to open CSV: {$csvPath}");
            return;
        }

        $header = fgetcsv($file);
        $count = 0;
        while (($row = fgetcsv($file)) !== false) {
            $data = count($header) === count($row) ? array_combine($header, $row) : null;
            if ($data === null) {
                // fallback: assume first column is title
                $title = $row[0] ?? null;
                $description = $row[1] ?? null;
                $created_at = $row[2] ?? null;
                $updated_at = $row[3] ?? null;
            } else {
                $title = $data['title'] ?? null;
                $description = $data['description'] ?? null;
                $created_at = $data['created_at'] ?? null;
                $updated_at = $data['updated_at'] ?? null;
            }

            if (empty($title)) {
                $this->command->warn("Skipping record - empty title");
                continue;
            }

            DB::table('job_titles')->updateOrInsert([
                'title' => $title,
            ], [
                'description' => $description ?? null,
                'created_at' => (empty($created_at) || strtolower($created_at) == 'null') ? now() : $created_at,
                'updated_at' => (empty($updated_at) || strtolower($updated_at) == 'null') ? now() : $updated_at,
            ]);
            $count++;
        }
        fclose($file);
        $this->command->info("✅ Seeded {$count} job titles.");
    }
}
