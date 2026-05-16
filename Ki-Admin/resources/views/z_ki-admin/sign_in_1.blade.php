<!DOCTYPE html>
<html lang="en">
@section('title', 'Sign In Bg')
@include('layout.head')

@include('layout.css')

<body>
<div class="app-wrapper d-block">
    <div class="">
        <!-- Body main section starts -->
        <main class="w-100 p-0">
            <!-- Login to your Account start -->
            <div class="container-fluid">
                <div class="row">

                    <div class="col-12 p-0">
                        <div class="login-form-container">
                            <div class="mb-4">
                                <a class="logo" href="/ki-admin/index">
                                    <img alt="#" src="../assets/images/logo/3.png" class="">
                                </a>
                            </div>
                            <div class="form_container">

                                <form class="app-form">
                                    <div class="mb-3 text-center">
                                        <h3>Login to your Account</h3>
                                        <p class="f-s-12 text-secondary">Get started with our app, just create an
                                            account and enjoy the experience.</p>
                                    </div>
                                    <div class="mb-3">
                                        <label class="form-label" for="emailId">Email address</label>
                                        <input class="form-control" type="email" id="emailId">
                                        <div class="form-text text">We'll never share your email with anyone else.</div>
                                    </div>
                                    <div class="mb-3">
                                        <label class="form-label" for="password">Password</label>
                                        <input class="form-control" type="password" id="password">
                                    </div>
                                    <div class="mb-3 form-check">
                                        <input class="form-check-input" id="formCheck1" type="checkbox">
                                        <label class="form-check-label" for="formCheck1">remember me</label>
                                    </div>
                                    <div>
                                        <a class="btn btn-primary w-100" href="/ki-admin/index" role="button">Continue</a>
                                    </div>
                                    <div class="app-divider-v justify-content-center">
                                        <p>OR</p>
                                    </div>
                                    <div class="mb-3">
                                        <div class="text-center">
                                            <button class="btn btn-primary icon-btn b-r-5 m-1" type="button"><i
                                                    class="ti ti-brand-facebook text-white"></i></button>
                                            <button class="btn btn-danger icon-btn b-r-5 m-1" type="button"><i
                                                    class="ti ti-brand-google text-white"></i></button>
                                            <button class="btn btn-dark icon-btn b-r-5 m-1" type="button"><i
                                                    class="ti ti-brand-github text-white"></i></button>
                                        </div>
                                    </div>
                                    <div class="text-center">
                                        <a class="text-secondary text-decoration-underline"
                                           href="/ki-admin/terms_condition">Terms of use &amp;
                                            Conditions</a>
                                    </div>
                                </form>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <!-- Login to your Account end -->
        </main>
        <!-- Body main section ends -->
    </div>
</div>


    <!-- Bootstrap js-->
    <script src="{{asset('assets/vendor/bootstrap/bootstrap.bundle.min.js')}}"></script>


</body>
</html>

