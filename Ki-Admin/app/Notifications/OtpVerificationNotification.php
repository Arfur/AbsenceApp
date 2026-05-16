<?php
/**
 * =========================================================
 * Project   : ki-admin - v1.0.0
 * File Name : OtpVerificationNotification.php
 * 
 * Author    : Michael Battle
 * Created On: 05/Aug/2025
 * Updated On: 05/Aug/2025
 * 
 * Description:
 * Custom OTP verification notification that sends emails using
 * a custom mailable instead of Laravel's default notification system.
 * 
 * Origin:
 * Custom notification system for ki-admin user registration with OTP codes
 * 
 * Changes:
 * - Uses custom OtpVerificationMail mailable
 * - Bypasses Laravel's default MailMessage system
 * - Sends formatted OTP codes with custom styling
 * =========================================================
 */

namespace App\Notifications;

use App\Mail\OtpVerificationMail;
use Illuminate\Bus\Queueable;
use Illuminate\Notifications\Notification;

class OtpVerificationNotification extends Notification
{
    use Queueable;

    protected string $otpCode;

    /* =========================================================
     * Section: Constructor
     * Description: Initialize notification with OTP code
     * ========================================================= */
    public function __construct(string $otpCode)
    {
        $this->otpCode = $otpCode;
    }

    /* =========================================================
     * Section: Delivery Channels
     * Description: Specify that this notification uses email
     * ========================================================= */
    public function via(object $notifiable): array
    {
        return ['mail'];
    }

    /* =========================================================
     * Section: Mail Generation
     * Description: Returns custom mailable instead of MailMessage
     * ========================================================= */
    public function toMail(object $notifiable): OtpVerificationMail
    {
        return new OtpVerificationMail($notifiable->username, $this->otpCode);
    }
}