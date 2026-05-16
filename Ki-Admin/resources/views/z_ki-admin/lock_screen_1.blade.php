<!DOCTYPE html>
<html lang="en">
@section('title', 'Lock Screen bg')
@include('layout.head')

@include('layout.css')

<body>
<div class="app-wrapper d-block">
    <div class="">
        <!-- Body main section starts -->
        <main class="w-100 p-0">
            <!-- lock_screen start -->
            <div class="container-fluid">
                <div class="row">
                    <div class="col-12 p-0">
                        <div class="login-form-container">
                            <div class="mb-4">
                                <a class="logo"  href="/ki-admin/index">
                                    <img alt="#" src="../assets/images/logo/3.png">
                                </a>
                            </div>
                            <div class="form_container">
                                <form class="app-form p-3">
                                    <div class="row">
                                        <div class="col-12">
                                            <div class="mb-3 text-center">
                                                <h3>Lock Screen</h3>
                                                <p>Hello enter your password to unlock the screen</p>
                                            </div>
                                        </div>
                                        <div class="col-12">
                                            <div class="user-container mb-3">
                                                <div
                                                    class="h-80 w-80 d-flex-center b-r-16 overflow-hidden text-bg-primary">
                                                    <img alt="" class="img-fluid"
                                                         src="../assets/images/avatar/6.png">
                                                </div>
                                                <h5 class="f-w-600">Sunny Airey</h5>
                                                <p class="text-secondary">Enter Your Password to View your Screen
                                                </p>
                                            </div>
                                        </div>
                                        <div class="col-12">
                                            <div class="mb-3">
                                                <label class="form-label" for="password">Password</label>
                                                <input class="form-control" id="password"
                                                       placeholder="Enter Your Password" required="" type="password">
                                            </div>
                                        </div>
                                        <div class="col-12">
                                            <div class="form-check mb-3">
                                                <input class="form-check-input" id="invalidCheck" required=""
                                                       type="checkbox" value="">
                                                <label class="form-check-label" for="invalidCheck">
                                                    Remember me
                                                </label>
                                            </div>
                                        </div>
                                        <div class="col-12">
                                            <div class="mb-3">
                                                <a class="btn btn-primary w-100"  href="/ki-admin/index"
                                                   role="button">Unlock</a>
                                            </div>
                                        </div>
                                    </div>
                                </form>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <!-- lock_screen end -->
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

