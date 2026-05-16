<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title>Support Hub - Verification Code</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            line-height: 1.6;
            color: #333;
            max-width: 600px;
            margin: 0 auto;
            padding: 20px;
        }
        .container {
            background-color: #f9f9f9;
            padding: 30px;
            border-radius: 10px;
            border: 1px solid #ddd;
        }
        .header {
            text-align: center;
            margin-bottom: 30px;
        }
        .logo {
            font-size: 24px;
            font-weight: bold;
            color: #2563eb;
        }
        .greeting {
            font-size: 18px;
            margin-bottom: 20px;
        }
        .otp-code {
            text-align: center;
            margin: 30px 0;
            padding: 20px;
            background-color: #ffffff;
            border: 2px solid #2563eb;
            border-radius: 8px;
        }
        .otp-label {
            font-size: 16px;
            color: #666;
            margin-bottom: 10px;
        }
        .otp-digits {
            font-size: 36px;
            font-weight: bold;
            color: #2563eb;
            letter-spacing: 8px;
            font-family: 'Courier New', monospace;
        }
        .message {
            font-size: 16px;
            margin: 15px 0;
        }
        .footer {
            margin-top: 30px;
            text-align: center;
            font-size: 16px;
        }
        .signature {
            font-weight: bold;
            color: #2563eb;
        }
    </style>
</head>
<body>
    <div class="container">
        <div class="header">
            <div class="logo">🛠️ Support Hub</div>
        </div>
        
        <div class="greeting">
            Dear {{ $username }},
        </div>
        
        <div class="otp-code">
            <div class="otp-label">Your one-time verification code:</div>
            <div class="otp-digits">{{ $formattedCode }}</div>
        </div>
        
        <div class="message">
            You've got a breezy 15 minutes to use it before it vanishes like a biscuit at breaktime.
        </div>
        
        <div class="message">
            If it expires, no worries—just request a fresh one and we'll whip it up faster than you can say "Support Hub!"
        </div>
        
        <div class="footer">
            <div>Cheers,</div>
            <div class="signature">The Support Hub Team 🛠️</div>
        </div>
    </div>
</body>
</html>
