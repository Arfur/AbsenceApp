<?php

namespace App\Mail;

use Illuminate\Bus\Queueable;
use Illuminate\Mail\Mailable;
use Illuminate\Mail\Mailables\Content;
use Illuminate\Mail\Mailables\Envelope;
use Illuminate\Queue\SerializesModels;

class OtpVerificationMail extends Mailable
{
    use Queueable, SerializesModels;

    public string $username;
    public string $formattedCode;
    public string $otpCode;

    public function __construct(string $username, string $otpCode)
    {
        $this->username = $username;
        $this->otpCode = $otpCode;
        $this->formattedCode = implode(' ', str_split($otpCode));
    }

    public function envelope(): Envelope
    {
        return new Envelope(
            subject: 'Your Support Hub Verification Code',
        );
    }

    public function content(): Content
    {
        return new Content(
            view: 'emails.otp-verification',
        );
    }

    public function attachments(): array
    {
        return [];
    }
}
