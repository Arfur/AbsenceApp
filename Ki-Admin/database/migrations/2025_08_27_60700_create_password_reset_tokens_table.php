<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

/**
 * =========================================================
 * Project   : Support Hub - v1.0.0
 * File Name : YYYY_MM_DD_HHMMSS_create_password_reset_tokens_table.php
 * 
 * Author    : Michael Battle
 * Created On: [Set current date]
 * Updated On: [Set current date]
 * 
 * Description:
 * Stores secure password reset tokens linked to users. 
 * Tracks reset flow metadata and token lifecycle for 
 * validation and abuse prevention.
 * 
 * Origin:
 * Authentication module — handles credential recovery securely.
 * 
 * Changes:
 * - Added secure token and expiry tracking
 * - Linked to users via FK for traceability
 * - Included optional metadata for auditing
 * =========================================================
 */

return new class extends Migration
{
    /**
     * =========================================================
     * Section: Create Password Reset Tokens Table
     * Description: Supports credential recovery and token lifecycle tracking
     * =========================================================
     */
    public function up(): void
    {
        Schema::create('password_reset_tokens', function (Blueprint $table) {
            $table->id();
            $table->foreignId('user_id')->constrained('users')->onDelete('cascade');

            $table->string('email')->nullable(); // fallback field
            $table->string('token')->unique();
            $table->timestamp('expires_at')->nullable();
            $table->timestamp('used_at')->nullable();

            $table->string('ip_address')->nullable();
            $table->text('user_agent')->nullable();
            $table->string('status')->default('pending');

            $table->timestamps();

            $table->index(['user_id', 'email', 'status']);
        });
    }

    /**
     * =========================================================
     * Section: Drop Password Reset Tokens Table
     * Description: Removes secure token tracking for password recovery
     * =========================================================
     */
    public function down(): void
    {
        Schema::dropIfExists('password_reset_tokens');
    }
};
