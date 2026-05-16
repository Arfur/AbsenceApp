<?php

namespace App\Console\Commands;

use Illuminate\Console\Command;
use App\Models\User;
use App\Models\RoleMenuItem;
use App\Models\UserMenuItem;

class TestSpecificAssignment extends Command
{
    protected $signature = 'test:specific-assignment';
    protected $description = 'Test assignment with specific debug output';

    public function handle()
    {
        $this->info('=== TESTING SPECIFIC ASSIGNMENT ===');
        
        $user = User::first();
        $this->info("User: {$user->email}");
        $this->info("  id: {$user->id}");
        $this->info("  user_id: {$user->user_id}");
        $this->info("  role_type_id: {$user->role_type_id}");
        
        // Get ONE role menu item
        $roleMenuItem = RoleMenuItem::where('role_type_id', $user->role_type_id)
            ->where('is_granted', true)
            ->where('status', 'active')
            ->first();
            
        if (!$roleMenuItem) {
            $this->error("No role menu items found");
            return 1;
        }
        
        $this->info("\nTesting with RoleMenuItem:");
        $this->line("  id: {$roleMenuItem->id}");
        $this->line("  role_type_id: {$roleMenuItem->role_type_id}");
        $this->line("  menu_item_id: {$roleMenuItem->menu_item_id}");
        $this->line("  is_granted: " . ($roleMenuItem->is_granted ? 'true' : 'false'));
        $this->line("  status: {$roleMenuItem->status}");
        
        // Clear any existing for this test
        UserMenuItem::where('user_id', $user->user_id)
            ->where('menu_item_id', $roleMenuItem->menu_item_id)
            ->delete();
            
        $this->info("\nAttempting to create UserMenuItem...");
        $this->line("  user_id: {$user->user_id}");
        $this->line("  menu_item_id: {$roleMenuItem->menu_item_id}");
        
        try {
            $userMenuItem = UserMenuItem::create([
                'user_id' => $user->user_id,
                'menu_item_id' => $roleMenuItem->menu_item_id,
                'is_granted' => true,
                'custom_order' => 0
            ]);
            
            $this->info("✅ SUCCESS! UserMenuItem created with ID: {$userMenuItem->id}");
            
            // Verify it exists
            $found = UserMenuItem::where('user_id', $user->user_id)
                ->where('menu_item_id', $roleMenuItem->menu_item_id)
                ->first();
                
            if ($found) {
                $this->info("✅ VERIFIED: UserMenuItem found in database");
            } else {
                $this->error("❌ VERIFICATION FAILED: UserMenuItem not found");
            }
            
        } catch (\Exception $e) {
            $this->error("❌ FAILED: " . $e->getMessage());
            $this->line("Stack trace:");
            $this->line($e->getTraceAsString());
        }
        
        return 0;
    }
}
