<!DOCTYPE html>
<html lang="en">
@section('title', 'Password Create')
@include('layout.head')

@include('layout.css')

<body class="sign-in-bg">
<div class="app-wrapper d-block">
    <div class="main-container">
        <!-- Create Password start -->
        <div class="container">
            <div class="row main-content-box">
                <div class="col-lg-7 image-content-box d-none d-lg-block">
                    <div class="form-container">
                        <div class="signup-content mt-4">
                    <span>
                      <img alt="" class="img-fluid " src="../assets/images/logo/1.png">
                    </span>
                        </div>
                        <div class="signup-bg-img">
                            <img alt="" class="img-fluid" src="../assets/images/login/05.png">
                        </div>
                    </div>
                </div>
                <div class="col-lg-5 form-content-box">
                    <div class="form-container">
                        <form class="app-form">
                            <div class="row">
                                <div class="col-12">
                                    <div class="mb-5 text-center text-lg-start">
                                        <h2 class="text-white f-w-600">Create <span class="text-dark">Password</span></h2>
                                        <p>Your new password must be different from previous used password</p>
                                    </div>
                                </div>

                                <div class="col-lg-6">
                                    <div class=" form-floating mb-3">
                                        <input class="form-control" id="password1" placeholder="Enter Your Password"
                                               type="password">
                                        <label class="form-label" for="password1">New Password</label>

                                    </div>
                                </div>
                                <div class="col-lg-6">
                                    <div class="form-floating mb-3">
                                        <input class="form-control" id="password2" placeholder="Enter Your Password"
                                               type="password">
                                        <label class="form-label" for="password2">Confirm Password</label>

                                    </div>
                                </div>
                                <div class="col-12 mt-3">
                                    <a class="btn btn-primary w-100 btn-lg"  href="/ki-admin/sign_in" role="button">Create
                                        Password</a>
                                </div>
                            </div>
                        </form>
                    </div>
                </div>
            </div>
        </div>
        <!-- Create Password end -->

    </div>
</div>


    <!-- latest jquery-->
    <script src="{{asset('assets/js/jquery-3.6.3.min.js')}}"></script>

    <!-- Bootstrap js-->
    <script src="{{asset('assets/vendor/bootstrap/bootstrap.bundle.min.js')}}"></script>


</body>
</html>
