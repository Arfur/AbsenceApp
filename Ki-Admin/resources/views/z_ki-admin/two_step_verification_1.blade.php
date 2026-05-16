<!DOCTYPE html>
<html lang="en">
@section('title', 'Two Step Verification Bg')
@include('layout.head')

@include('layout.css')

<body>
<div class="app-wrapper d-block">
    <div class="">
        <!-- Body main section starts -->
        <main class="w-100 p-0">
            <div class="container-fluid">
                <div class="row">
                    <!-- Verify OTP 1 start -->
                    <div class="col-12 p-0 ">
                        <div class="login-form-container">
                            <div class="mb-4">
                                <a class="logo"  href="/ki-admin/index">
                                    <img alt="#" src="../assets/images/logo/3.png">
                                </a>
                            </div>
                            <div class="form_container">
                                <form class="app-form">
                                    <div class="row">
                                        <div class="col-12">
                                            <div class="mb-5 text-center">
                                                <h2 class="text-primary">Verify OTP</h2>
                                                <p>Enter the 5 digit code sent to the registered email Id</p>
                                            </div>
                                        </div>
                                        <div class="col-12">
                                            <div class="verification-box">
                                                <div>
                                                    <input class="form-control h-60 w-60 text-center" id="one"
                                                           maxlength="1"
                                                           oninput='digitValidate(this)' onkeyup='tabChange(1)'
                                                           type="text">
                                                </div>
                                                <div>
                                                    <input class="form-control h-60 w-60 text-center" id="two"
                                                           maxlength="1"
                                                           oninput='digitValidate(this)' onkeyup='tabChange(2)'
                                                           type="text">
                                                </div>
                                                <div>
                                                    <input class="form-control h-60 w-60 text-center" id="three"
                                                           maxlength="1"
                                                           oninput='digitValidate(this)' onkeyup='tabChange(3)'
                                                           type="text">
                                                </div>
                                                <div>
                                                    <input class="form-control h-60 w-60 text-center" id="four"
                                                           maxlength="1"
                                                           oninput='digitValidate(this)' onkeyup='tabChange(4)'
                                                           type="text">
                                                </div>
                                                <div>
                                                    <input class="form-control h-60 w-60 text-center" id="five"
                                                           maxlength="1"
                                                           oninput='digitValidate(this)' onkeyup='tabChange(5)'
                                                           type="text">
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-12">
                                            <p>
                                                Did not recieve a code <a class="link-primary text-decoration-underline"
                                                                          href="#">
                                                    Resend!</a>
                                            </p>
                                        </div>
                                        <div class="col-12">
                                            <div class="mb-3">
                                                <button class="btn btn-primary w-100" type="reset">Verify</button>
                                            </div>
                                        </div>
                                    </div>
                                </form>
                            </div>
                        </div>
                    </div>
                    <!-- Verify OTP 1 end -->
                </div>
            </div>
        </main>
        <!-- Body main section ends -->
    </div>
</div>
<!-- latest jquery-->
<script src="{{asset('assets/js/jquery-3.6.3.min.js')}}"></script>

<!-- Bootstrap js-->
<script src="{{asset('assets/vendor/bootstrap/bootstrap.bundle.min.js')}}"></script>
<script src="{{asset('assets/js/two_step.js')}}"></script>

</body>
</html>

