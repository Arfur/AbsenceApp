<?php
/**
 * =========================================================
 * Project   : ki-admin - v1.0.0
 * File Name : Enhanced-MenuService-Example.php
 * 
 * Author    : Michael Battle
 * Created On: 05/Aug/2025
 * Updated On: 05/Aug/2025
 * 
 * Description:
 * EXAMPLE FILE: Future MenuService with dynamic permission system
 * allowing admins to grant/revoke menu access beyond default role permissions.
 * This is a design document showing how the enhanced system will work.
 * 
 * Origin:
 * Future implementation plan for dynamic menu permissions
 * 
 * Changes:
 * - Created example enhanced MenuService design
 * - Shows dynamic permission system architecture
 * - Demonstrates user and role override functionality
 * =========================================================
 */

// NOTE: This is an EXAMPLE file showing future implementation
// The classes referenced here do not exist yet and will be created
// when implementing the dynamic permission system

namespace App\Services;

use App\Models\User;
// Future models that will be created:
// use App\Models\UserMenuPermission;
// use App\Models\RoleMenuOverride;
// use App\Models\MenuPermissionLog;

/**
 * EXAMPLE: Enhanced MenuService with Dynamic Permissions
 * This shows how the MenuService will be extended to support
 * dynamic permission management by administrators
 */
class EnhancedMenuServiceExample
{
    /**
     * Get menu items for a specific user considering:
     * 1. Default role permissions
     * 2. Individual user permission overrides
     * 3. Role-level permission overrides
     */
    public function getMenuForUser(User $user): array
    {
        // 1. Start with default role permissions
        $roleMenus = $this->getDefaultRoleMenus($user);
        
        // 2. Apply role-level overrides (affects all users of this role)
        $roleMenus = $this->applyRoleOverrides($user, $roleMenus);
        
        // 3. Apply individual user overrides (specific to this user)
        $finalMenus = $this->applyUserOverrides($user, $roleMenus);
        
        // 4. Build the actual menu structure
        return $this->buildMenuStructure($finalMenus);
    }

    /**
     * Get default menu items for user's role
     */
    private function getDefaultRoleMenus(User $user): array
    {
        $roleConfig = config('role_menu.role_menus');
        $userRole = $user->roleType->name ?? 'teacher'; // Default fallback
        
        return $roleConfig[$userRole] ?? [];
    }

    /**
     * Apply role-level overrides (SuperAdmin/Admin can modify entire role permissions)
     */
    private function applyRoleOverrides(User $user, array $defaultMenus): array
    {
        $overrides = RoleMenuOverride::where('role_type_id', $user->role_type_id)->get();
        
        foreach ($overrides as $override) {
            if ($override->permission_type === 'grant') {
                // Add menu item if not already present
                if (!in_array($override->menu_item, $defaultMenus)) {
                    $defaultMenus[] = $override->menu_item;
                }
            } elseif ($override->permission_type === 'revoke') {
                // Remove menu item
                $defaultMenus = array_filter($defaultMenus, function($item) use ($override) {
                    return $item !== $override->menu_item;
                });
            }
        }
        
        return array_values($defaultMenus);
    }

    /**
     * Apply individual user overrides (specific grants/revokes for this user)
     */
    private function applyUserOverrides(User $user, array $roleMenus): array
    {
        $userPermissions = UserMenuPermission::where('user_id', $user->id)
            ->where(function($query) {
                $query->whereNull('expires_at')
                      ->orWhere('expires_at', '>', now());
            })
            ->get();
        
        foreach ($userPermissions as $permission) {
            if ($permission->permission_type === 'grant') {
                // Grant additional access
                if (!in_array($permission->menu_item, $roleMenus)) {
                    $roleMenus[] = $permission->menu_item;
                }
            } elseif ($permission->permission_type === 'revoke') {
                // Revoke access (override role permission)
                $roleMenus = array_filter($roleMenus, function($item) use ($permission) {
                    return $item !== $permission->menu_item;
                });
            }
        }
        
        return array_values($roleMenus);
    }

    /**
     * Admin method: Grant menu access to a specific user
     */
    public function grantMenuAccess(User $targetUser, string $menuItem, User $grantedBy, ?string $reason = null, ?\DateTime $expiresAt = null): bool
    {
        // Check if granter has permission to grant this access
        if (!$this->canManagePermissions($grantedBy, $targetUser)) {
            return false;
        }

        // Remove any existing revoke for this menu item
        UserMenuPermission::where('user_id', $targetUser->id)
            ->where('menu_item', $menuItem)
            ->where('permission_type', 'revoke')
            ->delete();

        // Create or update grant permission
        UserMenuPermission::updateOrCreate(
            [
                'user_id' => $targetUser->id,
                'menu_item' => $menuItem,
                'permission_type' => 'grant'
            ],
            [
                'granted_by' => $grantedBy->id,
                'granted_at' => now(),
                'expires_at' => $expiresAt,
                'reason' => $reason,
            ]
        );

        // Log the action
        $this->logPermissionChange($targetUser, null, $menuItem, 'granted', $grantedBy, null, 'grant', $reason);

        return true;
    }

    /**
     * Admin method: Revoke menu access from a specific user
     */
    public function revokeMenuAccess(User $targetUser, string $menuItem, User $revokedBy, ?string $reason = null): bool
    {
        if (!$this->canManagePermissions($revokedBy, $targetUser)) {
            return false;
        }

        // Remove any existing grant for this menu item
        UserMenuPermission::where('user_id', $targetUser->id)
            ->where('menu_item', $menuItem)
            ->where('permission_type', 'grant')
            ->delete();

        // Create revoke permission (removes access even if role normally has it)
        UserMenuPermission::create([
            'user_id' => $targetUser->id,
            'menu_item' => $menuItem,
            'permission_type' => 'revoke',
            'granted_by' => $revokedBy->id,
            'granted_at' => now(),
            'reason' => $reason,
        ]);

        $this->logPermissionChange($targetUser, null, $menuItem, 'revoked', $revokedBy, 'grant', 'revoke', $reason);

        return true;
    }

    /**
     * Admin method: Modify entire role permissions
     */
    public function modifyRolePermissions(int $roleTypeId, string $menuItem, string $action, User $modifiedBy): bool
    {
        if (!$this->canManageRolePermissions($modifiedBy)) {
            return false;
        }

        RoleMenuOverride::updateOrCreate(
            [
                'role_type_id' => $roleTypeId,
                'menu_item' => $menuItem,
            ],
            [
                'permission_type' => $action, // 'grant' or 'revoke'
                'modified_by' => $modifiedBy->id,
                'modified_at' => now(),
            ]
        );

        return true;
    }

    /**
     * Check if user can manage permissions for target user
     */
    private function canManagePermissions(User $manager, User $targetUser): bool
    {
        $managerRole = $manager->roleType->name ?? '';
        $targetRole = $targetUser->roleType->name ?? '';
        
        // SuperAdmin can manage anyone
        if ($managerRole === 'superadmin') {
            return true;
        }
        
        // Admin can manage Teachers and Support Agents, but not other Admins or SuperAdmins
        if ($managerRole === 'admin') {
            return in_array($targetRole, ['teacher', 'support_agent']);
        }
        
        return false;
    }

    /**
     * Check if user can manage role-level permissions
     */
    private function canManageRolePermissions(User $user): bool
    {
        return $user->roleType->name === 'superadmin';
    }

    /**
     * Get all users with custom permissions (for admin interface)
     */
    public function getUsersWithCustomPermissions(): array
    {
        return UserMenuPermission::with(['user', 'grantedByUser'])
            ->get()
            ->groupBy('user_id')
            ->map(function($permissions) {
                return [
                    'user' => $permissions->first()->user,
                    'permissions' => $permissions->groupBy('permission_type')
                ];
            })
            ->values()
            ->toArray();
    }

    // ... other helper methods
}

/**
 * EXAMPLE USAGE IN ADMIN INTERFACE:
 * 
 * // Grant a Teacher access to User Management
 * $menuService = new MenuService();
 * $admin = User::find(1); // Admin user
 * $teacher = User::find(5); // Teacher user
 * 
 * $menuService->grantMenuAccess(
 *     $teacher, 
 *     'user_management', 
 *     $admin, 
 *     'Temporary access for training purposes',
 *     now()->addDays(30) // Expires in 30 days
 * );
 * 
 * // Revoke access
 * $menuService->revokeMenuAccess($teacher, 'reports', $admin, 'No longer needed');
 * 
 * // Grant entire role additional access
 * $menuService->modifyRolePermissions(3, 'reports', 'grant', $superAdmin); // All Teachers get Reports
 */
