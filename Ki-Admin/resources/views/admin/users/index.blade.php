{{--
/**
 * =========================================================
 * Project   : ki-admin - v1.0.0
 * File Name : index.blade.php
 * 
 * Author    : Michael Battle
 * Created On: 06/Aug/2025
 * Updated On: 06/Aug/2025
 * 
 * Description:
 * Admin user management page for viewing and managing all system users.
 * Only accessible by admin and superadmin roles.
 * 
 * Origin:
 * Admin user management for ki-admin Support Hub
 * 
 * Changes:
 * - Created admin users listing page
 * - Added user management functionality
 * - Implemented role-based access control
 * =========================================================
 */
--}}

@extends('layout.master')
@section('title', 'User Management - Support Hub')
@section('css')
    <!-- DataTables CSS -->
    <link rel="stylesheet" href="{{asset('assets/vendor/datatables/datatables.min.css')}}">
    
    <!-- Custom Admin CSS -->
    <style>
        .user-avatar {
            width: 40px;
            height: 40px;
            border-radius: 50%;
            display: flex;
            align-items: center;
            justify-content: center;
            font-weight: bold;
            color: white;
        }
        .role-badge {
            font-size: 0.75rem;
        }
        .action-buttons .btn {
            padding: 0.25rem 0.5rem;
            font-size: 0.75rem;
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
            <h4 class="main-title">User Management</h4>
            <ul class="app-line-breadcrumbs mb-3">
                <li class="">
                    <a href="{{ route('dashboard') }}" class="f-s-14 f-w-500">
                        <span>
                            <i class="ph-duotone ph-house f-s-16"></i> Dashboard
                        </span>
                    </a>
                </li>
                <li class="">
                    <a href="#" class="f-s-14 f-w-500">Admin</a>
                </li>
                <li class="active">
                    <a href="#" class="f-s-14 f-w-500">Users</a>
                </li>
            </ul>
        </div>
    </div>

    {{-- =========================================================
     * Section: Statistics Cards
     * Description: User statistics overview
     * ========================================================= --}}
    <div class="row mb-4">
        <div class="col-md-3 col-sm-6 mb-3">
            <div class="card border-0 shadow">
                <div class="card-body text-center">
                    <div class="d-flex align-items-center justify-content-between">
                        <div>
                            <h3 class="mb-1 text-primary">0</h3>
                            <p class="mb-0 text-muted">Total Users</p>
                        </div>
                        <div class="bg-primary text-white rounded-circle p-3">
                            <i class="ph-duotone ph-users f-s-24"></i>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        
        <div class="col-md-3 col-sm-6 mb-3">
            <div class="card border-0 shadow">
                <div class="card-body text-center">
                    <div class="d-flex align-items-center justify-content-between">
                        <div>
                            <h3 class="mb-1 text-success">0</h3>
                            <p class="mb-0 text-muted">Active Users</p>
                        </div>
                        <div class="bg-success text-white rounded-circle p-3">
                            <i class="ph-duotone ph-check-circle f-s-24"></i>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="col-md-3 col-sm-6 mb-3">
            <div class="card border-0 shadow">
                <div class="card-body text-center">
                    <div class="d-flex align-items-center justify-content-between">
                        <div>
                            <h3 class="mb-1 text-warning">0</h3>
                            <p class="mb-0 text-muted">Pending Verification</p>
                        </div>
                        <div class="bg-warning text-white rounded-circle p-3">
                            <i class="ph-duotone ph-clock f-s-24"></i>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="col-md-3 col-sm-6 mb-3">
            <div class="card border-0 shadow">
                <div class="card-body text-center">
                    <div class="d-flex align-items-center justify-content-between">
                        <div>
                            <h3 class="mb-1 text-info">0</h3>
                            <p class="mb-0 text-muted">Admins</p>
                        </div>
                        <div class="bg-info text-white rounded-circle p-3">
                            <i class="ph-duotone ph-shield f-s-24"></i>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    {{-- =========================================================
     * Section: User Management Table
     * Description: DataTable showing all users with management actions
     * ========================================================= --}}
    <div class="row">
        <div class="col-12">
            <div class="card border-0 shadow">
                <div class="card-header bg-white border-bottom">
                    <div class="d-flex justify-content-between align-items-center">
                        <h5 class="card-title mb-0">
                            <i class="ph-duotone ph-users me-2 text-primary"></i>
                            All Users
                        </h5>
                        <button class="btn btn-primary btn-sm" onclick="createUser()">
                            <i class="ph-duotone ph-plus me-1"></i>
                            Add New User
                        </button>
                    </div>
                </div>
                <div class="card-body">
                    <div class="table-responsive">
                        <table id="usersTable" class="table table-hover">
                            <thead>
                                <tr>
                                    <th>User</th>
                                    <th>Email</th>
                                    <th>Role</th>
                                    <th>Status</th>
                                    <th>Verified</th>
                                    <th>Created</th>
                                    <th>Actions</th>
                                </tr>
                            </thead>
                            <tbody>
                                {{-- Sample user row - in real implementation this would be populated from database --}}
                                <tr>
                                    <td>
                                        <div class="d-flex align-items-center">
                                            <div class="user-avatar bg-primary me-2">
                                                {{ substr(auth()->user()->username, 0, 1) }}
                                            </div>
                                            <div>
                                                <p class="mb-0 fw-bold">{{ auth()->user()->username }}</p>
                                                <small class="text-muted">ID: {{ auth()->user()->user_id ?? auth()->user()->id }}</small>
                                            </div>
                                        </div>
                                    </td>
                                    <td>{{ auth()->user()->email }}</td>
                                    <td>
                                        <span class="badge role-badge bg-primary">{{ ucfirst(auth()->user()->role->name) }}</span>
                                    </td>
                                    <td>
                                        <span class="badge bg-success">Active</span>
                                    </td>
                                    <td>
                                        @if(auth()->user()->email_verified_at)
                                            <span class="badge bg-success">
                                                <i class="ph-duotone ph-check-circle me-1"></i>
                                                Yes
                                            </span>
                                        @else
                                            <span class="badge bg-warning">
                                                <i class="ph-duotone ph-clock me-1"></i>
                                                Pending
                                            </span>
                                        @endif
                                    </td>
                                    <td>
                                        <span data-bs-toggle="tooltip" title="{{ auth()->user()->created_at->format('M j, Y \a\t g:i A') }}">
                                            {{ auth()->user()->created_at->diffForHumans() }}
                                        </span>
                                    </td>
                                    <td>
                                        <div class="action-buttons">
                                            <button class="btn btn-outline-primary btn-sm me-1" onclick="viewUser({{ auth()->user()->id }})" data-bs-toggle="tooltip" title="View Details">
                                                <i class="ph-duotone ph-eye"></i>
                                            </button>
                                            <button class="btn btn-outline-success btn-sm me-1" onclick="editUser({{ auth()->user()->id }})" data-bs-toggle="tooltip" title="Edit User">
                                                <i class="ph-duotone ph-pencil"></i>
                                            </button>
                                            @if(auth()->user()->role === 'superadmin')
                                            <button class="btn btn-outline-danger btn-sm" onclick="deleteUser({{ auth()->user()->id }})" data-bs-toggle="tooltip" title="Delete User">
                                                <i class="ph-duotone ph-trash"></i>
                                            </button>
                                            @endif
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>

{{-- =========================================================
 * Section: User Management Modals
 * Description: Modals for user operations
 * ========================================================= --}}

<!-- User Details Modal -->
<div class="modal fade" id="userDetailsModal" tabindex="-1">
    <div class="modal-dialog modal-lg">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title">User Details</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
            </div>
            <div class="modal-body">
                <p>User details will be displayed here.</p>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
            </div>
        </div>
    </div>
</div>

<!-- Edit User Modal -->
<div class="modal fade" id="editUserModal" tabindex="-1">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title">Edit User</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
            </div>
            <div class="modal-body">
                <p>Edit user form will be displayed here.</p>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                <button type="button" class="btn btn-primary">Save Changes</button>
            </div>
        </div>
    </div>
</div>
@endsection

@section('script')
    <!-- DataTables JS -->
    <script src="{{asset('assets/vendor/datatables/datatables.min.js')}}"></script>
    
    <script>
        /* =====================================================
         * Section: User Management Functions
         * Description: JavaScript functions for user management
         * ===================================================== */
        
        function createUser() {
            alert('Create user functionality will be implemented in future updates.');
        }

        function viewUser(userId) {
            // Show user details modal
            $('#userDetailsModal').modal('show');
        }

        function editUser(userId) {
            // Show edit user modal
            $('#editUserModal').modal('show');
        }

        function deleteUser(userId) {
            if (confirm('Are you sure you want to delete this user? This action cannot be undone.')) {
                alert('Delete user functionality will be implemented in future updates.');
            }
        }

        // Initialize page
        document.addEventListener('DOMContentLoaded', function() {
            // Initialize DataTable
            if (typeof $.fn.DataTable !== 'undefined') {
                $('#usersTable').DataTable({
                    responsive: true,
                    order: [[5, 'desc']], // Sort by created date
                    pageLength: 25,
                    language: {
                        search: "_INPUT_",
                        searchPlaceholder: "Search users..."
                    }
                });
            }

            // Initialize tooltips
            var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
            var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
                return new bootstrap.Tooltip(tooltipTriggerEl);
            });

            console.log('User management page initialized');
        });
    </script>
@endsection
