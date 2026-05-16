{{-- 
/**
 * =========================================================
 * Project   : ki-admin - v1.0.0
 * File Name : login.blade.php
 * 
 * Author    : Michael Battle
 * Created On: 03/Aug/2025
 * Updated On: 03/Aug/2025
 * 
 * Description:
 * Custom login page using ki-admin design with functional authentication.
 * Includes validation error display and proper form submission.
 * 
 * Origin:
 * Authentication views for Support Hub - login form
 * 
 * Changes:
 * - Created functional login form using ki-admin styling
 * - Added Laravel form validation and error handling
 * - Integrated with custom LoginController
 * =========================================================
 */
--}}

<!DOCTYPE html>
<html lang="en">

@section('title', 'Login - Support Hub v2.0')
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
                         * Section: Login Form
                         * Description: Functional login form with validation
                         * ========================================================= --}}
                        
                        <form method="POST" action="{{ route('login') }}" class="app-form">
                            @csrf
                            <div class="row">
                                <div class="col-12">
                                    <div class="mb-5 text-center text-lg-start">
                                        <h2 class="text-white f-w-600">Sign in to <span class="text-dark">Support Hub</span> </h2>
                                        <p class="f-s-16 mt-2">Welcome back! Please enter your details</p>
                                        {{-- UPDATED AUTH VIEW - chg0023 --}}
                                    </div>

                                    {{-- Success/Error Messages --}}
                                    @if(session('success'))
                                        <div class="alert alert-success alert-dismissible fade show" role="alert">
                                            <i class="ph ph-check-circle me-2"></i>
                                            <div style="line-height: 1.5;">
                                                {!! nl2br(e(session('success'))) !!}
                                            </div>
                                            <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                                        </div>
                                    @endif

                                    @if(session('error'))
                                        <div class="alert alert-danger alert-dismissible fade show" role="alert">
                                            <i class="ph ph-x-circle me-2"></i>{{ session('error') }}
                                            <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                                        </div>
                                    @endif

                                    @if($errors->any())
                                        <div class="alert alert-danger alert-dismissible fade show" role="alert">
                                            <i class="ph ph-warning me-2"></i>
                                            @foreach($errors->all() as $error)
                                                {{ $error }}<br>
                                            @endforeach
                                            <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                                        </div>
                                    @endif
                                </div>

                                {{-- Username Field --}}
                                <div class="col-12">
                                    <div class="form-floating mb-3">
                                        <input class="form-control @error('username') is-invalid @enderror" 
                                               id="username" 
                                               name="username"
                                               placeholder="Username"
                                               type="text"
                                               value="{{ old('username') }}"
                                               required autofocus>
                                        <label for="username">Username</label>
                                        @error('username')
                                            <div class="invalid-feedback">
                                                {{ $message }}
                                            </div>
                                        @enderror
                                    </div>
                                    <div class="mb-3">
                                        <small class="text-muted">
                                            <i class="ph ph-info f-s-16 me-1"></i>
                                            Enter the username that was created when you registered.
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
                                    <div class="text-end ">
                                        <a class="text-dark f-w-500 text-decoration-underline" href="#">Forgot password</a>
                                    </div>
                                </div>

                                {{-- Remember Me --}}
                                <div class="col-12">
                                    <div class="form-check d-flex align-items-center gap-2 mb-3">
                                        <input class="form-check-input w-25 h-25" 
                                               id="remember" 
                                               name="remember"
                                               type="checkbox"
                                               value="1">
                                        <label class="form-check-label text-white mt-2 f-s-16 text-dark"
                                               for="remember">
                                            Remember me
                                        </label>
                                    </div>
                                </div>

                                {{-- Submit Button --}}
                                <div class="col-12 mt-3">
                                    <button type="submit" class="btn btn-primary btn-lg w-100">Sign In</button>
                                </div>

                                {{-- Registration Link --}}
                                <div class="col-12 mt-4">
                                    <div class="text-center text-lg-start f-w-500">
                                        Don't Have Your Account yet?
                                        <a class="text-white-800 text-decoration-underline" href="{{ route('register') }}">Sign up</a>
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
