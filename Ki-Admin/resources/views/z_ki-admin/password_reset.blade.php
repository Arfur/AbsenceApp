<!DOCTYPE html>
<html lang="en">
@section('title', 'Password Reset')
@include('layout.head')

@include('layout.css')

<body class="sign-in-bg">
<div class="app-wrapper d-block">
    <div class="main-container">
        <!-- Reset Your Password start -->
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
                            <img alt="" class="img-fluid" src="../assets/images/login/03.png">
                        </div>
                    </div>
                </div>
                <div class="col-lg-5 form-content-box">
                    <div class="form-container">
                        <form class="app-form">
                            <div class="row">
                                <div class="col-12">
                                    <div class="mb-5 text-center text-lg-start">
                                        <h2 class="text-white f-w-600">Reset Your <span class="text-dark">Password</span></h2>
                                        <p>Create a new password and sign in to admin</p>
                                    </div>
                                </div>

                                <div class="col-12">
                                    <div class="form-floating mb-3">
                                        <input class="form-control" id="password" placeholder="Enter Your Password"
                                               type="password">
                                        <label class="form-label" for="password">current password</label>

                                    </div>
                                </div>

                                <div class="col-lg-6">
                                    <div class="form-floating mb-3">
                                        <input class="form-control" id="password01" placeholder="Enter Your Password"
                                               type="password">
                                        <label class="form-label" for="password01">New Password</label>

                                    </div>
                                </div>
                                <div class="col-lg-6">
                                    <div class="form-floating mb-3">
                                        <input class="form-control" id="password02" placeholder="Enter Your Password"
                                               required="" type="password">
                                        <label class="form-label" for="password02">Confirm Password</label>

                                    </div>
                                </div>
                                <div class="col-12 mt-3">
                                    <a class="btn btn-primary btn-lg w-100"  href="/ki-admin/sign_in" role="button">Reset
                                        Password</a>
                                </div>
                            </div>
                        </form>
                    </div>
                </div>
            </div>
        </div>
        <!-- Reset Your Password end -->
    </div>
</div>

    <!-- latest jquery-->
    <script src="{{asset('assets/js/jquery-3.6.3.min.js')}}"></script>

    <!-- Bootstrap js-->
    <script src="{{asset('assets/vendor/bootstrap/bootstrap.bundle.min.js')}}"></script>


</body>
</html>



