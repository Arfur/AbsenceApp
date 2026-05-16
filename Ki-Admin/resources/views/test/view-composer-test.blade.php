{{--
/**
 * =========================================================
 * Project   : ki-admin - v1.0.0
 * File Name : view-composer-test.blade.php
 * 
 * Author    : Michael Battle
 * Created On: 09/Aug/2025
 * Updated On: 09/Aug/2025
 * 
 * Description:
 * Test view for validating SidebarMenuComposer functionality.
 * Demonstrates automatic menu injection without manual controller setup.
 * 
 * Origin:
 * Step 4 of MenuBuilder Helper implementation - View Composer testing
 * 
 * Changes:
 * - Created View Composer test interface
 * - Added menu data validation display
 * - Implemented debugging information for menu injection
 * =========================================================
 */
--}}

@extends('layout.master')

@section('title', 'View Composer Test - Menu Injection')
@section('css')
<style>
    .test-card {
        background: #f8f9fa;
        border: 1px solid #dee2e6;
        border-radius: 8px;
        padding: 20px;
        margin-bottom: 20px;
    }
    .success-indicator {
        color: #28a745;
        font-weight: bold;
    }
    .error-indicator {
        color: #dc3545;
        font-weight: bold;
    }
    .menu-item {
        padding: 8px 12px;
        background: white;
        border: 1px solid #e3e6f0;
        border-radius: 4px;
        margin-bottom: 8px;
    }
    .menu-child {
        margin-left: 20px;
        padding: 4px 8px;
        background: #f8f9ff;
        border-left: 3px solid #007bff;
        margin-top: 4px;
    }
</style>
@endsection

@section('main-content')
<div class="container-fluid">
    <!-- Breadcrumb start -->
    <div class="row m-1">
        <div class="col-12">
            <h4 class="main-title">View Composer Test - Automatic Menu Injection</h4>
            <ul class="app-line-breadcrumbs mb-3">
                <li class="">
                    <a href="#" class="f-s-14 f-w-500">
                        <span>
                            <i class="ph-duotone ph-gear f-s-16"></i> Testing
                        </span>
                    </a>
                </li>
                <li class="active">
                    <a href="#" class="f-s-14 f-w-500">View Composer Test</a>
                </li>
            </ul>
        </div>
    </div>
    <!-- Breadcrumb end -->
    
    <!-- Test Message -->
    <div class="test-card">
        <h5 class="mb-3">Test Information</h5>
        <p>{{ $test_message }}</p>
        <p class="text-muted">This view should automatically receive sidebar menu data from the SidebarMenuComposer without any manual injection in the controller.</p>
    </div>

    <!-- View Composer Status -->
    <div class="test-card">
        <h5 class="mb-3">View Composer Status</h5>
        
        @if(isset($sidebarMenu))
            <p class="success-indicator">✅ SUCCESS: SidebarMenu data automatically injected!</p>
            <p><strong>Menu Items Count:</strong> {{ count($sidebarMenu) }}</p>
        @else
            <p class="error-indicator">❌ FAILED: SidebarMenu data not found!</p>
        @endif
        
        @if(isset($menuStats))
            <p class="success-indicator">✅ SUCCESS: Menu statistics automatically injected!</p>
            <div class="mt-3">
                <h6>Menu Statistics:</h6>
                <ul>
                    <li><strong>Total Items:</strong> {{ $menuStats['total_items'] ?? 'N/A' }}</li>
                    <li><strong>User Specific Items:</strong> {{ $menuStats['user_specific_items'] ?? 'N/A' }}</li>
                    <li><strong>Is Authenticated:</strong> {{ ($menuStats['is_authenticated'] ?? false) ? 'Yes' : 'No' }}</li>
                    @if(isset($menuStats['user_role']))
                        <li><strong>User Role:</strong> {{ $menuStats['user_role'] }}</li>
                    @endif
                    @if(isset($menuStats['role_template_items']))
                        <li><strong>Role Template Items:</strong> {{ $menuStats['role_template_items'] }}</li>
                    @endif
                    @if(isset($menuStats['cache_enabled']))
                        <li><strong>Cache Enabled:</strong> {{ $menuStats['cache_enabled'] ? 'Yes' : 'No' }}</li>
                    @endif
                    @if(isset($menuStats['error']))
                        <li class="error-indicator"><strong>Error:</strong> {{ $menuStats['error'] ? 'Yes' : 'No' }}</li>
                    @endif
                </ul>
            </div>
        @else
            <p class="error-indicator">❌ FAILED: Menu statistics not found!</p>
        @endif
        
        @if(isset($currentUser))
            <p class="success-indicator">✅ SUCCESS: Current user data automatically injected!</p>
            <p><strong>User:</strong> {{ $currentUser->email ?? 'N/A' }}</p>
            <p><strong>User ID:</strong> {{ $currentUser->user_id ?? 'N/A' }}</p>
        @else
            <p class="error-indicator">❌ INFO: No authenticated user (or user data not injected)</p>
        @endif
    </div>

    <!-- Menu Structure Display -->
    @if(isset($sidebarMenu) && !empty($sidebarMenu))
    <div class="test-card">
        <h5 class="mb-3">Injected Menu Structure</h5>
        
        <div class="menu-structure">
            @foreach($sidebarMenu as $item)
                <div class="menu-item">
                    <i class="ph-duotone {{ $item['icon'] ?? 'ph-folder' }}"></i>
                    <strong>{{ $item['title'] }}</strong>
                    <span class="text-muted">(ID: {{ $item['id'] }})</span>
                    
                    @if(!empty($item['children']))
                        @foreach($item['children'] as $child)
                            <div class="menu-child">
                                <i class="ph-duotone {{ $child['icon'] ?? 'ph-file' }}"></i>
                                {{ $child['title'] }}
                                <span class="text-muted">(ID: {{ $child['id'] }})</span>
                            </div>
                        @endforeach
                    @endif
                </div>
            @endforeach
        </div>
    </div>
    @endif

    <!-- Debug Information -->
    <div class="test-card">
        <h5 class="mb-3">Debug Information</h5>
        
        <div class="row">
            <div class="col-md-6">
                <h6>Available Variables:</h6>
                <ul>
                    <li>$sidebarMenu: {{ isset($sidebarMenu) ? 'Available' : 'Missing' }}</li>
                    <li>$menuStats: {{ isset($menuStats) ? 'Available' : 'Missing' }}</li>
                    <li>$currentUser: {{ isset($currentUser) ? 'Available' : 'Missing' }}</li>
                    <li>$test_message: {{ isset($test_message) ? 'Available' : 'Missing' }}</li>
                </ul>
            </div>
            
            <div class="col-md-6">
                <h6>View Composer Registration:</h6>
                <p class="text-muted">The SidebarMenuComposer should be registered in AppServiceProvider for these view patterns:</p>
                <ul class="small text-muted">
                    <li>layout.master</li>
                    <li>layout.sidebar</li>
                    <li>partial.sidebar</li>
                    <li>components.sidebar</li>
                    <li>test.* (this view)</li>
                </ul>
            </div>
        </div>
    </div>

    <!-- Navigation -->
    <div class="text-center">
        <div class="btn-group" role="group">
            <a href="{{ route('test.role-switcher') }}" class="btn btn-outline-primary">Role Switcher</a>
            <a href="{{ route('test.user-menu') }}" class="btn btn-outline-primary">User Menu Test</a>
            <a href="{{ route('index') }}" class="btn btn-outline-secondary">Back to Dashboard</a>
        </div>
    </div>

</div>
@endsection

@section('script')
<script>
$(document).ready(function() {
    // Log menu data to console for debugging
    console.log('View Composer Test Results:');
    console.log('SidebarMenu available:', {{ isset($sidebarMenu) ? 'true' : 'false' }});
    console.log('MenuStats available:', {{ isset($menuStats) ? 'true' : 'false' }});
    console.log('CurrentUser available:', {{ isset($currentUser) ? 'true' : 'false' }});
    
    @if(isset($sidebarMenu))
        console.log('Menu items count:', {{ count($sidebarMenu) }});
    @endif
    
    @if(isset($menuStats))
        console.log('Menu statistics:', @json($menuStats));
    @endif
});
</script>
@endsection
