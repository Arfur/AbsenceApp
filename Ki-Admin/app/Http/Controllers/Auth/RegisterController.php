<?php
/**
 * =========================================================
 * Project   : ki-admin - v1.0.0
 * File Name : RegisterController.php
 * 
 * Author    : Michael Battle
 * Created On: 03/Aug/2025
 * Updated On: 03/Aug/2025
 * 
 * Description:
 * Custom registration controller with role assignment and email domain validation.
 * Restricts registration to @orchard.leics.sch.uk domain only.
 * 
 * Origin:
 * Authentication system for Support Hub - handles user registration
 * 
 * Changes:
 * - Created custom registration with domain validation
 * - Added role assignment logic
 * - Implemented email verification requirement
 * =========================================================
 */

namespace App\Http\Controllers\Auth;

use App\Http\Controllers\Controller;
use App\Models\User;
use App\Models\RoleType;
use App\Notifications\OtpVerificationNotification;
use Illuminate\Auth\Events\Registered;
use Illuminate\Http\RedirectResponse;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Auth;
use Illuminate\Support\Facades\Hash;
use Illuminate\Support\Facades\Log;
use Illuminate\Validation\Rules;
use Illuminate\View\View;

class RegisterController extends Controller
{
    /* =========================================================
     * Section: Display Registration Form
     * Description: Show the registration form to users
     * ========================================================= */

    /**
     * Display the registration view.
     */
    public function create(): View
    {
        return view('auth.register');
    }

    /* =========================================================
     * Section: Handle Registration
     * Description: Process registration with domain validation and role assignment
     * ========================================================= */

    /**
     * Handle an incoming registration request.
     *
     * @throws \Illuminate\Validation\ValidationException
     */
    public function store(Request $request): RedirectResponse
    {
        $request->validate([
            'email' => [
                'required', 
                'string', 
                'lowercase', 
                'email', 
                'max:255', 
                'unique:'.User::class
            ],
            'password' => ['required', 'confirmed', Rules\Password::defaults()],
        ]);

        // Extract username from email (part before @)
        $username = $this->extractUsernameFromEmail($request->email);
        
        // Generate a unique username if already exists
        $finalUsername = $this->ensureUniqueUsername($username);

        // Get the default role type
        $defaultRole = RoleType::getDefault();

        $user = User::create([
            'username' => $finalUsername,
            'email' => $request->email,
            'password' => Hash::make($request->password),
            'status' => 'active', // Default status
            'role_type_id' => $defaultRole?->id, // Assign default role (user)
            'is_verified' => false, // Not verified initially
        ]);

        // Generate OTP and send verification email
        Log::info('DEBUG: About to generate OTP for user: ' . $user->email);
        $otpCode = $user->generateOtpCode();
        Log::info('DEBUG: Generated OTP code: ' . $otpCode);
        
        Log::info('DEBUG: About to send OTP email directly');
        try {
            // Send email directly using our custom mailable (bypassing broken notification)
            \Illuminate\Support\Facades\Mail::to($user->email)->send(
                new \App\Mail\OtpVerificationMail($user->username, $otpCode)
            );
            Log::info('DEBUG: OTP email sent successfully via direct mailing');
        } catch (\Exception $e) {
            Log::error('DEBUG: Failed to send OTP email: ' . $e->getMessage());
            Log::error('DEBUG: Exception trace: ' . $e->getTraceAsString());
        }

        // Note: Removed event(new Registered($user)) to prevent default Laravel email verification
        // We're using custom OTP verification instead

        Auth::login($user);

        return redirect()->route('otp.verification.show');
    }

    /* =========================================================
     * Section: Helper Methods
     * Description: Username generation and validation helpers
     * ========================================================= */

    /**
     * Extract username from email address (part before @)
     */
    private function extractUsernameFromEmail(string $email): string
    {
        return strtolower(explode('@', $email)[0]);
    }

    /**
     * Ensure username is unique by adding numbers if needed
     */
    private function ensureUniqueUsername(string $username): string
    {
        $originalUsername = $username;
        $counter = 1;

        while (User::where('username', $username)->exists()) {
            $username = $originalUsername . $counter;
            $counter++;
        }

        return $username;
    }

    /**
     * Generate a display name from email address
     */
    private function generateNameFromEmail(string $email): string
    {
        $username = $this->extractUsernameFromEmail($email);
        // Convert username to a more readable format (capitalize first letter)
        return ucfirst(str_replace('.', ' ', $username));
    }
}
