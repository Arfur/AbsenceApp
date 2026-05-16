{{-- 
/**
 * =========================================================
 * Project   : ki-admin - v1.0.0
 * File Name : register.blade.php
 * 
 * Author    : Michael Battle
 * Created On: 03/Aug/2025
 * Updated On: 03/Aug/2025
 * 
 * Description:
 * Custom registration page using ki-admin design with functional registration.
 * Includes domain validation and proper form submission.
 * 
 * Origin:
 * Authentication views for Support Hub - registration form
 * 
 * Changes:
 * - Created functional registration form using ki-admin styling
 * - Added Laravel form validation and error handling
 * - Integrated with custom RegisterController
 * =========================================================
 */
--}}

<!DOCTYPE html>
<html lang="en">

@section('title', 'Register - Support Hub')
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
                         * Section: Registration Form
                         * Description: Functional registration form with validation
                         * ========================================================= --}}
                        
                        <form method="POST" action="{{ route('register') }}" class="app-form">
                            @csrf
                            <div class="row">
                                <div class="col-12">
                                    <div class="mb-5 text-center text-lg-start">
                                        <h2 class="text-white f-w-600">Join <span class="text-dark">Support Hub!</span> </h2>
                                        <p class="f-s-16 mt-2">Create your account with your email address and password</p>
                                    </div>
                                </div>

                                {{-- Email Field --}}
                                <div class="col-12">
                                    <div class="form-floating mb-3">
                                        <input class="form-control @error('email') is-invalid @enderror" 
                                               id="email" 
                                               name="email"
                                               placeholder="Email"
                                               type="email"
                                               value="{{ old('email') }}"
                                               required autofocus>
                                        <label for="email">Email Address</label>
                                        @error('email')
                                            <div class="invalid-feedback">
                                                {{ $message }}
                                            </div>
                                        @enderror
                                    </div>
                                    <div class="mb-3">
                                        <small class="text-muted">
                                            <i class="ph ph-info f-s-16 me-1"></i>
                                            Your username will be automatically created from your email address (the part before @).
                                        </small>
                                    </div>
                                </div>

                                {{-- Password Field --}}
                                <div class="col-12">
                                    <div class="form-floating mb-3">
                                        <input class="form-control @error('password') is-invalid @enderror" 
                                               id="password" 
                                               name="password"
                                               placeholder="Password"
                                               type="password"
                                               required>
                                        <label for="password">Password</label>
                                        @error('password')
                                            <div class="invalid-feedback">
                                                {{ $message }}
                                            </div>
                                        @enderror
                                    </div>
                                </div>

                                {{-- Confirm Password Field --}}
                                <div class="col-12">
                                    <div class="form-floating mb-3">
                                        <input class="form-control @error('password_confirmation') is-invalid @enderror" 
                                               id="password_confirmation" 
                                               name="password_confirmation"
                                               placeholder="Confirm Password"
                                               type="password"
                                               required>
                                        <label for="password_confirmation">Confirm Password</label>
                                        @error('password_confirmation')
                                            <div class="invalid-feedback">
                                                {{ $message }}
                                            </div>
                                        @enderror
                                    </div>
                                </div>

                                {{-- Submit Button --}}
                                <div class="col-12 mt-3">
                                    <button type="submit" class="btn btn-primary btn-lg w-100">Create Account</button>
                                </div>

                                {{-- Login Link --}}
                                <div class="col-12 mt-4">
                                    <div class="text-center text-lg-start f-w-500">
                                        Already have an account?
                                        <a class="text-white-800 text-decoration-underline" href="{{ route('login') }}">Sign in</a>
                                    </div>
                                </div>
                            </div>
                        </form>
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
