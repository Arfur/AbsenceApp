<!DOCTYPE html>
<html lang="en">

@section('title', 'Sign In')
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
                        <form class="app-form">
                            <div class="row">
                                <div class="col-12">
                                    <div class="mb-5 text-center text-lg-start">
                                        <h2 class="text-white f-w-600">Welcome To <span class="text-dark">ki-admin!</span> </h2>
                                        <p class="f-s-16 mt-2">Sign in with your data that you enterd during your
                                            registration</p>
                                    </div>
                                </div>
                                <div class="col-12">
                                    <div class="form-floating mb-3">
                                        <input class="form-control" id="UserName" placeholder="Email Username"
                                               type="text">
                                        <label for="UserName">Username</label>
                                    </div>
                                </div>
                                <div class="col-12">

                                    <div class="form-floating mb-3">
                                        <input class="form-control" id="floatingInput" placeholder="Email Username"
                                               type="text">
                                        <label for="floatingInput">Password</label>

                                    </div>
                                    <div class="text-end ">
                                        <a class="text-dark f-w-500 text-decoration-underline" href="#">Forgot password</a>
                                    </div>
                                </div>
                                <div class="col-12">
                                    <div class="form-check d-flex align-items-center gap-2 mb-3">
                                        <input class="form-check-input w-25 h-25" id="checkDefault" type="checkbox"
                                               value="">
                                        <label class="form-check-label text-white mt-2 f-s-16 text-dark"
                                               for="checkDefault">
                                            Remember me
                                        </label>
                                    </div>
                                </div>
                                <div class="col-12 mt-3">
                                    <a class="btn btn-primary btn-lg w-100" href="/ki-admin/index" role="button">Sign In</a>
                                </div>
                                <div class="col-12 mt-4">
                                    <div class="text-center text-lg-start f-w-500">
                                        Don't Have Your Account yet?
                                        <a class="text-white-800 text-decoration-underline" href="/ki-admin/sign_up">Sign up</a>
                                    </div>
                                </div>

                                <div class="app-divider-v light justify-content-center py-lg-5 py-3">
                                    <p>OR</p>
                                </div>

                                <div class="col-12">
                                    <div class="d-flex gap-3 justify-content-center text-center">
                                        <button class="btn btn-light-white  icon-btn w-45 h-45 b-r-15 " type="button">
                                            <i class="ph-bold ph-facebook-logo f-s-20"></i>
                                        </button>
                                        <button class="btn btn-light-white  icon-btn w-45 h-45 b-r-15 " type="button">
                                            <i class="ph-bold  ph-google-logo f-s-20"></i>
                                        </button>
                                        <button class="btn btn-light-white  icon-btn w-45 h-45 b-r-15 " type="button">
                                            <i class="ph-bold  ph-twitter-logo f-s-20"></i>
                                        </button>
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
