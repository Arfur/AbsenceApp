<?php

require_once 'vendor/autoload.php';
$app = require_once 'bootstrap/app.php';
$app->make('Illuminate\Contracts\Console\Kernel')->bootstrap();

use App\Notifications\OtpVerificationNotification;

echo "=== DEBUGGING OTP NOTIFICATION ===\n";

try {
    echo "1. Creating notification with OTP code '12345'...\n";
    $notification = new OtpVerificationNotification('12345');
    echo "✅ Notification created successfully\n";
    
    echo "\n2. Creating fake user object...\n";
    $fakeUser = new stdClass();
    $fakeUser->username = 'testuser';
    $fakeUser->email = 'test@example.com';
    echo "✅ Fake user created\n";
    
    echo "\n3. Calling toMail method...\n";
    $mailMessage = $notification->toMail($fakeUser);
    echo "✅ Mail message generated\n";
    echo "Message type: " . get_class($mailMessage) . "\n";
    
    echo "\n4. Checking notification implementation...\n";
    $notificationFile = file_get_contents('app/Notifications/OtpVerificationNotification.php');
    if (strpos($notificationFile, 'OtpVerificationMail') !== false) {
        echo "✅ Notification uses custom mailable\n";
    } else {
        echo "❌ Notification does NOT use custom mailable\n";
    }
    
    echo "\n5. Checking if custom email template exists...\n";
    if (file_exists('resources/views/emails/otp-verification.blade.php')) {
        echo "✅ Custom email template exists\n";
    } else {
        echo "❌ Custom email template NOT found\n";
    }
    
    echo "\n6. Checking if custom mailable class exists...\n";
    if (file_exists('app/Mail/OtpVerificationMail.php')) {
        echo "✅ Custom mailable class exists\n";
    } else {
        echo "❌ Custom mailable class does NOT exist\n";
    }
    
} catch (Exception $e) {
    echo "❌ ERROR: " . $e->getMessage() . "\n";
    echo "File: " . $e->getFile() . ":" . $e->getLine() . "\n";
    echo "Trace: " . $e->getTraceAsString() . "\n";
}

echo "\n=== END DEBUG ===\n";
