{{--
 * =========================================================
 * Project   : ki-admin - v1.0.0
 * File Name : dashboard/dashboard-overview.blade.php
 * Author    : Michael Battle
 * Created On: 06/Aug/2025
 * Updated On: 11/Aug/2025
 * Description: System Overview dashboard view showing minimal breadcrumb layout.
 * Origin: Primary dashboard for ki-admin Support Hub application
 * Changes: See PT file for full change log
 * ========================================================= --}}

@extends('layout.master')
@section('title', 'Dashboard - System Overview')
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
                    <a href="#" class="f-s-14 f-w-500">System Overview</a>
                </li>
            </ul>
        </div>
    </div>
</div>
@endsection
