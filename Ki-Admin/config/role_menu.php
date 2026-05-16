<?php
/**
 * =========================================================
 * Project   : ki-admin - v1.0.0
 * File Name : RoleMenuConfig.php
 * 
 * Author    : Michael Battle
 * Created On: 05/Aug/2025
 * Updated On: 05/Aug/2025
 * 
 * Description:
 * Configuration mapping menu items to user roles for the Support Hub.
 * Defines which menu items are visible to each role type.
 * 
 * Origin:
 * Role-based menu system implementation
 * 
 * Changes:
 * - Created initial role-to-menu mapping
 * - Defined menu structure based on PRD requirements
 * =========================================================
 */

return [
    /**
     * Menu items mapped to roles
     * Each role sees only the menu items listed in their array
     */
    'role_menus' => [
        'superadmin' => [
            'dashboard',
            'section_management', 
            'user_management',
            'tickets',
            'knowledge_base',
            'reports',
            'system_settings',
            'email_templates',
            'database_backup',
            'activity_logs',
        ],
        
        'admin' => [
            'dashboard',
            'section_management',
            'user_management', // Can manage users but not delete
            'tickets',
            'knowledge_base',
            'reports',
            'system_settings', // Limited access
        ],
        
        'teacher' => [
            'dashboard',
            'section_support',
            'my_tickets',
            'create_ticket',
            'knowledge_base', // Read-only
            'section_personal',
            'profile',
        ],
        
        'support_agent' => [
            'dashboard',
            'section_support',
            'assigned_tickets',
            'knowledge_base', // Read-only
            'section_personal',
            'profile',
        ],
        
        // Basic user role (maps to support_agent permissions)
        'user' => [
            'dashboard',
            'section_support',
            'assigned_tickets',
            'knowledge_base', // Read-only
            'section_personal',
            'profile',
        ],
    ],

    /**
     * Menu item definitions with routes, icons, and labels
     */
    'menu_items' => [
        // Section titles (these create menu section headers)
        'section_dashboard' => [
            'key' => 'section_title',
            'label' => 'Dashboard',
            'icon' => '',
            'route' => '#',
        ],
        
        'section_management' => [
            'key' => 'section_title',
            'label' => 'Management',
            'icon' => '',
            'route' => '#',
        ],
        
        'section_support' => [
            'key' => 'section_title',
            'label' => 'Support',
            'icon' => '',
            'route' => '#',
        ],
        
        'section_teaching' => [
            'key' => 'section_title',
            'label' => 'Teaching Tools',
            'icon' => '',
            'route' => '#',
        ],
        
        'section_personal' => [
            'key' => 'section_title',
            'label' => 'Personal',
            'icon' => '',
            'route' => '#',
        ],
        
        // Actual menu items
        'dashboard' => [
            'label' => 'Dashboard',
            'icon' => 'home',
            'route' => 'dashboard',
            'submenu' => [
                'overview' => [
                    'label' => 'Overview',
                    'route' => 'dashboard',
                ],
                'statistics' => [
                    'label' => 'Statistics',
                    'route' => 'dashboard.statistics',
                ],
            ],
        ],
        
        'user_management' => [
            'label' => 'User Management',
            'icon' => 'users',
            'route' => 'users.index',
            'submenu' => [
                'all_users' => [
                    'label' => 'All Users',
                    'route' => 'users.index',
                ],
                'add_user' => [
                    'label' => 'Add User',
                    'route' => 'users.create',
                ],
                'roles' => [
                    'label' => 'Manage Roles',
                    'route' => 'roles.index',
                    'roles' => ['superadmin'], // Only SuperAdmin
                ],
            ],
        ],
        
        'tickets' => [
            'label' => 'Support Tickets',
            'icon' => 'ticket',
            'route' => 'tickets.index',
            'submenu' => [
                'all_tickets' => [
                    'label' => 'All Tickets',
                    'route' => 'tickets.index',
                ],
                'open_tickets' => [
                    'label' => 'Open Tickets',
                    'route' => 'tickets.open',
                ],
                'closed_tickets' => [
                    'label' => 'Closed Tickets',
                    'route' => 'tickets.closed',
                ],
            ],
        ],
        
        'my_tickets' => [
            'label' => 'My Tickets',
            'icon' => 'ticket',
            'route' => 'tickets.my',
            'submenu' => [
                'my_open' => [
                    'label' => 'Open Tickets',
                    'route' => 'tickets.my.open',
                ],
                'my_closed' => [
                    'label' => 'Closed Tickets',
                    'route' => 'tickets.my.closed',
                ],
            ],
        ],
        
        'create_ticket' => [
            'label' => 'Create Ticket',
            'icon' => 'plus',
            'route' => 'tickets.create',
        ],
        
        'assigned_tickets' => [
            'label' => 'Assigned Tickets',
            'icon' => 'assignment',
            'route' => 'tickets.assigned',
        ],
        
        'knowledge_base' => [
            'label' => 'Knowledge Base',
            'icon' => 'book',
            'route' => 'knowledge.index',
            'submenu' => [
                'browse_articles' => [
                    'label' => 'Browse Articles',
                    'route' => 'knowledge.index',
                ],
                'categories' => [
                    'label' => 'Categories',
                    'route' => 'knowledge.categories',
                ],
                'create_article' => [
                    'label' => 'Create Article',
                    'route' => 'knowledge.create',
                    'roles' => ['superadmin', 'admin'], // Only admins can create
                ],
            ],
        ],
        
        'reports' => [
            'label' => 'Reports',
            'icon' => 'chart',
            'route' => 'reports.index',
            'submenu' => [
                'ticket_reports' => [
                    'label' => 'Ticket Reports',
                    'route' => 'reports.tickets',
                ],
                'user_activity' => [
                    'label' => 'User Activity',
                    'route' => 'reports.activity',
                ],
            ],
        ],
        
        'system_settings' => [
            'label' => 'Settings',
            'icon' => 'settings',
            'route' => 'settings.index',
            'submenu' => [
                'general' => [
                    'label' => 'General Settings',
                    'route' => 'settings.general',
                ],
                'email_settings' => [
                    'label' => 'Email Settings',
                    'route' => 'settings.email',
                    'roles' => ['superadmin'], // Only SuperAdmin
                ],
            ],
        ],
        
        'profile' => [
            'label' => 'Profile',
            'icon' => 'user',
            'route' => 'profile.show',
        ],
        
        'email_templates' => [
            'label' => 'Email Templates',
            'icon' => 'mail',
            'route' => 'email.templates',
        ],
        
        'database_backup' => [
            'label' => 'Database Backup',
            'icon' => 'database',
            'route' => 'backup.index',
        ],
        
        'activity_logs' => [
            'label' => 'Activity Logs',
            'icon' => 'activity',
            'route' => 'logs.index',
        ],
    ],

    /**
     * Role hierarchy for permissions inheritance
     */
    'role_hierarchy' => [
        'superadmin' => 4,
        'admin' => 3,
        'support_agent' => 2,
        'teacher' => 1,
    ],
];
