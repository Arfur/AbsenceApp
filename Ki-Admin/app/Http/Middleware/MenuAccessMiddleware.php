<?php
/**
 * =========================================================
 * Project   : ki-admin - v1.0.0
 * File Name : MenuAccessMiddleware.php
 * 
 * Author    : Michael Battle
 * Created On: 05/Aug/2025
 * Updated On: 05/Aug/2025
 * 
 * Description:
 * Middleware for protecting routes based on user's menu access permissions.
 * Integrates with role-based menu system to ensure users can only access
 * routes they have permission for via their assigned role.
 * 
 * Origin:
 * Role-based navigation system for ki-admin Support Hub
 * 
 * Changes:
 * - Created MenuAccessMiddleware for route protection
 * - Implemented menu access checking via MenuService
 * - Added role-based route authorization
 * - Foundation prepared for future dynamic permission system
 * =========================================================
 */

namespace App\Http\Middleware;

use Closure;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Auth;
use App\Services\MenuService;
use Symfony\Component\HttpFoundation\Response;

class MenuAccessMiddleware
{
    /* =====================================================
     * Section: Core Middleware Logic
     * Description: Main request handling and access control
     * ===================================================== */

    /**
     * Handle an incoming request.
     *
     * @param  \Closure(\Illuminate\Http\Request): (\Symfony\Component\HttpFoundation\Response)  $next
     */
    public function handle(Request $request, Closure $next, string $menuItem = null): Response
    {
        // Check if user is authenticated
        if (!Auth::check()) {
            return redirect()->route('sign_in')->with('error', 'Please log in to access this page.');
        }

        $user = Auth::user();
        $menuService = new MenuService();

        // If no specific menu item is provided, try to determine from route
        if (!$menuItem) {
            $menuItem = $this->determineMenuItemFromRoute($request);
        }

        // If we still can't determine menu item, allow access (for basic routes)
        if (!$menuItem) {
            return $next($request);
        }

        // Check if user has access to this menu item
        if (!$menuService->userHasMenuAccess($user, $menuItem)) {
            // Log the access attempt for security monitoring
            \Log::warning('Unauthorized menu access attempt', [
                'user_id' => $user->id,
                'user_email' => $user->email,
                'role' => $user->roleType ? $user->roleType->name : 'No Role',
                'menu_item' => $menuItem,
                'route' => $request->route()->getName(),
                'ip_address' => $request->ip(),
                'user_agent' => $request->userAgent()
            ]);

            // Return 403 Forbidden or redirect based on request type
            if ($request->expectsJson()) {
                return response()->json([
                    'error' => 'Access denied',
                    'message' => 'You do not have permission to access this resource.'
                ], 403);
            }

            // For web requests, redirect with error message
            return redirect()->route('index')->with('error', 'Access denied. You do not have permission to access that page.');
        }

        return $next($request);
    }

    /* =====================================================
     * Section: Route Analysis Methods
     * Description: Methods for determining menu access requirements
     * ===================================================== */

    /* =====================================================
     * Section: Route to Menu Mapping
     * Description: Maps Laravel routes to menu items for access control
     * ===================================================== */

    /**
     * Determine the required menu item based on the current route
     * 
     * @param Request $request
     * @return string|null Menu item key or null if not applicable
     */
    private function determineMenuItemFromRoute(Request $request): ?string
    {
        $routeName = $request->route()->getName();
        
        // Map common route patterns to menu items
        $routeToMenuMap = [
            // Dashboard routes
            'index' => 'dashboard',
            'project_dashboard' => 'dashboard',
            
            // Apps routes
            'calendar' => 'apps',
            'profile' => 'apps',
            'setting' => 'apps',
            'project_app' => 'apps',
            'project_details' => 'apps',
            'to_do' => 'apps',
            'team' => 'apps',
            'api' => 'apps',
            'ticket' => 'apps',
            'ticket_details' => 'apps',
            'email' => 'apps',
            'read_email' => 'apps',
            'cart' => 'apps',
            'product' => 'apps',
            'add_product' => 'apps',
            'product_details' => 'apps',
            'product_list' => 'apps',
            'orders' => 'apps',
            'order_details' => 'apps',
            'order_list' => 'apps',
            'checkout' => 'apps',
            'wishlist' => 'apps',
            'invoice' => 'apps',
            'chat' => 'apps',
            'file_manager' => 'apps',
            'bookmark' => 'apps',
            'kanban_board' => 'apps',
            'timeline' => 'apps',
            'faq' => 'apps',
            'pricing' => 'apps',
            'gallery' => 'apps',
            'blog' => 'apps',
            'blog_details' => 'apps',
            'add_blog' => 'apps',
            
            // Widgets
            'widget' => 'widgets',
            
            // UI Kits
            'cheatsheet' => 'ui_kits',
            'alert' => 'ui_kits',
            'badges' => 'ui_kits',
            'buttons' => 'ui_kits',
            'cards' => 'ui_kits',
            'dropdown' => 'ui_kits',
            'grid' => 'ui_kits',
            'avatar' => 'ui_kits',
            'tabs' => 'ui_kits',
            'accordions' => 'ui_kits',
            'progress' => 'ui_kits',
            'notifications' => 'ui_kits',
            'list' => 'ui_kits',
            'helper_classes' => 'ui_kits',
            'background' => 'ui_kits',
            'divider' => 'ui_kits',
            'ribbons' => 'ui_kits',
            'editor' => 'ui_kits',
            'collapse' => 'ui_kits',
            'footer_page' => 'ui_kits',
            'shadow' => 'ui_kits',
            'wrapper' => 'ui_kits',
            'bullet' => 'ui_kits',
            'placeholder' => 'ui_kits',
            'alignment' => 'ui_kits',
            
            // Advanced UI
            'modals' => 'advance_ui',
            'offcanvas' => 'advance_ui',
            'sweetalert' => 'advance_ui',
            'scrollbar' => 'advance_ui',
            'spinners' => 'advance_ui',
            'animation' => 'advance_ui',
            'video_embed' => 'advance_ui',
            'tour' => 'advance_ui',
            'slick_slider' => 'advance_ui',
            'bootstrap_slider' => 'advance_ui',
            'scrollpy' => 'advance_ui',
            'tooltips_popovers' => 'advance_ui',
            'ratings' => 'advance_ui',
            'prismjs' => 'advance_ui',
            
            // Tables
            'bootstrap_tables' => 'tables',
            'data_tables' => 'tables',
            
            // Charts
            'apex' => 'charts',
            'chartjs' => 'charts',
            'google_chart' => 'charts',
            
            // Icons
            'feather_icons' => 'icons',
            'font_awesome' => 'icons',
            'iconly_icons' => 'icons',
            'phosphor_icons' => 'icons',
            'tabler_icons' => 'icons',
            'flag_icons' => 'icons',
            'weather_icons' => 'icons',
            'crypto_icons' => 'icons',
            
            // Forms
            'form_validation' => 'forms',
            'base_inputs' => 'forms',
            'checkbox_radio' => 'forms',
            'input_groups' => 'forms',
            'input_masks' => 'forms',
            'floating_labels' => 'forms',
            'date_picker' => 'forms',
            'touch_spin' => 'forms',
            'select' => 'forms',
            'switch' => 'forms',
            'range_slider' => 'forms',
            'typeahead' => 'forms',
            'textarea' => 'forms',
            'clipboard' => 'forms',
            'file_upload' => 'forms',
            'dual_list_boxes' => 'forms',
            'default_forms' => 'forms',
            'form_wizards' => 'forms',
            'form_wizard_1' => 'forms',
            'form_wizard_2' => 'forms',
            'ready_to_use_form' => 'forms',
            'ready_to_use_table' => 'forms',
            
            // Admin Management (for SuperAdmin/Admin roles)
            'user_management' => 'admin_management',
            'role_management' => 'admin_management',
            'system_settings' => 'admin_management',
            'reports' => 'admin_management',
            
            // Teacher specific routes
            'student_grades' => 'teacher_tools',
            'class_management' => 'teacher_tools',
            'assignments' => 'teacher_tools',
            
            // Support specific routes
            'support_tickets' => 'support_tools',
            'knowledge_base' => 'support_tools'
        ];

        return $routeToMenuMap[$routeName] ?? null;
    }

    /* =====================================================
     * Section: Route Registration Helpers
     * Description: Helper methods for registering protected routes
     * ===================================================== */

    /**
     * Get middleware string for protecting a specific menu item
     * 
     * @param string $menuItem Menu item key
     * @return string Middleware string for route registration
     */
    public static function protectMenuItem(string $menuItem): string
    {
        return 'menu.access:' . $menuItem;
    }

    /**
     * Get commonly used middleware combinations
     * 
     * @return array Array of middleware combinations
     */
    public static function getCommonMiddleware(): array
    {
        return [
            'admin_only' => ['auth', 'menu.access:admin_management'],
            'teacher_only' => ['auth', 'menu.access:teacher_tools'],
            'support_only' => ['auth', 'menu.access:support_tools'],
            'basic_user' => ['auth', 'menu.access:dashboard'],
            'apps_access' => ['auth', 'menu.access:apps'],
            'reports_access' => ['auth', 'menu.access:reports']
        ];
    }
}
