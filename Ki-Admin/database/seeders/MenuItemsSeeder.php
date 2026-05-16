<?php

namespace Database\Seeders;

use Illuminate\Database\Console\Seeds\WithoutModelEvents;
use Illuminate\Database\Seeder;
use Illuminate\Support\Facades\DB;
use Illuminate\Support\Facades\File;

class MenuItemsSeeder extends Seeder
{
    /**
     * Run the database seeds.
     */
    public function run(): void
    {
        // Prefer ki_ CSV from Data Export, else fall back to Data/
        $preferred = database_path('seeders/Data Export/ki_menu_items.csv');
        $fallback = database_path('seeders/Data/menu_items.csv');
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
        
        // Process each row
        $insertData = [];
        foreach ($csvData as $row) {
            // Skip empty rows
            if (empty(array_filter($row))) {
                continue;
            }
            
            // Map CSV data to database fields
            $defaultDate = '2025-01-01 00:00:00';
            $data = [
                'id' => $row[0] ?: null,
                'title' => $row[1],
                'slug' => $row[2],
                'is_visible' => $row[3] == '1' ? true : false,
                'url' => $row[4] === 'NULL' || empty($row[4]) ? null : $row[4],
                'route_name' => $row[5] === 'NULL' || empty($row[5]) ? null : $row[5],
                'icon' => $row[6] === 'NULL' || empty($row[6]) ? null : $row[6],
                'icon_color' => $row[7] === 'NULL' || empty($row[7]) ? null : $row[7],
                'tooltip' => $row[8] === 'NULL' || empty($row[8]) ? null : $row[8],
                'badge_count' => $row[9] === 'NULL' || empty($row[9]) ? null : (int)$row[9],
                'is_collapsible' => $row[10] == '1' ? true : false,
                'permission_key' => $row[11] === 'NULL' || empty($row[11]) ? null : $row[11],
                'component' => $row[12] === 'NULL' || empty($row[12]) ? null : $row[12],
                'order' => (int)$row[13],
                'is_external' => $row[14] == '1' ? true : false,
                'parent_id' => $row[15] === 'NULL' || empty($row[15]) ? null : (int)$row[15],
                'display_order' => $row[16] === 'NULL' || empty($row[16]) ? null : (int)$row[16],
                'menu_group' => $row[17] ?: 'main',
                'is_favorite' => $row[18] == '1' ? true : false,
                'created_at' => empty($row[19]) ? $defaultDate : $row[19],
                'updated_at' => empty($row[20]) ? $defaultDate : $row[20],
            ];
            
            // Remove id if null to let auto-increment handle it
            if (is_null($data['id'])) {
                unset($data['id']);
            }
            
            $insertData[] = $data;
        }
        
        // Use upsert to handle existing records
        DB::table('menu_items')->upsert(
            $insertData,
            ['id'], // unique key
            ['title', 'slug', 'is_visible', 'url', 'route_name', 'icon', 'order', 'is_external', 'parent_id', 'display_order', 'menu_group', 'updated_at'] // columns to update
        );
        
        $this->command->info('Menu items seeded successfully from CSV!');
    }
}
