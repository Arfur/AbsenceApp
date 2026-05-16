<?php

namespace App\Console\Commands;

use Illuminate\Console\Command;
use App\Models\User;

class TestAssignMenus extends Command
{
    protected $signature = 'test:assign-menus';
    protected $description = 'Test the improved assignDefaultMenuItems functionality';

    public function handle()
    {
        $this->info('=== TESTING IMPROVED assignDefaultMenuItems ===');
        
        // Get a test user
        $user = User::with('roleType')->first();
        
        if (!$user) {
            $this->error('No users found in database');
            return 1;
        }
        
        $this->info("Testing with user: {$user->email} (role_type_id: {$user->role_type_id})");
        
        // Count before
        $beforeCount = \App\Models\UserMenuItem::where('user_id', $user->id)->count();
        $this->line("User menu items before: {$beforeCount}");
        
        // Clear existing user menu items for testing
        \App\Models\UserMenuItem::where('user_id', $user->id)->delete();
        $this->line("Cleared existing user menu items");
        
        // Test the improved method
        $this->line("Running assignDefaultMenuItems()...");
        $user->assignDefaultMenuItems();
        
        // Count after
        $afterCount = \App\Models\UserMenuItem::where('user_id', $user->id)->count();
        $this->line("User menu items after: {$afterCount}");
        
        if ($afterCount > 0) {
            $this->info("✅ SUCCESS: Assigned {$afterCount} menu items!");
            
            // Test MenuBuilder
            $this->line("Testing MenuBuilder with assigned items...");
            $menu = \App\Helpers\MenuBuilder::getMenuForUser($user);
            $this->line("MenuBuilder returned " . count($menu) . " top-level menu items");
            
        } else {
            $this->error("❌ FAILED: No menu items were assigned");
            
            // Debug info
            $roleMenuCount = \App\Models\RoleMenuItem::where('role_type_id', $user->role_type_id)
                ->where('is_granted', true)
                ->where('status', 'active')
                ->count();
            $this->line("Debug: Found {$roleMenuCount} role menu items for role_type_id {$user->role_type_id}");
        }
        
        return 0;
    }
}
