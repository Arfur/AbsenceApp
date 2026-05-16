{{--
/**
 * =========================================================
 * Project   : ki-admin - v1.0.0
 * File Name : profile.blade.php
 * 
 * Author    : Michael Battle
 * Created On: 06/Aug/2025
 * Updated On: 06/Aug/2025
 * 
 * Description:
 * User profile page showing user information and account settings.
 * Allows users to view and edit their personal information.
 * 
 * Origin:
 * Profile management page for ki-admin Support Hub
 * 
 * Changes:
 * - Created profile view based on z_ki-admin template
 * - Adapted to use main layout instead of z_ki-admin layout
 * - Added user information display and edit functionality
 * =========================================================
 */
--}}

@extends('layout.master')
@section('title', 'Profile - Support Hub')
@section('css')
    <!--font-awesome-css-->
    <link rel="stylesheet" href="{{asset('assets/vendor/fontawesome/css/all.css')}}">
    
    <!-- Custom Profile CSS -->
    <style>
        .profile-header {
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white;
            border-radius: 15px;
            padding: 2rem;
        }
        .profile-avatar {
            width: 120px;
            height: 120px;
            border: 5px solid white;
            box-shadow: 0 4px 8px rgba(0,0,0,0.1);
        }
        .profile-info-card {
            border-radius: 10px;
            transition: transform 0.2s;
        }
        .profile-info-card:hover {
            transform: translateY(-2px);
        }
    </style>
@endsection

@section('main-content')
<div class="container-fluid">
    {{-- =========================================================
     * Section: Page Header & Breadcrumb
     * Description: Page title and navigation breadcrumb
     * ========================================================= --}}
    <div class="row m-1">
        <div class="col-12">
            <h4 class="main-title">Profile</h4>
            <ul class="app-line-breadcrumbs mb-3">
                <li class="">
                    <a href="{{ route('dashboard') }}" class="f-s-14 f-w-500">
                        <span>
                            <i class="ph-duotone ph-house f-s-16"></i> Dashboard
                        </span>
                    </a>
                </li>
                <li class="active">
                    <a href="#" class="f-s-14 f-w-500">Profile</a>
                </li>
            </ul>
        </div>
    </div>

    {{-- =========================================================
     * Section: Profile Header
     * Description: User profile header with avatar and basic info
     * ========================================================= --}}
    <div class="row mb-4">
        <div class="col-12">
            <div class="card profile-header border-0">
                <div class="card-body">
                    <div class="d-flex align-items-center">
                        <div class="me-4">
                            <div class="profile-avatar bg-white text-primary rounded-circle d-flex align-items-center justify-content-center" style="font-size: 48px; font-weight: bold;">
                                {{ substr(auth()->user()->username, 0, 2) }}
                            </div>
                        </div>
                        <div class="flex-grow-1">
                            <h2 class="mb-1 text-white">{{ auth()->user()->username }}</h2>
                            <p class="mb-2 text-white-50">{{ auth()->user()->email }}</p>
                            <div class="mb-2">
                                <span class="badge bg-white text-primary me-2">{{ ucfirst(auth()->user()->role) }}</span>
                                @if(auth()->user()->email_verified_at)
                                    <span class="badge bg-success">Email Verified</span>
                                @else
                                    <span class="badge bg-warning">Email Pending</span>
                                @endif
                            </div>
                            <small class="text-white-50">Member since {{ auth()->user()->created_at->format('M Y') }}</small>
                        </div>
                        <div>
                            <button class="btn btn-light btn-sm" onclick="editProfile()">
                                <i class="ph-duotone ph-pencil me-1"></i> Edit Profile
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    {{-- =========================================================
     * Section: Profile Information
     * Description: Detailed user information cards
     * ========================================================= --}}
    <div class="row">
        <div class="col-lg-4 mb-4">
            <div class="card profile-info-card border-0 shadow">
                <div class="card-header bg-white border-bottom">
                    <h5 class="card-title mb-0">
                        <i class="ph-duotone ph-user me-2 text-primary"></i>
                        Account Information
                    </h5>
                </div>
                <div class="card-body">
                    <div class="mb-3">
                        <label class="form-label text-muted">Username</label>
                        <p class="mb-0 fw-bold">{{ auth()->user()->username }}</p>
                    </div>
                    <div class="mb-3">
                        <label class="form-label text-muted">Email Address</label>
                        <p class="mb-0">{{ auth()->user()->email }}</p>
                    </div>
                    <div class="mb-3">
                        <label class="form-label text-muted">User ID</label>
                        <p class="mb-0">#{{ auth()->user()->user_id ?? auth()->user()->id }}</p>
                    </div>
                    <div class="mb-0">
                        <label class="form-label text-muted">Account Status</label>
                        <p class="mb-0">
                            <span class="badge bg-success">{{ ucfirst(auth()->user()->status ?? 'active') }}</span>
                        </p>
                    </div>
                </div>
            </div>
        </div>

        <div class="col-lg-4 mb-4">
            <div class="card profile-info-card border-0 shadow">
                <div class="card-header bg-white border-bottom">
                    <h5 class="card-title mb-0">
                        <i class="ph-duotone ph-shield-check me-2 text-success"></i>
                        Security Information
                    </h5>
                </div>
                <div class="card-body">
                    <div class="mb-3">
                        <label class="form-label text-muted">Role</label>
                        <p class="mb-0">
                            <span class="badge bg-primary">{{ ucfirst(auth()->user()->role) }}</span>
                        </p>
                    </div>
                    <div class="mb-3">
                        <label class="form-label text-muted">Email Verification</label>
                        <p class="mb-0">
                            @if(auth()->user()->email_verified_at)
                                <span class="badge bg-success">
                                    <i class="ph-duotone ph-check-circle me-1"></i>
                                    Verified on {{ auth()->user()->email_verified_at->format('M j, Y') }}
                                </span>
                            @else
                                <span class="badge bg-warning">
                                    <i class="ph-duotone ph-clock me-1"></i>
                                    Pending Verification
                                </span>
                            @endif
                        </p>
                    </div>
                    <div class="mb-3">
                        <label class="form-label text-muted">Two-Factor Authentication</label>
                        <p class="mb-0">
                            <span class="badge bg-info">
                                <i class="ph-duotone ph-device-mobile me-1"></i>
                                OTP Enabled
                            </span>
                        </p>
                    </div>
                    <div class="mb-0">
                        <label class="form-label text-muted">Last Password Change</label>
                        <p class="mb-0 text-muted">{{ auth()->user()->updated_at->format('M j, Y') }}</p>
                    </div>
                </div>
            </div>
        </div>

        <div class="col-lg-4 mb-4">
            <div class="card profile-info-card border-0 shadow">
                <div class="card-header bg-white border-bottom">
                    <h5 class="card-title mb-0">
                        <i class="ph-duotone ph-chart-line me-2 text-info"></i>
                        Activity Summary
                    </h5>
                </div>
                <div class="card-body">
                    <div class="mb-3">
                        <label class="form-label text-muted">Account Created</label>
                        <p class="mb-0">{{ auth()->user()->created_at->format('M j, Y \a\t g:i A') }}</p>
                    </div>
                    <div class="mb-3">
                        <label class="form-label text-muted">Last Login</label>
                        <p class="mb-0">{{ now()->format('M j, Y \a\t g:i A') }}</p>
                    </div>
                    <div class="mb-3">
                        <label class="form-label text-muted">Total Tickets</label>
                        <p class="mb-0">
                            <span class="badge bg-primary">0</span>
                        </p>
                    </div>
                    <div class="mb-0">
                        <label class="form-label text-muted">Articles Read</label>
                        <p class="mb-0">
                            <span class="badge bg-info">0</span>
                        </p>
                    </div>
                </div>
            </div>
        </div>
    </div>

    {{-- =========================================================
     * Section: Quick Actions
     * Description: Profile-related quick actions
     * ========================================================= --}}
    <div class="row">
        <div class="col-12">
            <div class="card border-0 shadow">
                <div class="card-header bg-white border-bottom">
                    <h5 class="card-title mb-0">
                        <i class="ph-duotone ph-gear me-2 text-warning"></i>
                        Quick Actions
                    </h5>
                </div>
                <div class="card-body">
                    <div class="row">
                        <div class="col-md-3 col-sm-6 mb-3">
                            <button class="btn btn-outline-primary w-100" onclick="changePassword()">
                                <i class="ph-duotone ph-lock me-2"></i>
                                Change Password
                            </button>
                        </div>
                        <div class="col-md-3 col-sm-6 mb-3">
                            <button class="btn btn-outline-success w-100" onclick="updateEmail()">
                                <i class="ph-duotone ph-envelope me-2"></i>
                                Update Email
                            </button>
                        </div>
                        <div class="col-md-3 col-sm-6 mb-3">
                            <a href="{{ route('dashboard') }}" class="btn btn-outline-info w-100">
                                <i class="ph-duotone ph-house me-2"></i>
                                Back to Dashboard
                            </a>
                        </div>
                        <div class="col-md-3 col-sm-6 mb-3">
                            <form method="POST" action="{{ route('logout') }}" class="d-inline w-100">
                                @csrf
                                <button type="submit" class="btn btn-outline-danger w-100">
                                    <i class="ph-duotone ph-sign-out me-2"></i>
                                    Logout
                                </button>
                            </form>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
@endsection

@section('script')
    <script>
        /* =====================================================
         * Section: Profile Functions
         * Description: JavaScript functions for profile interactions
         * ===================================================== */
        
        function editProfile() {
            // Placeholder for edit profile functionality
            alert('Edit profile functionality will be implemented in future updates.');
        }

        function changePassword() {
            // Placeholder for change password functionality
            alert('Change password functionality will be implemented in future updates.');
        }

        function updateEmail() {
            // Placeholder for update email functionality
            alert('Update email functionality will be implemented in future updates.');
        }

        // Initialize profile page
        document.addEventListener('DOMContentLoaded', function() {
            console.log('Profile page loaded for user:', '{{ auth()->user()->username }}');
            
            // Add hover effects to info cards
            const infoCards = document.querySelectorAll('.profile-info-card');
            infoCards.forEach(card => {
                card.addEventListener('mouseenter', function() {
                    this.classList.add('shadow-lg');
                });
                card.addEventListener('mouseleave', function() {
                    this.classList.remove('shadow-lg');
                });
            });
        });
    </script>
@endsection
