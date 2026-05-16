<?php

namespace App\Console\Commands;

use Illuminate\Console\Command;
use Illuminate\Support\Facades\DB;

class CheckForeignKeys extends Command
{
    protected $signature = 'check:foreign-keys';
    protected $description = 'Check foreign key constraints on user_menu_items table';

    public function handle()
    {
        $this->info('=== CHECKING FOREIGN KEY CONSTRAINTS ===');
        
        try {
            // Check table structure
            $columns = DB::select("DESCRIBE user_menu_items");
            $this->info("user_menu_items table structure:");
            foreach ($columns as $column) {
                $this->line("  {$column->Field}: {$column->Type} {$column->Null} {$column->Key} {$column->Default} {$column->Extra}");
            }
            
            $this->info("\nForeign key constraints:");
            $foreignKeys = DB::select("
                SELECT 
                    COLUMN_NAME,
                    CONSTRAINT_NAME,
                    REFERENCED_TABLE_NAME,
                    REFERENCED_COLUMN_NAME
                FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE 
                WHERE TABLE_SCHEMA = DATABASE() 
                AND TABLE_NAME = 'user_menu_items' 
                AND REFERENCED_TABLE_NAME IS NOT NULL
            ");
            
            if (empty($foreignKeys)) {
                $this->line("  No foreign key constraints found");
            } else {
                foreach ($foreignKeys as $fk) {
                    $this->line("  {$fk->COLUMN_NAME} -> {$fk->REFERENCED_TABLE_NAME}.{$fk->REFERENCED_COLUMN_NAME}");
                }
            }
            
            // Check if the values we're trying to insert exist
            $this->info("\nChecking referenced values:");
            
            // Check if user_id 1 exists
            $userExists = DB::table('users')->where('user_id', 1)->exists();
            $this->line("  User with user_id=1 exists: " . ($userExists ? 'YES' : 'NO'));
            
            // Check if menu_item_id 1 exists  
            $menuExists = DB::table('menu_items')->where('id', 1)->exists();
            $this->line("  MenuItem with id=1 exists: " . ($menuExists ? 'YES' : 'NO'));
            
        } catch (\Exception $e) {
            $this->error("Error: " . $e->getMessage());
        }
        
        return 0;
    }
}
