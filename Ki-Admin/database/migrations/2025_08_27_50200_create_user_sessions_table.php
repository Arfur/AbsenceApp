<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

/**
 * =========================================================
 * Project   : Support Hub - v1.0.0
 * File Name : 2025_07_25_400430_create_user_sessions_table.php
 * 
 * Author    : Michael Battle
 * Created On: 27/July/2025
 * Updated On: 27/July/2025
 * 
 * Description:
 * Tracks login sessions across browsers, devices, IPs, and tokens. 
 * Supports session lifecycle auditing, multi-device control,
 * and enforced logout + expiry workflows.
 * 
 * Origin:
 * Authentication and session lifecycle module.
 * 
 * Changes:
 * - Merged legacy `logged_in_at` and `logged_out_at` timestamps
 * - Added full metadata for session tokens and device identity
 * - Indexing optimized for status and filtering
 * =========================================================
 */

return new class extends Migration
{
    /**
     * =========================================================
     * Section: Create User Sessions Table
     * Description: Records lifecycle and metadata for each user session instance
     * =========================================================
     */
    public function up(): void
    {
        Schema::create('user_sessions', function (Blueprint $table) {
            $table->id(); // Primary key
            $table->foreignId('user_id')->constrained('users')->onDelete('cascade'); // FK → users.id

            $table->string('session_token')->unique(); // Unique session token reference
            $table->string('ip_address')->nullable(); // IP captured at login
            $table->text('user_agent')->nullable(); // Browser/device fingerprint
            $table->string('device_name')->nullable(); // Optional device label
            $table->string('login_method')->nullable(); // Auth method: password, oauth, sso

            $table->boolean('is_current')->default(true); // Flag for active sessions

            // Lifecycle timestamps
            $table->timestamp('session_started_at')->nullable(); // Start time
            $table->timestamp('logged_in_at')->nullable(); // Legacy login timestamp
            $table->timestamp('last_activity_at')->nullable(); // Most recent activity
            $table->timestamp('logged_out_at')->nullable(); // Explicit logout timestamp
            $table->timestamp('session_expires_at')->nullable(); // Scheduled expiry

            $table->string('status')->default('active'); // active, expired, revoked
            $table->text('notes')->nullable(); // Optional notes/comments

            $table->timestamps(); // created_at + updated_at

            $table->index(['user_id', 'status', 'session_token']); // Optimized filters
        });
    }

    /**
     * =========================================================
     * Section: Drop User Sessions Table
     * Description: Removes session history and related metadata
     * =========================================================
     */
    public function down(): void
    {
        Schema::dropIfExists('user_sessions');
    }
};
