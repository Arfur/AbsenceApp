<!DOCTYPE html>
<html lang="en">
@section('title', 'Two Step Verifications')
@include('layout.head')

@include('layout.css')

<body class="sign-in-bg">
<div class="app-wrapper d-block">
    <!-- <div class="app-content"> -->
    <div class="main-container">
        <!-- Body main section starts -->

        <div class="container">
            <!-- Verify OTP start -->
            <div class="sign-in-content-bg">
                <div class="row main-content-box">
                    <div class="col-lg-7 image-content-box d-none d-lg-block">
                        <div class="form-container">
                            <div class="signup-content mt-4">
                  <span>
                    <img alt="" class="img-fluid " src="../assets/images/logo/1.png">
                  </span>
                            </div>
                            <div class="signup-bg-img">
                                <img alt="" class="img-fluid" src="../assets/images/login/04.png">
                            </div>
                        </div>
                    </div>
                    <div class="col-lg-5 form-content-box">
                        <div class="form-container">
                            <form class="app-form">
                                <div class="row">
                                    <div class="col-12">
                                        <div class="mb-5 text-center text-lg-start">
                                            <h2 class="text-white">Verify <span class="text-dark">OTP</span></h2>
                                            <p>Enter the 5 digit code sent to the registered email Id</p>
                                        </div>
                                    </div>
                                    <div class="col-12">
                                        <div class="verification-box justify-content-lg-start mb-3">
                                            <div>
                                                <input class="form-control h-60 w-60 text-center" id="one"
                                                       maxlength="1"
                                                       oninput='digitValidate(this)' onkeyup='tabChange(1)' type="text">
                                            </div>
                                            <div>
                                                <input class="form-control h-60 w-60 text-center" id="two"
                                                       maxlength="1"
                                                       oninput='digitValidate(this)' onkeyup='tabChange(2)' type="text">
                                            </div>
                                            <div>
                                                <input class="form-control h-60 w-60 text-center" id="three"
                                                       maxlength="1"
                                                       oninput='digitValidate(this)' onkeyup='tabChange(3)' type="text">
                                            </div>
                                            <div>
                                                <input class="form-control h-60 w-60 text-center" id="four"
                                                       maxlength="1"
                                                       oninput='digitValidate(this)' onkeyup='tabChange(4)' type="text">
                                            </div>
                                            <div>
                                                <input class="form-control h-60 w-60 text-center" id="five"
                                                       maxlength="1"
                                                       oninput='digitValidate(this)' onkeyup='tabChange(5)' type="text">
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-12 mb-3">
                                        <p>
                                            Did not recieve a code <a class="link-white text-decoration-underline"
                                                                      href="#">
                                                Resend!</a>
                                        </p>
                                    </div>
                                    <div class="col-12">
                                        <div class="mb-3">
                                            <button class="btn btn-primary btn-lg w-100" type="reset">Verify</button>
                                        </div>
                                    </div>
                                </div>
                            </form>
                        </div>
                    </div>
                </div>
            </div>
            <!-- Verify OTP end -->
        </div>

        <!-- Body main section ends -->
    </div>
</div>


    <!-- latest jquery-->
    <script src="{{asset('assets/js/jquery-3.6.3.min.js')}}"></script>

    <!-- Bootstrap js-->
    <script src="{{asset('assets/vendor/bootstrap/bootstrap.bundle.min.js')}}"></script>

    <!-- js -->
    <script src="{{asset('assets/js/two_step.js')}}"></script>


</body>
</html>

