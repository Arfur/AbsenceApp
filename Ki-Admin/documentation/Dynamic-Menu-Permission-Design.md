<?php
/**
 * =========================================================
 * Dynamic Menu Permission System Design
 * =========================================================
 * 
 * This system allows SuperAdmins/Admins to grant additional
 * menu access to users beyond their default role permissions.
 */

/**
 * ADDITIONAL TABLES NEEDED:
 * 
 * 1. user_menu_permissions - Individual user menu overrides
 * 2. role_menu_overrides - Role-level menu additions/restrictions
 * 3. menu_permission_logs - Audit trail of permission changes
 */

return [
    'tables' => [
        'user_menu_permissions' => [
            'id' => 'Primary key',
            'user_id' => 'Foreign key to users table',
            'menu_item' => 'Menu item key from config',
            'permission_type' => 'grant|revoke',
            'granted_by' => 'User ID who granted permission',
            'granted_at' => 'Timestamp',
            'expires_at' => 'Optional expiration date',
            'reason' => 'Why permission was granted/revoked',
        ],
        
        'role_menu_overrides' => [
            'id' => 'Primary key',
            'role_type_id' => 'Foreign key to role_types table',
            'menu_item' => 'Menu item key from config',
            'permission_type' => 'grant|revoke',
            'modified_by' => 'User ID who made change',
            'modified_at' => 'Timestamp',
        ],
        
        'menu_permission_logs' => [
            'id' => 'Primary key',
            'target_user_id' => 'User affected',
            'target_role_id' => 'Role affected (if role-level change)',
            'menu_item' => 'Menu item changed',
            'action' => 'granted|revoked|modified',
            'performed_by' => 'Admin who made change',
            'old_permission' => 'Previous state',
            'new_permission' => 'New state',
            'reason' => 'Admin reason for change',
            'created_at' => 'Timestamp',
        ],
    ],
];
?>
