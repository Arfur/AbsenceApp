{{--
 * =========================================================
 * Project   : ki-admin - v1.0.0
 * File Name : dashboard/dashboard.blade.php
 * Author    : Michael Battle
 * Created On: 06/Aug/2025
 * Updated On: 11/Aug/2025
 * Description: Main dashboard view for authenticated users showing activity summary, quick actions, and role-based content. Serves as the default landing page after successful login.
 * Origin: Primary dashboard for ki-admin Support Hub application
 * Changes: See PT file for full change log
 * ========================================================= --}}

@extends('layout.master')
@section('title', 'Dashboard - Support Hub')
{{-- Section: Custom Dashboard CSS --}}
@section('css')
    <style>
        .dashboard-welcome-card {
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white;
            border-radius: 15px;
            padding: 2rem;
        }
        .stat-icon {
            width: 60px;
            height: 60px;
            border-radius: 50%;
            display: flex;
            align-items: center;
            justify-content: center;
        .activity-card {
            border-radius: 10px;
            transition: transform 0.2s;
        }
        .activity-card:hover {
            transform: translateY(-5px);
        }
    </style>
@endsection
{{-- Section: Main Dashboard Content --}}
@section('main-content')
<div class="container-fluid">
    <div class="row m-1 mt-2">
        <div class="col-12">
            <h4 class="main-title">Dashboard</h4>
            <ul class="app-line-breadcrumbs mb-3">
                <li class="">
                    <a href="#" class="f-s-14 f-w-500">
                        <span>
                            <i class="ph-duotone ph-house f-s-16"></i> Home
                        </span>
                    </a>
                </li>
                <li class="active">
                    <a href="#" class="f-s-14 f-w-500">My Activity</a>
                </li>
            </ul>
        </div>
    </div>
    <div class="row mb-4">
        <div class="col-12">
            <div class="card dashboard-welcome-card">
                <div class="card-body">
                    <div class="d-flex align-items-center">
                        <div class="avatar-image avatar-lg me-3">
                            <div class="avatar-image-placeholder bg-white text-primary rounded-circle d-flex align-items-center justify-content-center" style="width: 60px; height: 60px; font-size: 24px; font-weight: bold;">
                                {{ substr(auth()->user()->username, 0, 1) }}
                            </div>
                        </div>
                        <div>
                            <h2 class="mb-1 text-white">Welcome back, {{ auth()->user()->username }}!</h2>
                            <p class="mb-0 text-white-50">
                                Role: <span class="badge bg-white text-primary">{{ optional(auth()->user()->roleType)->display_name ?? 'No Role Assigned' }}</span>
                                @if(auth()->user()->email_verified_at)
                                    <span class="badge bg-success ms-2">Email Verified</span>
                                @else
                                    <span class="badge bg-warning ms-2">Email Pending Verification</span>
                                @endif
                            </p>
                            <small class="text-white-50">Last login: {{ now()->format('M j, Y \a\t g:i A') }}</small>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row mb-4 justify-content-center">
        <div class="col-lg-4 col-md-6 mb-3 d-flex">
            <div class="card activity-card border-0 shadow flex-fill">
                <div class="card-body text-center">
                    <div class="stat-icon bg-primary text-white mx-auto mb-3">
                        <i class="ph-duotone ph-ticket"></i>
                    </div>
                    <h5>Support Tickets</h5>
                    <p class="text-muted">Create or view your tickets</p>
                    <a href="#" class="btn btn-outline-primary btn-sm">View Tickets</a>
                </div>
            </div>
        </div>
        <div class="col-lg-4 col-md-6 mb-3 d-flex">
            <div class="card activity-card border-0 shadow flex-fill">
                <div class="card-body text-center">
                    <div class="stat-icon bg-success text-white mx-auto mb-3">
                        <i class="ph-duotone ph-book"></i>
                    </div>
                    <h5>Knowledge Base</h5>
                    <p class="text-muted">Browse helpful articles</p>
                    <a href="#" class="btn btn-outline-success btn-sm">Browse Articles</a>
                </div>
            </div>
        </div>
        @if(optional(auth()->user()->roleType)->name === 'super_admin')
        <div class="col-lg-4 col-md-6 mb-3 d-flex">
            <div class="card activity-card border-0 shadow flex-fill">
                <div class="card-body text-center">
                    <div class="stat-icon bg-warning text-white mx-auto mb-3">
                        <i class="ph-duotone ph-user"></i>
                    </div>
                    <h5>My Profile</h5>
                    <p class="text-muted">Update your information</p>
                    <a href="#" class="btn btn-outline-info btn-sm">Edit Profile</a>
                </div>
            </div>
        </div>
        @endif
    </div>
    <div class="row">
        <div class="col-lg-8 mb-4">
            <div class="card border-0 shadow">
                <div class="card-header bg-white border-bottom">
                    <h5 class="card-title mb-0">Recent Activity</h5>
                </div>
                <div class="card-body">
                    <div class="timeline">
                        <div class="timeline-item d-flex align-items-center mb-3">
                            <div class="timeline-marker bg-primary rounded-circle me-3" style="width: 12px; height: 12px;"></div>
                            <div>
                                <p class="mb-1"><strong>Account Created</strong></p>
                                <small class="text-muted">{{ auth()->user()->created_at->format('M j, Y \a\t g:i A') }}</small>
                            </div>
                        </div>
                        @if(auth()->user()->email_verified_at)
                        <div class="timeline-item d-flex align-items-center mb-3">
                            <div class="timeline-marker bg-success rounded-circle me-3" style="width: 12px; height: 12px;"></div>
                            <div>
                                <p class="mb-1"><strong>Email Verified</strong></p>
                                <small class="text-muted">{{ auth()->user()->email_verified_at->format('M j, Y \a\t g:i A') }}</small>
                            </div>
                        </div>
                        @endif
                        <div class="timeline-item d-flex align-items-center mb-3">
                            <div class="timeline-marker bg-info rounded-circle me-3" style="width: 12px; height: 12px;"></div>
                            <div>
                                <p class="mb-1"><strong>Dashboard Access</strong></p>
                                <small class="text-muted">{{ now()->format('M j, Y \a\t g:i A') }}</small>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-lg-4 mb-4">
            <div class="card border-0 shadow">
                <div class="card-header bg-white border-bottom">
                    <h5 class="card-title mb-0">Quick Stats</h5>
                </div>
                <div class="card-body">
                    <div class="d-flex justify-content-between align-items-center mb-3">
                        <span>Open Tickets</span>
                        <span class="badge bg-danger">0</span>
                    </div>
                    <div class="d-flex justify-content-between align-items-center mb-3">
                        <span>Resolved Tickets</span>
                        <span class="badge bg-success">0</span>
                    </div>
                    <div class="d-flex justify-content-between align-items-center mb-3">
                        <span>Articles Read</span>
                        <span class="badge bg-info">0</span>
                    </div>
                    <div class="d-flex justify-content-between align-items-center">
                        <span>Account Status</span>
                        <span class="badge bg-primary">Active</span>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
@endsection
{{-- Section: Dashboard Page Scripts --}}
@section('script')
    <script>
        document.addEventListener('DOMContentLoaded', function() {
            const activityCards = document.querySelectorAll('.activity-card');
            activityCards.forEach((card, index) => {
                card.style.animationDelay = `${index * 0.1}s`;
            });
            setInterval(function() {
                console.log('Refreshing dashboard stats...');
            }, 300000);
        });
    </script>
@endsection
