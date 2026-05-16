{{-- 
    =========================================================
    Sidebar Menu Item Partial (Dynamic)
    Refactored to match static sample sidebar markup and logic
    - Uses 'another-level' for items with children, 'no-sub' for leaf items
    - Bootstrap collapse attributes for submenus
    - SVG icons, badges, and submenu structure consistent with sample
    - Unlimited nesting via recursion
    - Detailed comments for maintainability
    ========================================================= --}}



@extends('layout.master')
@section('title', 'Sidebar Comparison Test')
@section('main-content')
    <div class="container mt-4">
        <h2>Sidebar Attribute Comparison Test</h2>
        <div class="row">
            <div class="col-md-6">
                <h4>Sample Sidebar (Static)</h4>
                <ul class="main-nav">
                    {{-- Hardcoded sample items from z_ki-admin/layout/sidebar.blade.php --}}
                    <li class="another-level" style="border-left:none;">
                        <a aria-expanded="false" data-bs-toggle="collapse" href="#Profile-page">Profile</a>
                        <ul class="collapse" id="Profile-page">
                            <li><a href="/ki-admin/profile">Profile</a></li>
                            <li><a href="/ki-admin/setting">Setting</a></li>
                        </ul>
                    </li>
                    <li class="no-sub" style="border-left:none;">
                        <a href="/ki-admin/to_do">To-Do</a>
                    </li>
                    {{-- Add more sample items as needed for comparison --}}
                </ul>
            </div>
            <div class="col-md-6">
                <h4>Dashboard Sidebar (Dynamic)</h4>
                <ul class="main-nav">
                    @foreach($sidebarMenu as $menuItem)
                        <li class="{{ empty($menuItem['children']) ? 'no-sub' : 'another-level' }}" style="border-left:none;">
                            <a
                                href="{{ !empty($menuItem['children']) ? '#menu_' . $menuItem['id'] : ($menuItem['url'] ?? '#') }}"
                                @if(!empty($menuItem['children']))
                                    data-bs-toggle="collapse" aria-expanded="false"
                                @endif
                                style="font-size: 1rem;"
                            >
                                {{ $menuItem['title'] ?? 'Menu Item' }}
                            </a>
                        </li>
                    @endforeach
                </ul>
            </div>
        </div>
    </div>
@endsection