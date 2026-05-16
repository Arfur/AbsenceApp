{{--
/**
 * =========================================================
 * Project   : ki-admin - v1.0.0
 * File Name : user-menu.blade.php
 * 
 * Author    : Michael Battle
 * Created On: 05/Aug/2025
 * Updated On: 05/Aug/2025
 * 
 * Description:
 * Testing interface for displaying current user's accessible menu items
 * based on their role. Shows menu generation debugging information.
 * 
 * Origin:
 * Role-based menu system testing and validation interface
 * 
 * Changes:
 * - Created user menu debugging interface
 * - Added role information display and menu statistics
 * - Implemented menu access testing buttons
 * - Added AJAX functionality for live testing
 * - Integrated development tool navigation
 * =========================================================
 */
--}}

@extends('layout.master')

@section('title', 'Menu System Test - User Menu')
@section('css')
<style>
    /* Debug overlay for layout troubleshooting */
    .debug-overlay {
        position: fixed;
        top: 10px;
        right: 10px;
        background: rgba(255, 0, 0, 0.8);
        color: white;
        padding: 10px;
        border-radius: 5px;
        z-index: 9999;
        font-size: 12px;
        max-width: 200px;
    }
    
    .debug-card {
        background: #f8f9fa;
        border: 1px solid #dee2e6;
        border-radius: 8px;
        padding: 20px;
        margin-bottom: 20px;
    }
    .menu-item {
        background: white;
        border: 1px solid #e3e6f0;
        border-radius: 5px;
        padding: 10px;
        margin: 5px 0;
    }
    .submenu-item {
        background: #f1f3f4;
        margin-left: 20px;
        padding: 5px 10px;
        border-left: 3px solid #007bff;
    }
    .role-badge {
        font-size: 0.875rem;
        padding: 0.375rem 0.75rem;
        border-radius: 0.375rem;
    }
    .access-test {
        margin-top: 20px;
    }
</style>
@endsection

@section('main-content')
<div class="debug-overlay">
    <div>Viewport: <span id="viewport"></span></div>
    <div>Sidebar width: <span id="sidebar-width"></span></div>
    <div>Content padding: <span id="content-padding"></span></div>
</div>

<div class="container-fluid">
    <!-- Breadcrumb start -->
    <div class="row m-1">
        <div class="col-12 ">
            <h4 class="main-title">Menu System Test - User Menu</h4>
            <ul class="app-line-breadcrumbs mb-3">
                <li class="">
                    <a href="#" class="f-s-14 f-w-500">
                  <span>
                    <i class="ph-duotone ph-gear f-s-16"></i> Testing
                  </span>
                    </a>
                </li>
                <li class="active">
                    <a href="#" class="f-s-14 f-w-500">User Menu</a>
                </li>
            </ul>
        </div>
    </div>
    <!-- Breadcrumb end -->

    <!-- User Information -->
    <div class="row">
        <div class="col-md-6">
            <div class="debug-card">
                <h5 class="mb-3">Current User Information</h5>
                @if($user)
                    <p><strong>User ID:</strong> {{ $user->id }}</p>
                    <p><strong>Email:</strong> {{ $user->email }}</p>
                    <p><strong>Role:</strong> 
                        <span class="badge role-badge 
                            @if($userRole === 'SuperAdmin') bg-danger
                            @elseif($userRole === 'Admin') bg-warning
                            @elseif($userRole === 'Teacher') bg-success
                            @elseif($userRole === 'Support Agent') bg-info
                            @else bg-secondary
                            @endif">
                            {{ $userRole }}
                        </span>
                    </p>
                    <p><strong>Role Type ID:</strong> {{ $user->role_type_id ?? 'Not Set' }}</p>
                @else
                    <p class="text-danger">No user logged in</p>
                @endif
            </div>
        </div>
        
        <div class="col-md-6">
            <div class="debug-card">
                <h5 class="mb-3">Menu Statistics</h5>
                <p><strong>Accessible Menu Items:</strong> {{ count($userMenu) }}</p>
                <p><strong>Total Available Menu Items:</strong> {{ count($allMenuItems) }}</p>
                <p><strong>Role Hierarchy Levels:</strong> {{ count($roleHierarchy) }}</p>
            </div>
        </div>
    </div>

    <!-- User's Menu Items -->
    <div class="row">
        <div class="col-12">
            <div class="debug-card">
                <h5 class="mb-3">User's Accessible Menu Items</h5>
                
                @if(!empty($userMenu))
                    @foreach($userMenu as $menuItem)
                        <div class="menu-item">
                            <div class="d-flex justify-content-between align-items-center">
                                <div>
                                    <strong>{{ $menuItem['label'] }}</strong>
                                    <span class="text-muted">({{ $menuItem['key'] }})</span>
                                </div>
                                <div>
                                    <span class="badge bg-primary">{{ $menuItem['icon'] }}</span>
                                    @if($menuItem['route'] !== '#')
                                        <a href="{{ $menuItem['route'] }}" class="btn btn-sm btn-outline-primary ms-2">Visit</a>
                                    @endif
                                </div>
                            </div>
                            
                            @if(!empty($menuItem['submenu']))
                                <div class="mt-2">
                                    <small class="text-muted">Submenu items:</small>
                                    @foreach($menuItem['submenu'] as $subItem)
                                        <div class="submenu-item">
                                            {{ $subItem['label'] }} 
                                            <span class="text-muted">({{ $subItem['key'] }})</span>
                                            @if($subItem['route'] !== '#')
                                                - <a href="{{ $subItem['route'] }}" class="text-decoration-none">{{ $subItem['route'] }}</a>
                                            @endif
                                        </div>
                                    @endforeach
                                </div>
                            @endif
                        </div>
                    @endforeach
                @else
                    <div class="alert alert-warning">
                        <h6>No Menu Items Found</h6>
                        <p>This user has no accessible menu items. This could indicate:</p>
                        <ul>
                            <li>User has no role assigned</li>
                            <li>Role configuration is missing</li>
                            <li>MenuService configuration error</li>
                        </ul>
                    </div>
                @endif
            </div>
        </div>
    </div>

    <!-- Test Access to Specific Items -->
    <div class="row">
        <div class="col-12">
            <div class="debug-card access-test">
                <h5 class="mb-3">Test Access to Specific Menu Items</h5>
                <div class="row">
                    @foreach(['dashboard', 'apps', 'admin_management', 'teacher_tools', 'support_tools', 'widgets'] as $testItem)
                        <div class="col-md-4 mb-3">
                            <div class="card">
                                <div class="card-body text-center">
                                    <h6>{{ ucfirst(str_replace('_', ' ', $testItem)) }}</h6>
                                    <button class="btn btn-sm test-access-btn
                                        @if(in_array($testItem, array_column($userMenu, 'key'))) btn-success
                                        @else btn-danger
                                        @endif"
                                        data-menu-item="{{ $testItem }}">
                                        @if(in_array($testItem, array_column($userMenu, 'key')))
                                            ✓ Has Access
                                        @else
                                            ✗ No Access
                                        @endif
                                    </button>
                                </div>
                            </div>
                        </div>
                    @endforeach
                </div>
            </div>
        </div>
    </div>

    <!-- Development Tools -->
    @if(app()->environment('local', 'development'))
        <div class="row">
            <div class="col-12">
                <div class="debug-card">
                    <h5 class="mb-3">Development Tools</h5>
                    <div class="btn-group" role="group">
                        <a href="{{ route('test.role-access') }}" class="btn btn-outline-primary">Test All Roles</a>
                        <a href="{{ route('test.role-switcher') }}" class="btn btn-outline-warning">Switch Role</a>
                        <button id="refreshMenu" class="btn btn-outline-info">Refresh Menu</button>
                        <button id="validateConfig" class="btn btn-outline-secondary">Validate Config</button>
                    </div>
                </div>
            </div>
        </div>
    @endif

</div>
@endsection

@section('script')
<script>
$(document).ready(function() {
    
    // Test access to specific menu items
    $('.test-access-btn').click(function() {
        const menuItem = $(this).data('menu-item');
        const button = $(this);
        
        button.prop('disabled', true).text('Testing...');
        
        $.post('{{ route("test.menu-access") }}', {
            menu_item: menuItem,
            _token: '{{ csrf_token() }}'
        })
        .done(function(response) {
            const hasAccess = response.has_access;
            button.removeClass('btn-success btn-danger')
                  .addClass(hasAccess ? 'btn-success' : 'btn-danger')
                  .text(hasAccess ? '✓ Has Access' : '✗ No Access');
        })
        .fail(function() {
            button.removeClass('btn-success btn-danger')
                  .addClass('btn-warning')
                  .text('Error Testing');
        })
        .always(function() {
            button.prop('disabled', false);
        });
    });
    
    // Refresh menu data
    $('#refreshMenu').click(function() {
        location.reload();
    });
    
    // Validate configuration
    $('#validateConfig').click(function() {
        const button = $(this);
        button.prop('disabled', true).text('Validating...');
        
        $.get('{{ route("test.validate-config") }}')
        .done(function(response) {
            let message = `Configuration Status:\n`;
            message += `- Config Loaded: ${response.config_loaded ? 'Yes' : 'No'}\n`;
            message += `- Roles Configured: ${response.roles_configured}\n`;
            message += `- Menu Items: ${response.menu_items_count}\n`;
            message += `- Hierarchy Defined: ${response.hierarchy_defined ? 'Yes' : 'No'}\n`;
            
            if (response.issues && response.issues.length > 0) {
                message += `\nIssues Found:\n${response.issues.join('\n')}`;
            } else {
                message += `\nNo issues found!`;
            }
            
            alert(message);
        })
        .fail(function() {
            alert('Error validating configuration');
        })
        .always(function() {
            button.prop('disabled', false).text('Validate Config');
        });
    });
    
    // Debug information for layout
    function updateDebugInfo() {
        const viewport = document.getElementById('viewport');
        const sidebarWidth = document.getElementById('sidebar-width');
        const contentPadding = document.getElementById('content-padding');
        
        if (viewport) viewport.textContent = window.innerWidth + 'x' + window.innerHeight;
        
        const sidebar = document.querySelector('nav');
        const content = document.querySelector('.app-content');
        
        if (sidebar && sidebarWidth) {
            const sidebarStyles = window.getComputedStyle(sidebar);
            sidebarWidth.textContent = sidebarStyles.width;
        }
        
        if (content && contentPadding) {
            const contentStyles = window.getComputedStyle(content);
            contentPadding.textContent = contentStyles.paddingLeft;
        }
    }
    
    updateDebugInfo();
    window.addEventListener('resize', updateDebugInfo);
    
});
</script>
@endsection
