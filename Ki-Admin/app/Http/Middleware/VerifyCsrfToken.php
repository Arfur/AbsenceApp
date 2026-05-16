<?php
/**
 * =========================================================
 * Project   : ki-admin - v1.0.0
 * File Name : VerifyCsrfToken.php
 * Author    : Michael Battle
 * Created On: 19/Aug/2025
 * Updated On: 19/Aug/2025
 * Description:
 * Custom CSRF middleware for ki-admin project. Adds temporary debug exception for sidebar JS diagnostics.
 * =========================================================
 */

namespace App\Http\Middleware;

use Illuminate\Foundation\Http\Middleware\VerifyCsrfToken as Middleware;

class VerifyCsrfToken extends Middleware
{
    /**
     * The URIs that should be excluded from CSRF verification.
     * =========================================================
     * TEMPORARY DEBUG: Allow JS debug POST for sidebar expansion diagnostics
     * Remove this exception after debugging is complete.
     * =========================================================
     */
    protected $except = [
        'sidebar-js-debug', // Try without leading slash
        // ...other exceptions...
    ];

    public function handle($request, \Closure $next)
    {
    // Debug: Show request path only
    dd($request->path());
        return parent::handle($request, $next);
    }
}
