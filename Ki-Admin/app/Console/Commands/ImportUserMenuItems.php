<?php

namespace App\Console\Commands;

use Illuminate\Console\Command;
use Illuminate\Support\Facades\DB;
use Illuminate\Support\Facades\File;

class ImportUserMenuItems extends Command
{
    protected $signature = 'import:user-menu-items';
    protected $description = 'Import user menu items from CSV file';

    public function handle()
    {
        $csvFile = database_path('seeders/Data/user_menu_items.csv');
        
        if (!File::exists($csvFile)) {
            $this->error("CSV file not found: {$csvFile}");
            return 1;
        }

        // Disable foreign key checks temporarily
        DB::statement('SET FOREIGN_KEY_CHECKS=0;');

        // Clear existing data
        DB::table('user_menu_items')->truncate();
        $this->info('Cleared existing user_menu_items data');

        // Read and import CSV
        $handle = fopen($csvFile, 'r');
        $header = fgetcsv($handle); // Skip header row
        
        $imported = 0;
        $errors = 0;

        while (($row = fgetcsv($handle)) !== false) {
            try {
                $data = array_combine($header, $row);
                
                // Convert date format from DD/MM/YYYY HH:MM to YYYY-MM-DD HH:MM:SS
                $createdAt = \DateTime::createFromFormat('d/m/Y H:i', $data['created_at']);
                $updatedAt = \DateTime::createFromFormat('d/m/Y H:i', $data['updated_at']);
                
                DB::table('user_menu_items')->insert([
                    'id' => $data['id'],
                    'user_id' => $data['user_id'],
                    'menu_item_id' => $data['menu_item_id'],
                    'is_granted' => $data['is_granted'],
                    'custom_order' => $data['custom_order'],
                    'is_custom' => $data['is_custom'],
                    'created_at' => $createdAt ? $createdAt->format('Y-m-d H:i:s') : now()->format('Y-m-d H:i:s'),
                    'updated_at' => $updatedAt ? $updatedAt->format('Y-m-d H:i:s') : now()->format('Y-m-d H:i:s')
                ]);
                
                $imported++;
            } catch (\Exception $e) {
                $this->error("Error importing row: " . $e->getMessage());
                $errors++;
            }
        }

        fclose($handle);

        // Re-enable foreign key checks
        DB::statement('SET FOREIGN_KEY_CHECKS=1;');

        $this->info("Import completed!");
        $this->info("Records imported: {$imported}");
        if ($errors > 0) {
            $this->warn("Errors encountered: {$errors}");
        }

        // Show summary by user_id
        $summary = DB::table('user_menu_items')
            ->selectRaw('user_id, COUNT(*) as count')
            ->groupBy('user_id')
            ->get();

        $this->info("\nSummary by user_id:");
        foreach ($summary as $row) {
            $this->info("user_id {$row->user_id}: {$row->count} menu items");
        }

        return 0;
    }
}
