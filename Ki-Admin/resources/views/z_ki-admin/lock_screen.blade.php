<!DOCTYPE html>
<html lang="en">
@section('title', 'Lock Screen')
@include('layout.head')

@include('layout.css')

<body class="sign-in-bg">
<div class="app-wrapper d-block">
    <!-- Body main section starts -->
    <div class="main-container">
        <!-- lock_screen start -->
        <div class="container">
            <div class="row main-content-box">
                <div class="col-lg-7 image-content-box d-none d-lg-block">
                    <div class="form-container">
                        <div class="signup-content mt-4">
                  <span>
                    <img alt="avatar" class="img-fluid " src="../assets/images/logo/1.png">
                  </span>
                        </div>
                        <div class="signup-bg-img">
                            <img alt="avatar" class="img-fluid" src="../assets/images/login/06.png">
                        </div>
                    </div>
                </div>
                <div class="col-lg-5 form-content-box">
                    <div class="form-container">
                        <form class="app-form">
                            <div class="row">
                                <div class="col-12">
                                    <div class="mb-3 user-screen">
                                        <div class="w-90 h-90 b-r-15 d-flex-center overflow-hidden text-bg-primary">
                                            <img alt="avatar" class="img-fluid" src="../assets/images/avatar/14.png">
                                        </div>
                                    </div>
                                </div>

                                <div class="col-12">
                                    <div class="mb-5 text-center text-lg-start">
                                        <h2 class="text-white f-w-600">Lock <span class="text-dark">Screen</span></h2>
                                        <p>Hello enter your password to unlock the screen</p>
                                    </div>
                                </div>

                                <div class="col-12">
                                    <div class="form-floating mb-3">
                                        <input class="form-control" id="password" placeholder="Enter Your Password"
                                               required=""
                                               type="password">
                                        <label class="form-label" for="password">Password</label>

                                        <p class="text-white f-s-12 mt-2">Enter Your Password to View your Screen</p>
                                    </div>
                                </div>
                                <div class="col-12">
                                    <div class="form-check d-flex align-items-center gap-2 mb-3">
                                        <input class="form-check-input w-25 h-25" id="checkDefault" type="checkbox"
                                               value="">
                                        <label class="form-check-label text-white mt-1 f-s-16 text-dark"
                                               for="checkDefault">
                                            Remember me
                                        </label>
                                    </div>
                                </div>
                                <div class="col-12 mt-3">
                                    <a class="btn btn-primary btn-lg w-100" href="/ki-admin/index" role="button">Unlock</a>
                                </div>
                            </div>
                        </form>
                    </div>
                </div>
            </div>
        </div>
        <!-- lock_screen end -->

    </div>
    <!-- Body main section ends -->
</div>

    <!-- latest jquery-->
    <script src="{{asset('assets/js/jquery-3.6.3.min.js')}}"></script>

    <!-- Bootstrap js-->
    <script src="{{asset('assets/vendor/bootstrap/bootstrap.bundle.min.js')}}"></script>


</body>
</html>

