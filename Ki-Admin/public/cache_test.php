<?php
echo "Checking PHP cache status...\n";
if (function_exists('opcache_get_status')) {
    $status = opcache_get_status();
    if ($status && $status['opcache_enabled']) {
        echo "OPcache is enabled - resetting...\n";
        opcache_reset();
        echo "OPcache reset complete\n";
    } else {
        echo "OPcache is disabled\n";
    }
} else {
    echo "OPcache not available\n";
}

echo "\nTesting notification after cache clear...\n";
require_once __DIR__ . '/../vendor/autoload.php';
$app = require_once __DIR__ . '/../bootstrap/app.php';
$kernel = $app->make('Illuminate\Contracts\Console\Kernel');
$kernel->bootstrap();

$notification = new App\Notifications\OtpVerificationNotification('54321');
$user = new stdClass();
$user->username = 'mbattle';
$result = $notification->toMail($user);

echo "Result type: " . get_class($result) . "\n";
if ($result instanceof App\Mail\OtpVerificationMail) {
    echo "🎉 SUCCESS: Custom mailable is now working!\n";
} else {
    echo "❌ FAIL: Still returning " . get_class($result) . "\n";
}
