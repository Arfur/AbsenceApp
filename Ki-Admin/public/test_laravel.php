<?php
echo "Testing Laravel bootstrap...\n";

try {
    echo "1. Loading autoloader...\n";
    require_once __DIR__ . '/../vendor/autoload.php';
    echo "✅ Autoloader loaded\n";
    
    echo "2. Loading Laravel app...\n";
    $app = require_once __DIR__ . '/../bootstrap/app.php';
    echo "✅ Laravel app loaded\n";
    
    echo "3. Bootstrapping Laravel kernel...\n";
    $kernel = $app->make('Illuminate\Contracts\Console\Kernel');
    $kernel->bootstrap();
    echo "✅ Laravel kernel bootstrapped\n";
    
    echo "4. Testing notification class...\n";
    $notification = new App\Notifications\OtpVerificationNotification('12345');
    echo "✅ Notification class instantiated\n";
    
    echo "5. Testing mailable class...\n";
    $mailable = new App\Mail\OtpVerificationMail('testuser', '12345');
    echo "✅ Mailable class instantiated\n";
    echo "Mailable type: " . get_class($mailable) . "\n";
    
    echo "6. Testing notification toMail...\n";
    $user = new stdClass();
    $user->username = 'testuser';
    $result = $notification->toMail($user);
    echo "✅ toMail executed\n";
    echo "Result type: " . get_class($result) . "\n";
    
    if ($result instanceof App\Mail\OtpVerificationMail) {
        echo "🎉 SUCCESS: Custom mailable is working!\n";
        echo "Username: " . $result->username . "\n";
        echo "Formatted Code: " . $result->formattedCode . "\n";
    } else {
        echo "❌ FAIL: Still returning wrong type\n";
    }
    
} catch (Exception $e) {
    echo "❌ ERROR: " . $e->getMessage() . "\n";
    echo "File: " . $e->getFile() . ":" . $e->getLine() . "\n";
}
