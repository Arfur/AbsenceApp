{{-- 
/**
 * =========================================================
 * Project   : ki-admin - v1.0.0
 * File Name : verify-email.blade.php
 * 
 * Author    : Michael Battle
 * Created On: 03/Aug/2025
 * Updated On: 03/Aug/2025
 * 
 * Description:
 * Email verification notice page using ki-admin design.
 * Displays verification message and resend functionality.
 * 
 * Origin:
 * Authentication views for Support Hub - email verification
 * 
 * Changes:
 * - Created email verification page using ki-admin styling
 * - Added resend verification functionality
 * - Integrated with EmailVerificationController
 * =========================================================
 */
--}}

<!DOCTYPE html>
<html lang="en">

@section('title', 'Verify Email - Support Hub')
@include('layout.head')

@include('layout.css')
<!-- phosphor-icon css-->
<link href="{{asset('../assets/vendor/phosphor/phosphor-bold.css')}}" rel="stylesheet">
<body class="sign-in-bg">
<div class="app-wrapper d-block">
    <div class="">
        <!-- Body main section starts -->
        <div class="container main-container">
            <div class="row main-content-box">
                <div class="col-lg-7 image-content-box d-none d-lg-block">
                    <div class="form-container">
                        <div class="signup-content mt-4">
                            <span>
                              <img alt="" class="img-fluid " src="../assets/images/logo/1.png">
                            </span>
                        </div>

                        <div class="signup-bg-img">
                            <img alt="" class="img-fluid" src="../assets/images/login/01.png">
                        </div>
                    </div>
                </div>
                
                <div class="col-lg-5 form-content-box">
                    <div class="form-container ">
                        {{-- =========================================================
                         * Section: Email Verification Notice
                         * Description: Display verification instructions and resend option
                         * ========================================================= --}}
                        
                        <div class="app-form">
                            <div class="row">
                                <div class="col-12">
                                    <div class="mb-5 text-center text-lg-start">
                                        <h2 class="text-white f-w-600">Verify Your <span class="text-dark">Email</span></h2>
                                        <p class="f-s-16 mt-2">We've sent a verification link to your email address</p>
                                    </div>
                                </div>

                                {{-- Verification Message --}}
                                <div class="col-12">
                                    <div class="alert alert-info mb-4">
                                        <i class="ph-bold ph-envelope f-s-20 me-2"></i>
                                        <strong>Check your email!</strong><br>
                                        Before proceeding, please check your email for a verification link. 
                                        If you didn't receive the email, we can send another one.
                                    </div>
                                </div>

                                {{-- Success Message --}}
                                @if (session('status') == 'verification-link-sent')
                                    <div class="col-12">
                                        <div class="alert alert-success mb-4">
                                            <i class="ph-bold ph-check-circle f-s-20 me-2"></i>
                                            A new verification link has been sent to your email address.
                                        </div>
                                    </div>
                                @endif

                                {{-- Resend Verification Button --}}
                                <div class="col-12 mt-3">
                                    <form method="POST" action="{{ route('verification.send') }}">
                                        @csrf
                                        <button type="submit" class="btn btn-primary btn-lg w-100">
                                            <i class="ph-bold ph-paper-plane-tilt me-2"></i>
                                            Resend Verification Email
                                        </button>
                                    </form>
                                </div>

                                {{-- Logout Link --}}
                                <div class="col-12 mt-4">
                                    <div class="text-center text-lg-start f-w-500">
                                        Wrong email address?
                                        <form method="POST" action="{{ route('logout') }}" class="d-inline">
                                            @csrf
                                            <button type="submit" class="btn btn-link text-white-800 text-decoration-underline p-0 align-baseline">
                                                Sign out and try again
                                            </button>
                                        </form>
                                    </div>
                                </div>

                                {{-- Additional Help --}}
                                <div class="col-12 mt-4">
                                    <div class="text-center">
                                        <small class="text-muted">
                                            <i class="ph-bold ph-info f-s-16 me-1"></i>
                                            Check your spam folder if you don't see the email in your inbox.
                                        </small>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-- Body main section ends -->
    </div>
</div>

<!-- Bootstrap js-->
<script src="{{asset('assets/vendor/bootstrap/bootstrap.bundle.min.js')}}"></script>

</body>
</html>
