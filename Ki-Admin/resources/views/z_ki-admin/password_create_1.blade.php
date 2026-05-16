<!DOCTYPE html>
<html lang="en">
@section('title', 'Password Create Bg')
@include('layout.head')

@include('layout.css')

<body>
<div class="app-wrapper d-block">
    <div class="">
        <!-- Body main section starts -->
        <main class="w-100 p-0">
            <div class="container-fluid">
                <!-- Create Password start -->
                <div class="row">
                    <div class="col-12 p-0">
                        <div class="login-form-container">
                            <div class="mb-4">
                                <a class="logo"  href="/ki-admin/index">
                                    <img alt="#" src="../assets/images/logo/3.png">
                                </a>
                            </div>
                            <div class="form_container">
                                <form class="app-form">
                                    <div class="mb-3 text-center">
                                        <h3>Create Password</h3>
                                        <p class="f-s-12 text-secondary">Your new password must be different from
                                            previous used password</p>
                                    </div>
                                    <div class="mb-3">
                                        <label class="form-label" for="newPassword">New password</label>
                                        <input class="form-control" id="newPassword" placeholder="Enter Your Password" type="password">
                                    </div>
                                    <div class="mb-3">
                                        <label class="form-label" for="cfPassword">Confirm Password</label>
                                        <input class="form-control" id="cfPassword" placeholder="Enter Your Password" type="password">
                                    </div>
                                    <div>
                                        <a class="btn btn-primary w-100"  href="/ki-admin/index" role="button">Reset
                                            Password</a>
                                    </div>
                                </form>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- Create Password end -->
            </div>
        </main>
        <!-- Body main section ends -->
    </div>
</div>

    <!-- latest jquery-->
    <script src="{{asset('assets/js/jquery-3.6.3.min.js')}}"></script>

    <!-- Bootstrap js-->
    <script src="{{asset('assets/vendor/bootstrap/bootstrap.bundle.min.js')}}"></script>



</body>
</html>




