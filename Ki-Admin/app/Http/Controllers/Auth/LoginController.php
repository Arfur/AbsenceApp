<?php
/**
 * =========================================================
 * Project   : ki-admin - v1.0.0
 * File Name : LoginController.php
 * 
 * Author    : Michael Battle
 * Created On: 03/Aug/2025
 * Updated On: 03/Aug/2025
 * 
 * Description:
 * Custom login controller with role-based redirects and authentication.
 * Handles login, logout, and role-based post-login routing.
 * 
 * Origin:
 * Authentication system for Support Hub - handles user login/logout
 * 
 * Changes:
 * - Created custom login with role-based redirects
 * - Added email verification check
 * - Implemented logout functionality
 * =========================================================
 */

namespace App\Http\Controllers\Auth;

use App\Http\Controllers\Controller;
use App\Http\Requests\Auth\LoginRequest;
use Illuminate\Http\RedirectResponse;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Auth;
use Illuminate\View\View;

class LoginController extends Controller
{
    /* =========================================================
     * Section: Display Login Form
     * Description: Show the login form to users
     * ========================================================= */

    /**
     * Display the login view.
     */
    public function create(): View
    {
        return view('auth.login');
    }

    /* =========================================================
     * Section: Handle Authentication
     * Description: Process login attempts with role-based redirects
     * ========================================================= */

    /**
     * Handle an incoming authentication request.
     */
    public function store(LoginRequest $request): RedirectResponse
    {
        $request->authenticate();

        $request->session()->regenerate();

        return $this->redirectUserBasedOnRole();
    }

    /* =========================================================
     * Section: Handle Logout
     * Description: Process logout and session cleanup
     * ========================================================= */

    /**
     * Destroy an authenticated session.
     */
    public function destroy(Request $request): RedirectResponse
    {
        Auth::guard('web')->logout();

        $request->session()->invalidate();

        $request->session()->regenerateToken();

        return redirect('/');
    }

    /* =========================================================
     * Section: Role-Based Routing
     * Description: Redirect users to appropriate dashboard based on role
     * ========================================================= */

    /**
     * Redirect user to appropriate dashboard based on their role
     */
    private function redirectUserBasedOnRole(): RedirectResponse
    {
        $user = Auth::user();
        // Check if email is verified
        if (!$user->hasVerifiedEmail()) {
            return redirect()->route('verification.notice');
        }
        // Always redirect to main dashboard
        return redirect()->route('dashboard');
    }
}
