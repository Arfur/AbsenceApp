<?php

namespace App\Console\Commands;

use Illuminate\Console\Command;
use App\Models\User;
use App\Models\RoleMenuItem;
use App\Models\UserMenuItem;

class DebugMenuAssignment extends Command
{
    protected $signature = 'debug:menu-assignment';
    protected $description = 'Debug menu assignment process';

    public function handle()
    {
        $this->info('=== DEBUGGING MENU ASSIGNMENT ===');
        
        $user = User::find(4);
        if (!$user) {
            $this->error('User not found');
            return 1;
        }
        
        $this->info("User: {$user->email}");
        $this->info("Role Type ID: {$user->role_type_id}");
        $this->info("Role Type: " . ($user->roleType ? $user->roleType->name : 'NULL'));
        
        // Check role menu items
        $roleMenuItems = RoleMenuItem::where('role_type_id', $user->role_type_id)
            ->where('is_granted', true)
            ->where('status', 'active')
            ->get();
            
        $this->info("Role menu items found: " . $roleMenuItems->count());
        
        if ($roleMenuItems->count() > 0) {
            $this->line("First 5 role menu items:");
            foreach ($roleMenuItems->take(5) as $item) {
                $this->line("  - Menu Item ID: {$item->menu_item_id}, Granted: {$item->is_granted}, Status: {$item->status}");
            }
        }
        
        // Check total role menu items for this role type
        $totalRoleMenuItems = RoleMenuItem::where('role_type_id', $user->role_type_id)->count();
        $this->info("Total role menu items for this role: {$totalRoleMenuItems}");
        
        // Check if role type exists
        if ($user->roleType) {
            $this->info("Role Type details:");
            $this->line("  - ID: {$user->roleType->id}");
            $this->line("  - Name: {$user->roleType->name}");
            $this->line("  - Slug: {$user->roleType->slug}");
        }
        
        return 0;
    }
}
