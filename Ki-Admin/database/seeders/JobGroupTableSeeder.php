<?php

namespace Database\Seeders;

use Illuminate\Database\Seeder;
use Illuminate\Support\Facades\DB;

/**
 * =========================================================
 * Seeder    : A04_JobGroupTableSeeder
 * Project   : ki-admin - v1.0.0
 * Author    : Michael Battle
 * Created On: 07/Aug/2025
 *
 * Description:
 *   Seeds job_group table with educational role clusters
 * =========================================================
 */
class JobGroupTableSeeder extends Seeder
{
    public function run(): void
    {
    DB::table('job_groups')->truncate();

    $kiPath = base_path('database/seeders/Data Export/ki_job_groups.csv');
    $csvPath = file_exists($kiPath) ? $kiPath : base_path('database/seeders/Data/job_groups.csv');
        if (!file_exists($csvPath)) {
            $this->command->error("CSV file not found at {$csvPath}");
            return;
        }

        $file = fopen($csvPath, 'r');
        $header = fgetcsv($file); // get header row
        $count = 0;
        while (($row = fgetcsv($file)) !== false) {
            $data = count($header) === count($row) ? array_combine($header, $row) : null;
            if (empty($data['name'])) {
                continue;
            }
            DB::table('job_groups')->insert([
                'name' => $data['name'],
                'description' => $data['description'] ?? null,
                'created_at' => (empty($data['created_at']) || strtolower($data['created_at']) == 'null') ? now() : $data['created_at'],
                'updated_at' => (empty($data['updated_at']) || strtolower($data['updated_at']) == 'null') ? now() : $data['updated_at'],
            ]);
            $count++;
        }
        fclose($file);
        $this->command->info("✅ Seeded {$count} job groups from CSV.");
    }
}
