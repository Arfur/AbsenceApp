<!DOCTYPE html>
<html>
<head>
    <title>OTP Test</title>
    <style>
        .otp-input {
            height: 80px !important;
            width: 80px !important;
            font-size: 32px !important;
            font-weight: bold !important;
            text-align: center !important;
            border: 2px solid #ddd !important;
            border-radius: 8px !important;
            margin: 0 5px !important;
        }
        .verification-box {
            display: flex !important;
            justify-content: center !important;
            gap: 10px !important;
        }
    </style>
</head>
<body>
    <div class="verification-box">
        <input class="otp-input" id="one" maxlength="1" type="text">
        <input class="otp-input" id="two" maxlength="1" type="text">
        <input class="otp-input" id="three" maxlength="1" type="text">
        <input class="otp-input" id="four" maxlength="1" type="text">
        <input class="otp-input" id="five" maxlength="1" type="text">
    </div>
    
    <script>
        const inputs = ['one', 'two', 'three', 'four', 'five'];
        
        inputs.forEach((id, index) => {
            const element = document.getElementById(id);
            
            element.addEventListener('input', function() {
                this.value = this.value.replace(/[^0-9]/g, '');
                if (this.value.length === 1 && index < 4) {
                    document.getElementById(inputs[index + 1]).focus();
                }
            });
            
            element.addEventListener('keydown', function(e) {
                if (e.key === 'Backspace' && this.value === '' && index > 0) {
                    document.getElementById(inputs[index - 1]).focus();
                }
                if (e.key === 'Tab' && !e.shiftKey && index < 4) {
                    e.preventDefault();
                    document.getElementById(inputs[index + 1]).focus();
                }
            });
        });
        
        document.getElementById('one').focus();
    </script>
</body>
</html>
