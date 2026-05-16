<?php

namespace App\Console\Commands;

use Illuminate\Console\Command;
use App\Models\User;

class CheckUserIds extends Command
{
    protected $signature = 'check:user-ids';
    protected $description = 'Check the difference between id and user_id fields';

    public function handle()
    {
        $this->info('=== CHECKING USER ID vs USER_ID ===');
        
        $users = User::take(3)->get();
        
        foreach ($users as $user) {
            $this->line("User: {$user->email}");
            $this->line("  id: {$user->id}");
            $this->line("  user_id: {$user->user_id}");
            $this->line("  role_type_id: {$user->role_type_id}");
            $this->line("");
        }
        
        // Check what UserMenuItem expects
        $this->info("Checking UserMenuItem table structure...");
        $userMenuItem = \App\Models\UserMenuItem::first();
        if ($userMenuItem) {
            $this->line("Sample UserMenuItem:");
            $this->line("  user_id field value: {$userMenuItem->user_id}");
            
            // Find which user this belongs to
            $matchingUser = User::where('user_id', $userMenuItem->user_id)->first();
            if ($matchingUser) {
                $this->line("  This matches User with email: {$matchingUser->email}");
                $this->line("  That user's id: {$matchingUser->id}");
                $this->line("  That user's user_id: {$matchingUser->user_id}");
            }
        }
        
        return 0;
    }
}
