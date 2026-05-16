<?php
/**
 * =========================================================
 * Project   : ki-admin - v1.0.0
 * File Name : MenuTestController.php
 * 
 * Author    : Michael Battle
 * Created On: 05/Aug/2025
 * Updated On: 05/Aug/2025
 * 
 * Description:
 * Test controller for validating role-based menu system functionality.
 * Provides debugging endpoints to verify menu generation and access control.
 * 
 * Origin:
 * Role-based navigation system testing for ki-admin Support Hub
 * 
 * Changes:
 * - Created MenuTestController for system testing
 * - Added menu debugging endpoints
 * - Implemented role testing functionality
 * - Added access verification methods
 * =========================================================
 */

namespace App\Http\Controllers;

use Illuminate\Http\Request;
use Illuminate\Support\Facades\Auth;
use App\Services\MenuService;
use App\Models\User;

class MenuTestController extends Controller
{
    /* =====================================================
     * Section: Menu Testing Methods
     * Description: Methods for testing and debugging menu system
     * ===================================================== */

    /* =====================================================
     * Section: User Menu Display
     * Description: Shows current user's accessible menu items
     * ===================================================== */

    /**
     * Display current user's menu for debugging
     * 
     * @return \Illuminate\View\View
     */
    public function showUserMenu()
    {
        $user = Auth::user();
        $menuService = new MenuService();
        
        $userMenu = $menuService->getMenuForUser($user);
        $userRole = $user && $user->roleType ? $user->roleType->name : 'No Role';
        
        return view('test.user-menu', [
            'user' => $user,
            'userRole' => $userRole,
            'userMenu' => $userMenu,
            'allMenuItems' => $menuService->getAllMenuItems(),
            'roleHierarchy' => $menuService->getRoleHierarchy()
        ]);
    }

    /* =====================================================
     * Section: Cross-Role Testing
     * Description: Compare menu access across different user roles
     * ===================================================== */

    /**
     * Test menu access for different roles
     * 
     * @return \Illuminate\View\View
     */
    public function testRoleAccess()
    {
        $menuService = new MenuService();
        $roles = ['superadmin', 'admin', 'teacher', 'support_agent'];
        
        $roleMenus = [];
        foreach ($roles as $role) {
            $roleMenus[$role] = $menuService->getMenuItemsForRole($role);
        }
        
        return view('test.role-access', [
            'roleMenus' => $roleMenus,
            'allMenuItems' => $menuService->getAllMenuItems()
        ]);
    }

    /* =====================================================
     * Section: API Testing Endpoints
     * Description: JSON endpoints for AJAX menu testing
     * ===================================================== */

    /**
     * API endpoint to get user menu (for AJAX testing)
     * 
     * @return \Illuminate\Http\JsonResponse
     */
    public function getUserMenuApi()
    {
        $user = Auth::user();
        if (!$user) {
            return response()->json(['error' => 'User not authenticated'], 401);
        }

        $menuService = new MenuService();
        $userMenu = $menuService->getMenuForUser($user);
        
        return response()->json([
            'user_id' => $user->id,
            'user_email' => $user->email,
            'user_role' => $user->roleType ? $user->roleType->name : 'No Role',
            'menu_items' => $userMenu,
            'menu_count' => count($userMenu)
        ]);
    }

    /**
     * Test menu access for a specific menu item
     * 
     * @param Request $request
     * @return \Illuminate\Http\JsonResponse
     */
    public function testMenuAccess(Request $request)
    {
        $request->validate([
            'menu_item' => 'required|string'
        ]);

        $user = Auth::user();
        if (!$user) {
            return response()->json(['error' => 'User not authenticated'], 401);
        }

        $menuService = new MenuService();
        $menuItem = $request->input('menu_item');
        $hasAccess = $menuService->userHasMenuAccess($user, $menuItem);
        
        return response()->json([
            'user_id' => $user->id,
            'user_role' => $user->roleType ? $user->roleType->name : 'No Role',
            'menu_item' => $menuItem,
            'has_access' => $hasAccess,
            'message' => $hasAccess ? 'Access granted' : 'Access denied'
        ]);
    }

    /* =====================================================
     * Section: Role Switching (Development Only)
     * Description: Methods for testing different roles during development
     * ===================================================== */

    /**
     * Switch user role for testing (DEVELOPMENT ONLY)
     * 
     * @param Request $request
     * @return \Illuminate\Http\RedirectResponse
     */
    public function switchRole(Request $request)
    {
        // Only allow in development environment
        if (!app()->environment('local', 'development')) {
            abort(403, 'Role switching is only available in development mode');
        }

        $request->validate([
            'role_type_id' => 'required|integer|between:1,4'
        ]);

        $user = Auth::user();
        if (!$user) {
            return redirect()->route('sign_in')->with('error', 'Please log in first');
        }

        $roleNames = [
            1 => 'SuperAdmin',
            2 => 'Admin', 
            3 => 'Teacher',
            4 => 'Support Agent'
        ];

        $newRoleId = $request->input('role_type_id');
        $user->role_type_id = $newRoleId;
        $user->save();

        return redirect()->back()->with('success', 'Role switched to: ' . $roleNames[$newRoleId]);
    }

    /**
     * Show role switching interface (DEVELOPMENT ONLY)
     * 
     * @return \Illuminate\View\View
     */
    public function showRoleSwitcher()
    {
        // Only allow in development environment
        if (!app()->environment('local', 'development')) {
            abort(403, 'Role switching is only available in development mode');
        }

        $user = Auth::user();
        $roles = [
            1 => 'SuperAdmin',
            2 => 'Admin', 
            3 => 'Teacher',
            4 => 'Support Agent'
        ];

        return view('test.role-switcher', [
            'user' => $user,
            'roles' => $roles,
            'currentRole' => $user && $user->roleType ? $user->roleType->name : 'No Role'
        ]);
    }

    /* =====================================================
     * Section: Configuration Testing
     * Description: Methods for testing configuration and setup
     * ===================================================== */

    /**
     * Validate menu configuration
     * 
     * @return \Illuminate\Http\JsonResponse
     */
    public function validateConfig()
    {
        $menuService = new MenuService();
        $allMenuItems = $menuService->getAllMenuItems();
        $roleMenus = config('role_menu.role_menus', []);
        $roleHierarchy = $menuService->getRoleHierarchy();

        $validation = [
            'config_loaded' => !empty($allMenuItems),
            'roles_configured' => count($roleMenus),
            'menu_items_count' => count($allMenuItems),
            'hierarchy_defined' => !empty($roleHierarchy),
            'issues' => []
        ];

        // Check for missing menu items referenced in roles
        foreach ($roleMenus as $role => $menuItems) {
            foreach ($menuItems as $menuItem) {
                if (!isset($allMenuItems[$menuItem])) {
                    $validation['issues'][] = "Role '$role' references undefined menu item: '$menuItem'";
                }
            }
        }

        return response()->json($validation);
    }

    /**
     * Assign default menu items to current user based on their role
     * 
     * @param Request $request
     * @return \Illuminate\Http\JsonResponse
     */
    public function assignDefaultMenus(Request $request)
    {
        try {
            $user = Auth::user();
            
            if (!$user) {
                return response()->json([
                    'success' => false,
                    'message' => 'User not authenticated'
                ], 401);
            }

            // Get count before assignment
            $beforeCount = \App\Models\UserMenuItem::where('user_id', $user->user_id)->count();
            
            // Assign default menus
            $user->assignDefaultMenuItems();
            
            // Get count after assignment
            $afterCount = \App\Models\UserMenuItem::where('user_id', $user->user_id)->count();
            $newItemsCount = $afterCount - $beforeCount;

            return response()->json([
                'success' => true,
                'message' => "Successfully assigned {$newItemsCount} new menu items. Total: {$afterCount}",
                'before_count' => $beforeCount,
                'after_count' => $afterCount,
                'new_items' => $newItemsCount
            ]);

        } catch (\Exception $e) {
            \Log::error('Error assigning default menus: ' . $e->getMessage());
            
            return response()->json([
                'success' => false,
                'message' => 'Error assigning default menus: ' . $e->getMessage()
            ], 500);
        }
    }

    /**
     * Test View Composer functionality
     * 
     * @return \Illuminate\View\View
     */
    public function testViewComposer()
    {
        // This view will automatically receive sidebar menu data from SidebarMenuComposer
        return view('test.view-composer-test', [
            'test_message' => 'View Composer Test - Menu data should be automatically injected'
        ]);
    }

    /**
     * Debug menu data structure to identify field issues
     * 
     * @return \Illuminate\Http\JsonResponse
     */
    public function debugMenuStructure()
    {
        try {
            $user = Auth::user();
            
            if (!$user) {
                return response()->json(['error' => 'User not authenticated'], 401);
            }

            // Get menu data directly from MenuBuilder
            $menuBuilder = new \App\Helpers\MenuBuilder();
            $menuData = $menuBuilder->getSidebarMenu();
            
            // Also get first menu item structure for detailed inspection
            $firstItem = !empty($menuData) ? $menuData[0] : null;
            
            return response()->json([
                'user_id' => $user->user_id ?? $user->id,
                'user_email' => $user->email,
                'menu_count' => count($menuData),
                'first_menu_item_structure' => $firstItem,
                'all_menu_data' => $menuData,
                'available_fields' => $firstItem ? array_keys($firstItem) : []
            ], 200, [], JSON_PRETTY_PRINT);

        } catch (\Exception $e) {
            return response()->json([
                'error' => 'Debug failed: ' . $e->getMessage(),
                'trace' => $e->getTraceAsString()
            ], 500);
        }
    }
}
