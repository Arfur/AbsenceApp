<?php

namespace App\Console\Commands;

use Illuminate\Console\Command;
use App\Models\User;
use App\Models\RoleMenuItem;

class DebugRoleMenuItem extends Command
{
    protected $signature = 'debug:role-menu-item';
    protected $description = 'Debug RoleMenuItem structure';

    public function handle()
    {
        $this->info('=== DEBUGGING ROLE MENU ITEM STRUCTURE ===');
        
        $user = User::first();
        $this->info("User role_type_id: {$user->role_type_id}");
        
        // Get first role menu item for this user's role
        $roleMenuItem = RoleMenuItem::where('role_type_id', $user->role_type_id)
            ->where('is_granted', true)
            ->where('status', 'active')
            ->first();
            
        if ($roleMenuItem) {
            $this->info("Found RoleMenuItem:");
            $this->line("  ID: {$roleMenuItem->id}");
            $this->line("  role_type_id: {$roleMenuItem->role_type_id}");
            $this->line("  menu_item_id: {$roleMenuItem->menu_item_id}");
            $this->line("  is_granted: " . ($roleMenuItem->is_granted ? 'true' : 'false'));
            $this->line("  status: {$roleMenuItem->status}");
            
            // Check if 'order' field exists
            if (isset($roleMenuItem->order)) {
                $this->line("  order: {$roleMenuItem->order}");
            } else {
                $this->line("  order: NOT SET");
            }
            
            // Show all attributes
            $this->line("\nAll attributes:");
            foreach ($roleMenuItem->getAttributes() as $key => $value) {
                $this->line("  {$key}: {$value}");
            }
        } else {
            $this->error("No RoleMenuItem found");
        }
        
        return 0;
    }
}
