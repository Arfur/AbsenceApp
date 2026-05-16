<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

/**
 * =========================================================
 * Project   : Support Hub - v1.0.0
 * File Name : YYYY_MM_DD_HHMMSS_create_login_histories_table.php
 * 
 * Author    : Michael Battle
 * Created On: [Set current date]
 * Updated On: [Set current date]
 * 
 * Description:
 * Tracks user login attempts including timestamps, IP addresses, 
 * device metadata, and success status. Supports audit trail and 
 * security monitoring.
 * 
 * Origin:
 * Authentication module. Enhances visibility and intrusion detection.
 * 
 * Changes:
 * - Linked to `users.id`
 * - Includes timestamp and metadata fields
 * =========================================================
 */

return new class extends Migration
{
    /**
     * =========================================================
     * Section: Create Login Histories Table
     * Description: Logs each user login event with trace metadata
     * =========================================================
     */
    public function up(): void
    {
        Schema::create('login_histories', function (Blueprint $table) {
            $table->id();
            $table->foreignId('user_id')->constrained('users')->onDelete('cascade');

            $table->timestamp('logged_in_at')->nullable();
            $table->string('ip_address')->nullable();
            $table->string('user_agent')->nullable();
            $table->boolean('was_successful')->default(true);
            $table->string('location')->nullable(); // Optional: Geo data if available

            $table->timestamps();

            $table->index(['user_id', 'was_successful', 'logged_in_at']);
        });
    }

    /**
     * =========================================================
     * Section: Drop Login Histories Table
     * Description: Rolls back login tracking functionality
     * =========================================================
     */
    public function down(): void
    {
        Schema::dropIfExists('login_histories');
    }
};
