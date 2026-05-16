<?php
/**
 * =========================================================
 * Project   : ki-admin - v1.0.0
 * File Name : MenuService.php
 * 
 * Author    : Michael Battle
 * Created On: 05/Aug/2025
 * Updated On: 05/Aug/2025
 * 
 * Description:
 * Service class for generating role-based menu items for the Support Hub.
 * Handles menu generation based on user roles with foundation for dynamic permissions.
 * 
 * Origin:
 * Role-based navigation system for ki-admin Support Hub
 * 
 * Changes:
 * - Created MenuService class for role-based menu generation
 * - Implemented getMenuForUser method with role checking
 * - Added menu structure building functionality
 * - Foundation prepared for future dynamic permission system
 * =========================================================
 */

namespace App\Services;

use App\Models\User;
use Illuminate\Support\Facades\Auth;

class MenuService
{
    /* =====================================================
     * Section: Core Menu Generation
     * Description: Main methods for generating user-specific menus
     * ===================================================== */

    /**
     * Get menu items for a specific user based on their role
     * 
     * @param User|null $user The user to generate menu for
     * @return array Structured menu array
     */
    public function getMenuForUser(?User $user = null): array
    {
        // Use authenticated user if none provided
        if (!$user) {
            $user = Auth::user();
        }

        // Return empty menu if no user
        if (!$user) {
            return [];
        }

        // Get user's role-based menu items
        $allowedMenuItems = $this->getUserRoleMenuItems($user);

        // Build the complete menu structure
        return $this->buildMenuStructure($allowedMenuItems);
    }

    /* =====================================================
     * Section: Role-Based Menu Access
     * Description: Methods for determining user role menu permissions
     * ===================================================== */

    /**
     * Get menu items allowed for user's role type
     * 
     * @param User $user
     * @return array Array of menu item keys
     */
    private function getUserRoleMenuItems(User $user): array
    {
        $roleMenuConfig = config('role_menu.role_menus', []);
        
        // Get user's role name (with fallback)
        $userRoleName = $this->getUserRoleName($user);
        
        // Return menu items for this role, or empty array if role not found
        return $roleMenuConfig[$userRoleName] ?? [];
    }

    /**
     * Get user's role name with proper fallback
     * 
     * @param User $user
     * @return string Role name
     */
    private function getUserRoleName(User $user): string
    {
        // Try to get role from relationship
        if ($user->roleType && $user->roleType->name) {
            $roleName = strtolower($user->roleType->name);
            
            // Map standard role names to our menu config keys
            $roleMapping = [
                'user' => 'support_agent',  // Map basic users to support agent role
                'super_admin' => 'superadmin',
                'superadmin' => 'superadmin',
                'admin' => 'admin',
                'teacher' => 'teacher',
                'support_agent' => 'support_agent'
            ];
            
            return $roleMapping[$roleName] ?? 'support_agent'; // Default to support_agent
        }

        // Fallback: try direct role_type_id mapping
        $roleMap = [
            1 => 'support_agent',  // Map role_type_id 1 to support_agent instead of superadmin
            2 => 'admin', 
            3 => 'teacher',
            4 => 'superadmin'
        ];

        return $roleMap[$user->role_type_id] ?? 'support_agent'; // Default to support_agent
    }

    /* =====================================================
     * Section: Menu Structure Building
     * Description: Methods for building the final menu HTML structure
     * ===================================================== */

    /**
     * Build the complete menu structure from allowed menu items
     * 
     * @param array $allowedMenuItems Array of menu item keys
     * @return array Structured menu for rendering
     */
    private function buildMenuStructure(array $allowedMenuItems): array
    {
        $menuDefinitions = config('role_menu.menu_items', []);
        $structuredMenu = [];

        foreach ($allowedMenuItems as $menuKey) {
            if (isset($menuDefinitions[$menuKey])) {
                $menuItem = $menuDefinitions[$menuKey];
                
                // Build main menu item
                $structuredItem = [
                    'key' => $menuKey,
                    'label' => $menuItem['label'],
                    'icon' => $menuItem['icon'] ?? 'menu',
                    'route' => $menuItem['route'] ?? '#',
                    'submenu' => []
                ];

                // Add submenu items if they exist
                if (isset($menuItem['submenu']) && is_array($menuItem['submenu'])) {
                    $structuredItem['submenu'] = $this->buildSubmenuItems($menuItem['submenu']);
                }

                $structuredMenu[] = $structuredItem;
            }
        }

        return $structuredMenu;
    }

    /* =====================================================
     * Section: Submenu Processing
     * Description: Methods for handling submenu items and access control
     * ===================================================== */

    /**
     * Build submenu items structure
     * 
     * @param array $submenuItems Raw submenu configuration
     * @return array Structured submenu items
     */
    private function buildSubmenuItems(array $submenuItems): array
    {
        $structuredSubmenu = [];

        foreach ($submenuItems as $subKey => $subItem) {
            // Check if this submenu item has role restrictions
            if (isset($subItem['roles']) && !$this->userHasSubmenuAccess($subItem['roles'])) {
                continue; // Skip this submenu item
            }

            $structuredSubmenu[] = [
                'key' => $subKey,
                'label' => $subItem['label'],
                'route' => $subItem['route'] ?? '#',
                'roles' => $subItem['roles'] ?? []
            ];
        }

        return $structuredSubmenu;
    }

    /**
     * Check if current user has access to a submenu item
     * 
     * @param array $allowedRoles Array of roles allowed for this submenu
     * @return bool True if user has access
     */
    private function userHasSubmenuAccess(array $allowedRoles): bool
    {
        $user = Auth::user();
        if (!$user) {
            return false;
        }

        $userRole = $this->getUserRoleName($user);
        return in_array($userRole, $allowedRoles);
    }

    /* =====================================================
     * Section: Menu Utility Methods
     * Description: Helper methods for menu operations
     * ===================================================== */

    /**
     * Get all available menu items (for admin interfaces)
     * 
     * @return array All menu items from configuration
     */
    public function getAllMenuItems(): array
    {
        return config('role_menu.menu_items', []);
    }

    /**
     * Get menu items for a specific role
     * 
     * @param string $roleName Role name (superadmin, admin, teacher, support_agent)
     * @return array Menu items for the role
     */
    public function getMenuItemsForRole(string $roleName): array
    {
        $roleMenus = config('role_menu.role_menus', []);
        return $roleMenus[strtolower($roleName)] ?? [];
    }

    /**
     * Check if a user has access to a specific menu item
     * 
     * @param User $user User to check
     * @param string $menuItem Menu item key
     * @return bool True if user has access
     */
    public function userHasMenuAccess(User $user, string $menuItem): bool
    {
        $userMenuItems = $this->getUserRoleMenuItems($user);
        return in_array($menuItem, $userMenuItems);
    }

    /**
     * Get role hierarchy for permission checking
     * 
     * @return array Role hierarchy from configuration
     */
    public function getRoleHierarchy(): array
    {
        return config('role_menu.role_hierarchy', []);
    }

    /* =====================================================
     * Section: Future Dynamic Permissions Placeholder
     * Description: Methods prepared for future dynamic permission system
     * ===================================================== */

    /**
     * Placeholder for future dynamic permission application
     * This will be enhanced when implementing dynamic permissions
     * 
     * @param User $user
     * @param array $baseMenuItems
     * @return array Modified menu items with dynamic permissions
     */
    private function applyDynamicPermissions(User $user, array $baseMenuItems): array
    {
        // TODO: Implement dynamic permission system
        // This will check user_menu_permissions and role_menu_overrides tables
        // For now, just return the base menu items
        return $baseMenuItems;
    }
}
