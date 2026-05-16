<?php

namespace App\Console\Commands;

use Illuminate\Console\Command;
use App\Models\User;
use App\Models\RoleMenuItem;
use App\Models\UserMenuItem;

class DebugMenuAssignmentDetailed extends Command
{
    protected $signature = 'debug:menu-assignment-detailed';
    protected $description = 'Detailed debugging of menu assignment process';

    public function handle()
    {
        $this->info('=== DETAILED MENU ASSIGNMENT DEBUG ===');
        
        // Get a test user
        $user = User::with('roleType')->first();
        
        if (!$user) {
            $this->error('No users found');
            return 1;
        }
        
        $this->info("User: {$user->email} (ID: {$user->id}, Role Type ID: {$user->role_type_id})");
        
        // Step 1: Check role menu items
        $roleMenuItems = RoleMenuItem::where('role_type_id', $user->role_type_id)
            ->where('is_granted', true)
            ->where('status', 'active')
            ->limit(3) // Test with just first 3
            ->get();
            
        $this->info("Found {$roleMenuItems->count()} role menu items (showing first 3)");
        
        foreach ($roleMenuItems as $roleMenuItem) {
            $this->line("  - Role Menu Item ID: {$roleMenuItem->id}, Menu Item ID: {$roleMenuItem->menu_item_id}");
        }
        
        // Step 2: Test manual creation
        $this->info("\nTesting manual UserMenuItem creation...");
        
        $firstRoleMenuItem = $roleMenuItems->first();
        if ($firstRoleMenuItem) {
            try {
                // Clear any existing
                UserMenuItem::where('user_id', $user->id)
                    ->where('menu_item_id', $firstRoleMenuItem->menu_item_id)
                    ->delete();
                
                // Create manually
                $userMenuItem = new UserMenuItem();
                $userMenuItem->user_id = $user->id;
                $userMenuItem->menu_item_id = $firstRoleMenuItem->menu_item_id;
                $userMenuItem->is_granted = true;
                $userMenuItem->custom_order = 0;
                $userMenuItem->save();
                
                $this->info("✅ Manual creation successful - UserMenuItem ID: {$userMenuItem->id}");
                
                // Verify it exists
                $exists = UserMenuItem::where('user_id', $user->id)
                    ->where('menu_item_id', $firstRoleMenuItem->menu_item_id)
                    ->first();
                
                if ($exists) {
                    $this->info("✅ Verification: UserMenuItem exists in database");
                } else {
                    $this->error("❌ Verification failed: UserMenuItem not found");
                }
                
            } catch (\Exception $e) {
                $this->error("❌ Manual creation failed: " . $e->getMessage());
                $this->line("Stack trace: " . $e->getTraceAsString());
            }
        }
        
        // Step 3: Test updateOrCreate
        $this->info("\nTesting updateOrCreate...");
        
        if ($firstRoleMenuItem) {
            try {
                // Clear any existing first
                UserMenuItem::where('user_id', $user->id)
                    ->where('menu_item_id', $firstRoleMenuItem->menu_item_id)
                    ->delete();
                
                $userMenuItem = UserMenuItem::updateOrCreate(
                    [
                        'user_id' => $user->id,
                        'menu_item_id' => $firstRoleMenuItem->menu_item_id
                    ],
                    [
                        'is_granted' => true,
                        'custom_order' => 0
                    ]
                );
                
                $this->info("✅ updateOrCreate successful - UserMenuItem ID: {$userMenuItem->id}");
                $this->info("Was recently created: " . ($userMenuItem->wasRecentlyCreated ? 'Yes' : 'No'));
                
            } catch (\Exception $e) {
                $this->error("❌ updateOrCreate failed: " . $e->getMessage());
                $this->line("Stack trace: " . $e->getTraceAsString());
            }
        }
        
        return 0;
    }
}
