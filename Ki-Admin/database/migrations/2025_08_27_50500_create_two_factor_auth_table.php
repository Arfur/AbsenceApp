<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

/**
 * =========================================================
 * Project   : Support Hub - v1.0.0
 * File Name : YYYY_MM_DD_HHMMSS_create_two_factor_auth_table.php
 * 
 * Author    : Michael Battle
 * Created On: [Set current date]
 * Updated On: [Set current date]
 * 
 * Description:
 * Manages multi-factor authentication credentials and configuration. 
 * Tracks TOTP secret, verification status, backup codes, and method 
 * of delivery per user.
 * 
 * Origin:
 * Security and credential hardening module.
 * 
 * Changes:
 * - Stores secret key and delivery method
 * - Added recovery codes and verification auditing
 * =========================================================
 */

return new class extends Migration
{
    /**
     * =========================================================
     * Section: Create Two-Factor Auth Table
     * Description: Stores 2FA configuration and history for secure login
     * =========================================================
     */
    public function up(): void
    {
        Schema::create('two_factor_auth', function (Blueprint $table) {
            $table->id();
            $table->foreignId('user_id')->constrained('users')->onDelete('cascade');

            $table->string('secret_key')->nullable(); // TOTP seed or token
            $table->string('method')->default('app'); // app, sms, email, biometric

            $table->boolean('is_enabled')->default(false);
            $table->json('recovery_codes')->nullable();
            $table->timestamp('last_verified_at')->nullable();

            $table->integer('verification_attempts')->default(0);
            $table->timestamp('locked_out_until')->nullable();

            $table->timestamps();

            $table->index(['user_id', 'method', 'is_enabled']);
        });
    }

    /**
     * =========================================================
     * Section: Drop Two-Factor Auth Table
     * Description: Rolls back multi-factor configuration and history
     * =========================================================
     */
    public function down(): void
    {
        Schema::dropIfExists('two_factor_auth');
    }
};
