<?php
echo "Testing custom OTP email to Gmail...\n";

require_once __DIR__ . '/../vendor/autoload.php';
$app = require_once __DIR__ . '/../bootstrap/app.php';
$kernel = $app->make('Illuminate\Contracts\Console\Kernel');
$kernel->bootstrap();

echo "1. Creating custom mailable for test...\n";
$testOtpCode = '15451'; // Your requested test code
$mailable = new App\Mail\OtpVerificationMail('mbattle', $testOtpCode);
echo "✅ Mailable created with code: {$testOtpCode}\n";

echo "2. Sending test email to your Gmail address...\n";
try {
    // Send to your Gmail address from .env file
    \Illuminate\Support\Facades\Mail::to('mbattle@orchardprimary.org')->send($mailable);
    echo "✅ Test email sent successfully!\n";
    echo "📧 Check your Gmail inbox (mbattle@orchardprimary.org)\n";
    echo "🎯 You should see:\n";
    echo "   - Subject: 'Your Support Hub Verification Code'\n";
    echo "   - Large OTP code: '1 5 4 5 1' (36px font)\n";
    echo "   - Message: 'Dear mbattle, Your one-time verification code:'\n";
    echo "   - Fun text: 'biscuit at breaktime'\n";
    echo "   - 15-minute expiry notice\n";
} catch (Exception $e) {
    echo "❌ Error sending test email: " . $e->getMessage() . "\n";
    echo "Details: " . $e->getFile() . ":" . $e->getLine() . "\n";
}

echo "\nDone! Check your Gmail inbox.\n";
