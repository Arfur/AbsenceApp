<?php
namespace Database\Seeders;

use Illuminate\Database\Seeder;
use Illuminate\Support\Facades\DB;


class RoleMenuItemsSeeder extends Seeder
{
    /**
     * Run the database seeds.
     */
    public function run(): void
    {
        // Prefer the 'ki_' CSV in Data Export when available, else fall back
        $preferred = database_path('seeders/Data Export/ki_role_menu_items.csv');
        $fallback = database_path('seeders/Data/role_menu_items.csv');
        if (file_exists($preferred)) {
            $csvFile = $preferred;
        } elseif (file_exists($fallback)) {
            $csvFile = $fallback;
        } else {
            $this->command->error("CSV file not found: checked {$preferred} and {$fallback}");
            return;
        }

        $this->command->info('Reading role menu items data from CSV...');

        // Read and parse CSV
        $csvData = array_map('str_getcsv', file($csvFile));
        $headers = array_shift($csvData); // Remove header row

        $batch = [];
        foreach ($csvData as $row) {
            if (count($row) !== count($headers)) continue;
            $data = array_combine($headers, $row);

            // Convert date fields from DD/MM/YYYY HH:MM to YYYY-MM-DD HH:MM:SS
            $assignedAt = $this->convertToMysqlDateTime($data['assigned_at'] ?? null);
            $createdAt = $this->convertToMysqlDateTime($data['created_at'] ?? null);
            $updatedAt = $this->convertToMysqlDateTime($data['updated_at'] ?? null);

            $record = [
                'role_type_id' => (int)($data['role_type_id'] ?? 0),
                'role_type' => $data['role_type'] ?? null,
                'menu_item_id' => (int)($data['menu_item_id'] ?? 0),
                'is_granted' => ($data['is_granted'] ?? '0') == '1' ? 1 : 0,
                'is_default' => ($data['is_default'] ?? '0') == '1' ? 1 : 0,
                'assigned_at' => $assignedAt,
                'status' => $data['status'] ?? null,
                'notes' => $data['notes'] ?? null,
                'created_at' => $createdAt,
                'updated_at' => $updatedAt,
            ];

            // Use upsert keyed on role_type_id + menu_item_id to be idempotent
            DB::table('role_menu_items')->updateOrInsert([
                'role_type_id' => $record['role_type_id'],
                'menu_item_id' => $record['menu_item_id'],
            ], $record);
        }

        // Display summary
        $userCount = DB::table('role_menu_items')->where('role_type', 'user')->count();
        $adminCount = DB::table('role_menu_items')->where('role_type', 'admin')->count();
        $superAdminCount = DB::table('role_menu_items')->where('role_type', 'super_admin')->count();

        $this->command->info("- User role: {$userCount} menu items");
        $this->command->info("- Admin role: {$adminCount} menu items");
        $this->command->info("- Super Admin role: {$superAdminCount} menu items");
    }

    private function convertToMysqlDateTime($dateString)
    {
        if (empty($dateString)) return null;
        $date = \DateTime::createFromFormat('d/m/Y H:i', $dateString);
        if ($date) {
            return $date->format('Y-m-d H:i:s');
        }
        return $dateString; // fallback to original if parsing fails
    }
}
