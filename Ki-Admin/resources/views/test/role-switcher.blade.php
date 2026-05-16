{{--
/**
 * =========================================================
 * Project   : ki-admin - v1.0.0
 * File Name : role-switcher.blade.php
 * 
 * Author    : Michael Battle
 * Created On: 05/Aug/2025
 * Updated On: 05/Aug/2025
 * 
 * Description:
 * Development tool for testing role-based menu system by switching
 * user roles in local/development environments only.
 * 
 * Origin:
 * Role-based menu system testing interface
 * 
 * Changes:
 * - Created role switcher interface for development testing
 * - Added role descriptions and current role indicators
 * - Implemented role switching form with confirmations
 * - Added navigation to other testing tools
 * =========================================================
 */
--}}

@extends('layout.master')

@section('title', 'Role Switcher - Development Tool')
@section('css')
<style>
    .debug-card {
        background: #f8f9fa;
        border: 1px solid #dee2e6;
        border-radius: 8px;
        padding: 20px;
        margin-bottom: 20px;
    }
    .role-card {
        background: white;
        border: 1px solid #e3e6f0;
        border-radius: 8px;
        padding: 20px;
        margin-bottom: 15px;
        transition: all 0.3s ease;
    }
    .role-card:hover {
        box-shadow: 0 4px 8px rgba(0,0,0,0.1);
        transform: translateY(-2px);
    }
    .role-card.current {
        border-color: #007bff;
        background: #f8f9ff;
    }
    .warning-box {
        background: #fff3cd;
        border: 1px solid #ffeaa7;
        border-radius: 8px;
        padding: 15px;
        margin-bottom: 20px;
    }
</style>
@endsection

@section('main-content')
<div class="container-fluid">
    <!-- Breadcrumb start -->
    <div class="row m-1">
        <div class="col-12">
            <h4 class="main-title">Role Switcher - Development Tool</h4>
            <ul class="app-line-breadcrumbs mb-3">
                <li class="">
                    <a href="#" class="f-s-14 f-w-500">
                        <span>
                            <i class="ph-duotone ph-gear f-s-16"></i> Testing
                        </span>
                    </a>
                </li>
                <li class="active">
                    <a href="#" class="f-s-14 f-w-500">Role Switcher</a>
                </li>
            </ul>
        </div>
    </div>
    <!-- Breadcrumb end -->
    
    <!-- Warning Notice -->
    <div class="warning-box">
        <h6 class="text-warning mb-2">⚠️ Development Tool Only</h6>
        <p class="mb-0 text-dark">This role switcher is for testing purposes only and is disabled in production environments.</p>
    </div>

    <!-- Current User Info -->
    <div class="debug-card mb-4">
        <h5 class="mb-3">Current User</h5>
        @if($user)
            <p><strong>Email:</strong> {{ $user->email }}</p>
            <p><strong>Current Role:</strong> 
                <span class="badge bg-primary">{{ $currentRole }}</span>
            </p>
            <p><strong>Role ID:</strong> {{ $user->role_type_id ?? 'Not Set' }}</p>
        @else
            <p class="text-danger">No user logged in</p>
        @endif
    </div>

    <!-- Role Selection -->
    <div class="mb-4">
        <h5 class="mb-3">Switch to Different Role</h5>
        
        @if($user)
            <form method="POST" action="{{ route('test.switch-role') }}">
                @csrf
                
                @foreach($roles as $roleId => $roleName)
                    <div class="role-card {{ $user->role_type_id == $roleId ? 'current' : '' }}">
                        <div class="d-flex justify-content-between align-items-center">
                            <div>
                                <h6 class="mb-1">{{ $roleName }}</h6>
                                <p class="text-muted mb-0 small">
                                    @switch($roleId)
                                        @case(1)
                                            Full system access - Can manage everything
                                            @break
                                        @case(2)
                                            Administrative access - Can manage users and most features
                                            @break
                                        @case(3)
                                            Teaching tools - Student grades, class management
                                            @break
                                        @case(4)
                                            Support tools - Tickets, knowledge base
                                            @break
                                    @endswitch
                                </p>
                            </div>
                            <div>
                                @if($user->role_type_id == $roleId)
                                    <span class="badge bg-success">Current Role</span>
                                @else
                                    <button type="submit" name="role_type_id" value="{{ $roleId }}" 
                                            class="btn btn-outline-primary btn-sm"
                                            onclick="return confirm('Switch to {{ $roleName }} role?')">
                                        Switch to {{ $roleName }}
                                    </button>
                                @endif
                            </div>
                        </div>
                        
                        <!-- Menu Preview for this role -->
                        <div class="mt-3">
                            <button class="btn btn-sm btn-outline-info" type="button" 
                                    data-bs-toggle="collapse" 
                                    data-bs-target="#menu-preview-{{ $roleId }}" 
                                    aria-expanded="false">
                                <i class="ph-duotone ph-list"></i> Preview Menu
                            </button>
                            
                            <div class="collapse mt-2" id="menu-preview-{{ $roleId }}">
                                <div class="card card-body bg-light">
                                    <h6 class="mb-2">Menu Items for {{ $roleName }}:</h6>
                                    @php
                                        // Get menu preview for this role
                                        $tempUser = clone $user;
                                        $tempUser->role_type_id = $roleId;
                                        $tempUser->assignDefaultMenuItems();
                                        $menuPreview = \App\Helpers\MenuBuilder::getMenuForUser($tempUser);
                                    @endphp
                                    
                                    @if(empty($menuPreview))
                                        <p class="text-muted mb-0 small">No menu items available</p>
                                    @else
                                        <ul class="list-unstyled mb-0 small">
                                            @foreach($menuPreview as $item)
                                                <li class="mb-1">
                                                    <i class="ph-duotone ph-folder text-primary"></i> 
                                                    {{ $item['title'] }}
                                                    @if(!empty($item['children']))
                                                        <ul class="list-unstyled ms-3 mt-1">
                                                            @foreach($item['children'] as $child)
                                                                <li class="mb-1">
                                                                    <i class="ph-duotone ph-file text-secondary"></i> 
                                                                    {{ $child['title'] }}
                                                                </li>
                                                            @endforeach
                                                        </ul>
                                                    @endif
                                                </li>
                                            @endforeach
                                        </ul>
                                    @endif
                                </div>
                            </div>
                        </div>
                    </div>
                @endforeach
                
            </form>
        @else
            <div class="alert alert-warning">
                Please log in first to use the role switcher.
            </div>
        @endif
    </div>

    <!-- Current User's Menu -->
    @if($user)
    <div class="debug-card mb-4">
        <h5 class="mb-3">Current User's Menu Structure</h5>
        
        @php
            try {
                $currentMenu = \App\Helpers\MenuBuilder::getSidebarMenu();
                $userMenuCount = \App\Models\UserMenuItem::where('user_id', $user->user_id)->where('is_granted', true)->count();
            } catch (Exception $e) {
                $currentMenu = [];
                $userMenuCount = 0;
            }
        @endphp
        
        <div class="row">
            <div class="col-md-6">
                <h6 class="text-muted">User Menu Permissions</h6>
                <p class="mb-2">
                    <span class="badge bg-info">{{ $userMenuCount }}</span> 
                    user-specific menu items granted
                </p>
                
                @if($userMenuCount === 0)
                    <div class="alert alert-warning alert-sm">
                        <i class="ph-duotone ph-warning"></i>
                        No user-specific menu permissions found. 
                        <a href="#" onclick="assignDefaultMenus()" class="alert-link">
                            Assign default menus for current role
                        </a>
                    </div>
                @endif
            </div>
            
            <div class="col-md-6">
                <h6 class="text-muted">Active Menu Structure</h6>
                @if(empty($currentMenu))
                    <p class="text-muted">No menu items loaded</p>
                @else
                    <div class="menu-preview">
                        @foreach($currentMenu as $item)
                            <div class="mb-2">
                                <i class="ph-duotone ph-folder text-primary"></i> 
                                <strong>{{ $item['title'] }}</strong>
                                @if(!empty($item['children']))
                                    <div class="ms-3 mt-1">
                                        @foreach(array_slice($item['children'], 0, 3) as $child)
                                            <div class="small text-muted">
                                                <i class="ph-duotone ph-file"></i> {{ $child['title'] }}
                                            </div>
                                        @endforeach
                                        @if(count($item['children']) > 3)
                                            <div class="small text-muted">
                                                ... and {{ count($item['children']) - 3 }} more
                                            </div>
                                        @endif
                                    </div>
                                @endif
                            </div>
                        @endforeach
                    </div>
                @endif
            </div>
        </div>
    </div>
    @endif

    <!-- Navigation -->
    <div class="text-center">
        <div class="btn-group" role="group">
            <a href="{{ route('test.user-menu') }}" class="btn btn-primary">View User Menu</a>
            <a href="{{ route('test.role-access') }}" class="btn btn-outline-primary">Test All Roles</a>
            <a href="{{ route('test.view-composer-test') }}" class="btn btn-outline-info">View Composer Test</a>
            <a href="{{ route('index') }}" class="btn btn-outline-secondary">Back to Dashboard</a>
        </div>
    </div>

</div>
@endsection

@section('script')
<script>
$(document).ready(function() {
    // Auto-refresh menu after role switch
    @if(session('success'))
        setTimeout(function() {
            // Optional: redirect to menu test page to see changes
            // window.location.href = '{{ route("test.user-menu") }}';
        }, 2000);
    @endif
});

// Function to assign default menus for current user
function assignDefaultMenus() {
    if(confirm('Assign default menu permissions for your current role?')) {
        $.ajax({
            url: '{{ route("test.assign-default-menus") }}',
            method: 'POST',
            data: {
                _token: '{{ csrf_token() }}'
            },
            success: function(response) {
                if(response.success) {
                    location.reload(); // Refresh to show new menus
                } else {
                    alert('Error: ' + (response.message || 'Unknown error'));
                }
            },
            error: function() {
                alert('Error assigning default menus. Please try again.');
            }
        });
    }
}

// Function to preview menu for a specific role
function previewRoleMenu(roleId, roleName) {
    // This could be enhanced to show a modal with detailed menu preview
    console.log('Previewing menu for role:', roleId, roleName);
}
</script>
@endsection
