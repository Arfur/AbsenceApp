<?php
echo "Testing direct mailable approach...\n";

require_once __DIR__ . '/../vendor/autoload.php';
$app = require_once __DIR__ . '/../bootstrap/app.php';
$kernel = $app->make('Illuminate\Contracts\Console\Kernel');
$kernel->bootstrap();

echo "1. Creating mailable directly...\n";
$mailable = new App\Mail\OtpVerificationMail('mbattle', '12345');
echo "✅ Mailable created: " . get_class($mailable) . "\n";

echo "2. Testing Mail::send directly...\n";
try {
    // Send email directly using Laravel's Mail facade
    \Illuminate\Support\Facades\Mail::to('michael.battle@proton.me')->send($mailable);
    echo "✅ Email sent successfully using direct mailable!\n";
    echo "🎉 Check your email - it should have the custom template with large OTP code!\n";
} catch (Exception $e) {
    echo "❌ Error sending email: " . $e->getMessage() . "\n";
}

echo "\n3. For comparison, let's see what the notification returns...\n";
$notification = new App\Notifications\OtpVerificationNotification('54321');
$user = new stdClass();
$user->username = 'mbattle';
$result = $notification->toMail($user);
echo "Notification returns: " . get_class($result) . "\n";

echo "\nDone!\n";
