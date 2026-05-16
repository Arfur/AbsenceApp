<?php
/**
 * =========================================================
 * Project   : ki-admin - v1.0.0
 * File Name : app.php
 * 
 * Author    : Michael Battle
 * Created On: 05/Aug/2025
 * Updated On: 05/Aug/2025
 * 
 * Description:
 * Laravel 11 application bootstrap configuration file.
 * Configures routing, middleware aliases, and exception handling
 * for the ki-admin Support Hub application.
 * 
 * Origin:
 * Main application configuration and middleware registration
 * 
 * Changes:
 * - Added RoleMiddleware alias for role-based access control
 * - Added OtpVerified middleware for two-factor authentication
 * - Added MenuAccessMiddleware for dynamic menu protection
 * - Configured standard Laravel 11 routing and health check
 * =========================================================
 */

use Illuminate\Foundation\Application;
use Illuminate\Foundation\Configuration\Exceptions;
use Illuminate\Foundation\Configuration\Middleware;

/* =====================================================
 * Section: Application Configuration
 * Description: Main Laravel application setup and middleware registration
 * ===================================================== */

return Application::configure(basePath: dirname(__DIR__))
    ->withRouting(
        web: __DIR__.'/../routes/web.php',
        commands: __DIR__.'/../routes/console.php',
        health: '/up',
    )
    /* =====================================================
     * Section: Middleware Aliases
     * Description: Custom middleware registration for authentication and access control
     * ===================================================== */
    ->withMiddleware(function (Middleware $middleware) {
        $middleware->alias([
            'role' => \App\Http\Middleware\RoleMiddleware::class,
            'otp.verified' => \App\Http\Middleware\OtpVerified::class,
            'menu.access' => \App\Http\Middleware\MenuAccessMiddleware::class,
        ]);
    })
    /* =====================================================
     * Section: Exception Handling
     * Description: Global exception handling configuration
     * ===================================================== */
    ->withExceptions(function (Exceptions $exceptions) {
        //
    })->create();
