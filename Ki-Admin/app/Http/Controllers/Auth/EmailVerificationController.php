<?php
/**
 * =========================================================
 * Project   : ki-admin - v1.0.0
 * File Name : EmailVerificationController.php
 * 
 * Author    : Michael Battle
 * Created On: 03/Aug/2025
 * Updated On: 03/Aug/2025
 * 
 * Description:
 * Email verification controller for handling email verification process.
 * Manages verification notices, resending verification emails, and verification.
 * 
 * Origin:
 * Authentication system for Support Hub - email verification
 * 
 * Changes:
 * - Created email verification controller
 * - Added verification notice and resend functionality
 * - Implemented verification process
 * =========================================================
 */

namespace App\Http\Controllers\Auth;

use App\Http\Controllers\Controller;
use Illuminate\Auth\Events\Verified;
use Illuminate\Foundation\Auth\EmailVerificationRequest;
use Illuminate\Http\RedirectResponse;
use Illuminate\Http\Request;
use Illuminate\View\View;

class EmailVerificationController extends Controller
{
    /* =========================================================
     * Section: Verification Notice
     * Description: Display email verification notice
     * ========================================================= */

    /**
     * Display the email verification prompt.
     */
    public function notice(Request $request): RedirectResponse|View
    {
        return $request->user()->hasVerifiedEmail()
                    ? redirect()->intended(route('dashboard'))
                    : view('auth.verify-email');
    }

    /* =========================================================
     * Section: Verify Email
     * Description: Mark the authenticated user's email address as verified
     * ========================================================= */

    /**
     * Mark the authenticated user's email address as verified.
     */
    public function verify(EmailVerificationRequest $request): RedirectResponse
    {
        if ($request->user()->hasVerifiedEmail()) {
            return redirect()->intended(route('dashboard').'?verified=1');
        }

        if ($request->user()->markEmailAsVerified()) {
            event(new Verified($request->user()));
        }

        return redirect()->intended(route('dashboard').'?verified=1');
    }

    /* =========================================================
     * Section: Resend Verification
     * Description: Send a new email verification notification
     * ========================================================= */

    /**
     * Send a new email verification notification.
     */
    public function resend(Request $request): RedirectResponse
    {
        if ($request->user()->hasVerifiedEmail()) {
            return redirect()->intended(route('dashboard'));
        }

        $request->user()->sendEmailVerificationNotification();

        return back()->with('status', 'verification-link-sent');
    }
}
