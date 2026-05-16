<?php
// Simple test to check if OTP verification view works
echo "Testing OTP Verification View...\n";

// Test if view file exists
$viewPath = __DIR__ . '/../resources/views/auth/two_step_verification.blade.php';
echo "View file exists: " . (file_exists($viewPath) ? "YES" : "NO") . "\n";

// Test if layout files exist
$headPath = __DIR__ . '/../resources/views/layout/head.blade.php';
$cssPath = __DIR__ . '/../resources/views/layout/css.blade.php';
echo "Layout head exists: " . (file_exists($headPath) ? "YES" : "NO") . "\n";
echo "Layout CSS exists: " . (file_exists($cssPath) ? "YES" : "NO") . "\n";

// Test if assets exist
$jqueryPath = __DIR__ . '/../public/assets/js/jquery-3.6.3.min.js';
$bootstrapPath = __DIR__ . '/../public/assets/vendor/bootstrap/bootstrap.bundle.min.js';
echo "jQuery exists: " . (file_exists($jqueryPath) ? "YES" : "NO") . "\n";
echo "Bootstrap exists: " . (file_exists($bootstrapPath) ? "YES" : "NO") . "\n";

echo "\nIf all files exist, the view should work properly.\n";
echo "Navigate to: https://ki-admin.test/otp-verification\n";
echo "(You'll need to be logged in first)\n";
?>
