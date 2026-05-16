<?php
/* =========================================================
 * File: Kernel.php
 * Project: KI Admin - v1.0.0
 * Description: HTTP kernel with middleware configuration
 * Changes:
 *  1. Added DebugViewData middleware
 *  2. Organized middleware groups with section comments
 *  3. Added role and permission middleware for account management
 * Updated: 2025-08-19 (KI Admin Middleware Setup)
 * ========================================================= */

namespace App\Http;

use Illuminate\Foundation\Http\Kernel as HttpKernel;

class Kernel extends HttpKernel
{
    /* =========================================================
     * Section: Global Middleware
     * Description: Runs on every request
     * ========================================================= */
    protected $middleware = [
        \App\Http\Middleware\TrustProxies::class,
        \Fruitcake\Cors\HandleCors::class,
        \App\Http\Middleware\PreventRequestsDuringMaintenance::class,
        \Illuminate\Foundation\Http\Middleware\ValidatePostSize::class,
        \App\Http\Middleware\TrimStrings::class,
        \Illuminate\Foundation\Http\Middleware\ConvertEmptyStringsToNull::class,
    ];

    /* =========================================================
     * Section: Web Middleware Group
     * Description: Applies to web routes
     * Key Changes:
     *  1. Added DebugViewData middleware
     * ========================================================= */
    protected $middlewareGroups = [
        'web' => [
            \App\Http\Middleware\EncryptCookies::class,
            \Illuminate\Cookie\Middleware\AddQueuedCookiesToResponse::class,
            \Illuminate\Session\Middleware\StartSession::class,
            \Illuminate\View\Middleware\ShareErrorsFromSession::class,
            \App\Http\Middleware\VerifyCsrfToken::class,
            \Illuminate\Routing\Middleware\SubstituteBindings::class,
            \App\Http\Middleware\DebugViewData::class, // Debug data
            // \App\Http\Middleware\InjectUserMenuMiddleware::class, // Temporarily disabled - using ViewServiceProvider instead
        ],

        /* =========================================================
         * Section: API Middleware Group
         * Description: Applies to API routes
         * ========================================================= */
        'api' => [
            'throttle:api',
            \Illuminate\Routing\Middleware\SubstituteBindings::class,
        ],
    ];

    /* =========================================================
     * Section: Route Middleware
     * Description: Individual middleware aliases
     * ========================================================= */
    protected $routeMiddleware = [
        'auth' => \App\Http\Middleware\Authenticate::class,
        'auth.basic' => \Illuminate\Auth\Middleware\AuthenticateWithBasicAuth::class,
        'cache.headers' => \Illuminate\Http\Middleware\SetCacheHeaders::class,
        'can' => \Illuminate\Auth\Middleware\Authorize::class,
        'guest' => \App\Http\Middleware\RedirectIfAuthenticated::class,
        'password.confirm' => \Illuminate\Auth\Middleware\RequirePassword::class,
        'signed' => \Illuminate\Routing\Middleware\ValidateSignature::class,
        'throttle' => \Illuminate\Routing\Middleware\ThrottleRequests::class,
        'verified' => \Illuminate\Auth\Middleware\EnsureEmailIsVerified::class,
        // Custom Middleware
        'password.change' => \App\Http\Middleware\ForcePasswordChange::class,
        'ensure.email' => \App\Http\Middleware\EnsureEmailDomain::class,
        'admin.approval' => \App\Http\Middleware\AdminApproval::class,
        'role' => \App\Http\Middleware\RoleMiddleware::class,
        'permission' => \App\Http\Middleware\PermissionMiddleware::class,
        'admin' => \App\Http\Middleware\AdminMiddleware::class,
    ];
}
