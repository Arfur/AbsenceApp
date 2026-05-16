<?php
/**
 * =========================================================
 * Project   : ki-admin - v1.0.0
 * File Name : RoleMiddleware.php
 * 
 * Author    : Michael Battle
 * Created On: 03/Aug/2025
 * Updated On: 03/Aug/2025
 * 
 * Description:
 * Role-based access control middleware for restricting routes.
 * Checks if authenticated user has required role(s) for access.
 * 
 * Origin:
 * Authentication and authorization system for Support Hub
 * 
 * Changes:
 * - Created role-based middleware
 * - Added support for multiple roles
 * - Implemented proper error handling
 * =========================================================
 */

namespace App\Http\Middleware;

use Closure;
use Illuminate\Http\Request;
use Symfony\Component\HttpFoundation\Response;

class RoleMiddleware
{
    /* =========================================================
     * Section: Handle Request
     * Description: Check if user has required role(s) for access
     * ========================================================= */

    /**
     * Handle an incoming request.
     *
     * @param  \Closure(\Illuminate\Http\Request): (\Symfony\Component\HttpFoundation\Response)  $next
     * @param  string  ...$roles
     */
    public function handle(Request $request, Closure $next, string ...$roles): Response
    {
        if (!auth()->check()) {
            return redirect()->route('login');
        }

        $user = auth()->user();
        
        // If no roles specified, just check if user is authenticated
        if (empty($roles)) {
            return $next($request);
        }
        
        // Check if user has any of the required roles
        foreach ($roles as $role) {
            if ($user->hasRole($role)) {
                return $next($request);
            }
        }

        // User doesn't have required role - redirect with error
        abort(403, 'You do not have permission to access this page.');
    }
}
