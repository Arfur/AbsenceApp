<!DOCTYPE html>
<html lang="en">
@section('title', 'Email Verification')
@include('layout.head')

@include('layout.css')

<!-- Custom OTP input styling -->
<style>
.otp-input {
    height: 80px !important;
    width: 80px !important;
    font-size: 48px !important;
    font-weight: bold !important;
    text-align: center !important;
    border: 2px solid #ddd !important;
    border-radius: 8px !important;
    margin: 0 5px !important;
    background-color: white !important;
    color: #333 !important;
    box-sizing: border-box !important;
}

.otp-input:focus {
    border-color: #007bff !important;
    box-shadow: 0 0 0 0.2rem rgba(0, 123, 255, 0.25) !important;
    outline: none !important;
}

.verification-box {
    display: flex !important;
    justify-content: center !important;
    align-items: center !important;
    gap: 10px !important;
}

/* Override any bootstrap form-control styles */
input.form-control.otp-input {
    height: 80px !important;
    width: 80px !important;
    font-size: 48px !important;
    padding: 0 !important;
    line-height: 1 !important;
    min-height: 80px !important;
}

/* Make verify button width match the OTP inputs */
.verify-button {
    width: 540px !important; /* 6 boxes (80px) + 5 gaps (10px) = 480px + 60px padding */
    max-width: 100% !important;
}
</style>

<body class="sign-in-bg">
<div class="app-wrapper d-block">
    <div class="main-container">
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
                            
                            <!-- Display success/error messages -->
                            @if (session('success'))
                                <div class="alert alert-success alert-dismissible fade show" role="alert">
                                    {{ session('success') }}
                                    <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
                                </div>
                            @endif

                            @if (session('error'))
                                <div class="alert alert-danger alert-dismissible fade show" role="alert">
                                    {{ session('error') }}
                                    <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
                                </div>
                            @endif

                            @if ($errors->any())
                                <div class="alert alert-danger alert-dismissible fade show" role="alert">
                                    {{ $errors->first() }}
                                    <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
                                </div>
                            @endif

                            <form class="app-form" method="POST" action="{{ route('otp.verification.verify') }}">
                                @csrf
                                <div class="row">
                                    <div class="col-12">
                                        <div class="mb-5 text-center text-lg-start">
                                            <h2 class="text-white">Verify <span class="text-dark">Email</span></h2>
                                            <p>Enter the 6-digit verification code sent to:<br>
                                               <strong>{{ $email ?? 'your email address' }}</strong></p>
                                            @if(isset($otp_expires_at))
                                                <p class="text-warning small">Code expires at: {{ $otp_expires_at->format('H:i:s') }}</p>
                                            @endif
                                        </div>
                                    </div>
                                    <div class="col-12">
                                        <div class="verification-box justify-content-lg-start mb-3">
                                            <div>
                                                <input class="form-control otp-input" id="one"
                                                       name="digit1" maxlength="1" required type="text">
                                            </div>
                                            <div>
                                                <input class="form-control otp-input" id="two"
                                                       name="digit2" maxlength="1" required type="text">
                                            </div>
                                            <div>
                                                <input class="form-control otp-input" id="three"
                                                       name="digit3" maxlength="1" required type="text">
                                            </div>
                                            <div>
                                                <input class="form-control otp-input" id="four"
                                                       name="digit4" maxlength="1" required type="text">
                                            </div>
                                            <div>
                                                <input class="form-control otp-input" id="five"
                                                       name="digit5" maxlength="1" required type="text">
                                            </div>
                                            <div>
                                                <input class="form-control otp-input" id="six"
                                                       name="digit6" maxlength="1" required type="text">
                                            </div>
                                        </div>
                                        <!-- Hidden field to combine all digits -->
                                        <input type="hidden" name="otp_code" id="otp_code">
                                    </div>
                                    <div class="col-12 mb-3">
                                        <p>
                                            Didn't receive a code? 
                                            <a href="{{ route('otp.verification.resend') }}" 
                                               class="link-white text-decoration-underline"
                                               onclick="event.preventDefault(); document.getElementById('resend-form').submit();">
                                                Resend Code
                                            </a>
                                        </p>
                                    </div>
                                    <div class="col-12">
                                        <div class="mb-3 text-center">
                                            <button class="btn btn-primary btn-lg verify-button" type="submit">Verify Email</button>
                                        </div>
                                    </div>
                                </div>
                            </form>

                            <!-- Resend form -->
                            <form id="resend-form" action="{{ route('otp.verification.resend') }}" method="POST" style="display: none;">
                                @csrf
                            </form>
                        </div>
                    </div>
                </div>
            </div>
            <!-- Verify OTP end -->
        </div>
    </div>
</div>

<!-- latest jquery-->
<script src="{{asset('assets/js/jquery-3.6.3.min.js')}}"></script>

<!-- Bootstrap js-->
<script src="{{asset('assets/vendor/bootstrap/bootstrap.bundle.min.js')}}"></script>

<!-- Custom OTP verification script -->
<script>
// Enhanced OTP input handling
document.addEventListener('DOMContentLoaded', function() {
    const inputs = document.querySelectorAll('.otp-input');
    
    inputs.forEach((input, index) => {
        // Clear any existing event listeners
        input.removeEventListener('input', handleInput);
        input.removeEventListener('keydown', handleKeydown);
        input.removeEventListener('paste', handlePaste);
        
        // Add new event listeners
        input.addEventListener('input', function(e) {
            handleInput(e, index);
        });
        
        input.addEventListener('keydown', function(e) {
            handleKeydown(e, index);
        });
        
        input.addEventListener('paste', function(e) {
            handlePaste(e, index);
        });
    });
    
    // Auto-focus first input
    if (inputs.length > 0) {
        inputs[0].focus();
    }
});

function handleInput(e, index) {
    const input = e.target;
    const inputs = document.querySelectorAll('.otp-input');
    
    // Allow only digits
    input.value = input.value.replace(/[^0-9]/g, '');
    
    // Move to next input if digit entered
    if (input.value.length === 1 && index < inputs.length - 1) {
        inputs[index + 1].focus();
    }
    
    updateOtpCode();
}

function handleKeydown(e, index) {
    const inputs = document.querySelectorAll('.otp-input');
    
    // Handle Backspace
    if (e.key === 'Backspace') {
        if (e.target.value === '' && index > 0) {
            inputs[index - 1].focus();
        }
    }
    
    // Handle Tab navigation
    if (e.key === 'Tab') {
        e.preventDefault();
        if (e.shiftKey) {
            // Shift+Tab - go backward
            if (index > 0) {
                inputs[index - 1].focus();
            }
        } else {
            // Tab - go forward
            if (index < inputs.length - 1) {
                inputs[index + 1].focus();
            }
        }
    }
    
    // Handle Arrow Keys
    if (e.key === 'ArrowLeft' && index > 0) {
        e.preventDefault();
        inputs[index - 1].focus();
    }
    if (e.key === 'ArrowRight' && index < inputs.length - 1) {
        e.preventDefault();
        inputs[index + 1].focus();
    }
}

function handlePaste(e, index) {
    e.preventDefault();
    const paste = (e.clipboardData || window.clipboardData).getData('text');
    const digits = paste.replace(/[^0-9]/g, '').split('');
    const inputs = document.querySelectorAll('.otp-input');
    
    // Fill inputs with pasted digits
    for (let i = 0; i < Math.min(digits.length, inputs.length - index); i++) {
        inputs[index + i].value = digits[i];
    }
    
    // Focus the next empty input or the last filled input
    const nextIndex = Math.min(index + digits.length, inputs.length - 1);
    inputs[nextIndex].focus();
    
    updateOtpCode();
}

function updateOtpCode() {
    const inputs = document.querySelectorAll('.otp-input');
    const digits = Array.from(inputs).map(input => input.value);
    document.getElementById('otp_code').value = digits.join('');
}

// Legacy functions for backward compatibility
function digitValidate(ele) {
    ele.value = ele.value.replace(/[^0-9]/g, '');
}

function tabChange(val) {
    // This function is kept for backward compatibility but the new event listeners handle this
    updateOtpCode();
}
</script>

</body>
</html>
