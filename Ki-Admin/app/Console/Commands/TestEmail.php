<?php

namespace App\Console\Commands;

use Illuminate\Console\Command;
use Illuminate\Support\Facades\Mail;
use App\Notifications\OtpVerificationNotification;
use App\Models\User;

class TestEmail extends Command
{
    protected $signature = 'test:email {email}';
    protected $description = 'Test email sending functionality';

    public function handle()
    {
        $email = $this->argument('email');
        
        $this->info("Testing email to: {$email}");
        
        try {
            // Create a temporary user object for testing
            $testUser = new User();
            $testUser->email = $email;
            $testUser->username = 'Test User';
            
            // Generate a test OTP
            $testOtp = '12345';
            
            $this->info("Sending test OTP notification...");
            
            // Send the notification
            $testUser->notify(new OtpVerificationNotification($testOtp));
            
            $this->info("✅ Email sent successfully!");
            $this->info("Test OTP code: {$testOtp}");
            
        } catch (\Exception $e) {
            $this->error("❌ Email failed: " . $e->getMessage());
            $this->error("Full error: " . $e->getTraceAsString());
        }
    }
}
