{{--
 * =========================================================
 * Project   : ki-admin - v1.0.0
 * File Name : dashboard/dashboard-custom.blade.php
 * Author    : Michael Battle
 * Created On: 06/Aug/2025
 * Updated On: 11/Aug/2025
 * Description: Custom Dashboard view showing minimal breadcrumb layout.
 * ========================================================= --}}

@extends('layout.master')
@section('title', 'Dashboard - Custom Dashboard')
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
                    <a href="#" class="f-s-14 f-w-500">Custom Dashboard</a>
                </li>
            </ul>
        </div>
    </div>
</div>
@endsection
