<?php

namespace App\Http\Controllers\Auth;

use App\Http\Controllers\Controller;
use App\Models\User;
use App\Notifications\OtpVerificationNotification;
use Illuminate\Http\Request;
use Illuminate\Http\RedirectResponse;
use Illuminate\View\View;
use Illuminate\Support\Facades\Auth;
use Illuminate\Support\Facades\Session;

class OtpVerificationController extends Controller
{
    /**
     * Display the OTP verification form.
     */
    public function show()
    {
        // Check if user is authenticated and needs verification
        if (!Auth::check()) {
            return redirect()->route('login')->with('error', 'Please login first.');
        }

        $user = Auth::user();
        
        if ($user->is_verified) {
            return redirect()->route('dashboard')->with('success', 'Email already verified.');
        }

        return view('auth.two_step_verification', [
            'email' => $user->email,
            'otp_expires_at' => $user->otp_expires_at,
        ]);
    }

    /**
     * Verify the OTP code.
     */
    public function verify(Request $request): RedirectResponse
    {
        $request->validate([
            'otp_code' => ['required', 'string', 'size:6'],
        ]);

        $user = Auth::user();

        if (!$user) {
            return redirect()->route('login')->with('error', 'Please login first.');
        }

        if ($user->is_verified) {
            return redirect()->route('dashboard')->with('success', 'Email already verified.');
        }

        // Check if OTP is expired
        if ($user->isOtpExpired()) {
            return back()->with('error', 'Verification code has expired. Please request a new one.');
        }

        // Verify the OTP code
        if (!$user->verifyOtpCode($request->otp_code)) {
            return back()->with('error', 'Invalid verification code. Please try again.');
        }

        // Mark user as verified
        $user->markAsVerified();

        // Store username for the success message
        $username = $user->username;

        // Logout the user so they can login with verified credentials
        Auth::logout();

        // Clear the session
        $request->session()->invalidate();
        $request->session()->regenerateToken();

        return redirect()->route('login')->with('success', "Email verified successfully!\nPlease login using '{$username}' and your password.");
    }

    /**
     * Resend the OTP verification code.
     */
    public function resend(Request $request): RedirectResponse
    {
        $user = Auth::user();

        if (!$user) {
            return redirect()->route('login')->with('error', 'Please login first.');
        }

        if ($user->is_verified) {
            return redirect()->route('dashboard')->with('success', 'Email already verified.');
        }

        // Generate new OTP
        $otpCode = $user->generateOtpCode();

        // Send OTP email directly using our custom mailable
        try {
            \Illuminate\Support\Facades\Mail::to($user->email)->send(
                new \App\Mail\OtpVerificationMail($user->username, $otpCode)
            );
        } catch (\Exception $e) {
            return back()->with('error', 'Failed to send verification email. Please try again.');
        }

        return back()->with('success', 'New verification code sent to your email.');
    }
}
