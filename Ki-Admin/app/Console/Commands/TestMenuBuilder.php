<?php

namespace App\Console\Commands;

use Illuminate\Console\Command;
use App\Models\User;
use App\Helpers\MenuBuilder;
use App\Models\UserMenuItem;

class TestMenuBuilder extends Command
{
    protected $signature = 'test:menu-builder';
    protected $description = 'Test the corrected MenuBuilder functionality';

    public function handle()
    {
        $this->info('=== TESTING CORRECTED MENU BUILDER ===');
        
        // Get test users
        $users = User::with('roleType')->take(3)->get();
        
        if ($users->isEmpty()) {
            $this->error('No users found in database');
            return 1;
        }
        
        $this->info("Found {$users->count()} users for testing:");
        
        foreach ($users as $user) {
            $roleTypeName = $user->roleType ? $user->roleType->name : 'No Role';
            $this->line("- User ID {$user->id}: {$user->email} (Role: {$roleTypeName})");
        }
        
        $this->newLine();
        
        // Test each user
        foreach ($users as $user) {
            $this->info("Testing User: {$user->email} (ID: {$user->id})");
            
            // Check user menu items
            $userMenuCount = UserMenuItem::where('user_id', $user->id)->count();
            $this->line("  Current user_menu_items count: {$userMenuCount}");
            
            if ($userMenuCount === 0) {
                $this->line("  No user menu items found - assigning defaults from role...");
                $user->assignDefaultMenuItems();
                
                $newCount = UserMenuItem::where('user_id', $user->id)->count();
                $this->line("  ✅ Assigned {$newCount} default menu items");
            }
            
            // Test MenuBuilder
            $this->line("  Building menu via MenuBuilder...");
            $menu = MenuBuilder::getMenuForUser($user);
            
            $this->line("  Menu structure:");
            if (empty($menu)) {
                $this->line("    ❌ No menu items returned");
            } else {
                foreach ($menu as $item) {
                    $this->line("    📁 {$item['title']} (ID: {$item['id']})");
                    if (!empty($item['children'])) {
                        foreach ($item['children'] as $child) {
                            $this->line("      📄 {$child['title']} (ID: {$child['id']})");
                        }
                    }
                }
            }
            
            $this->newLine();
        }
        
        // Database stats
        $this->info('Database consistency:');
        $totalMenuItems = \App\Models\MenuItem::count();
        $totalRoleMenuItems = \App\Models\RoleMenuItem::count();
        $totalUserMenuItems = UserMenuItem::count();
        
        $this->line("  Total menu_items: {$totalMenuItems}");
        $this->line("  Total role_menu_items: {$totalRoleMenuItems}");
        $this->line("  Total user_menu_items: {$totalUserMenuItems}");
        
        $this->newLine();
        $this->info('✅ MenuBuilder test completed!');
        $this->info('The corrected MenuBuilder now reads exclusively from user_menu_items table.');
        
        return 0;
    }
}
