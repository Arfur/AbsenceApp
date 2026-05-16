<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

/**
 * =========================================================
 * Project   : Support Hub - v1.0.0
 * File Name : 2025_07_08_002168_create_notification_channel_logs_table.php
 * 
 * Author    : Michael Battle
 * Created On: 08/July/2025
 * Updated On: 08/July/2025
 * 
 * Description:
 * Tracks delivery attempts and outcomes for notifications 
 * across channels. Supports diagnostics, retries, and 
 * performance monitoring.
 * 
 * Origin:
 * Notifications group. Used in delivery orchestration, 
 * error tracking, and audit dashboards.
 * 
 * Changes:
 * - Implemented agreed field structure
 * - Indexed key fields for performance
 * - Included timestamps and retry metadata
 * =========================================================
 */

return new class extends Migration
{
    /**
     * =========================================================
     * Section: Create Notification Channel Logs Table
     * Description: Tracks delivery attempts and outcomes for notifications across channels
     * =========================================================
     */
    public function up(): void
    {
        Schema::create('notification_channel_logs', function (Blueprint $table) {
            $table->id();

            $table->foreignId('user_id')->constrained()->onDelete('cascade');
            $table->foreignId('notification_id')->constrained('user_notifications')->onDelete('cascade');
            $table->foreignId('channel_id')->constrained('template_channels')->onDelete('cascade');

            $table->string('status')->default('queued');       // Delivery status
            $table->integer('attempt_count')->default(0);      // Retry count
            $table->string('response_code')->nullable();       // Provider response code
            $table->text('response_message')->nullable();      // Provider response message

            $table->datetime('sent_at')->nullable();           // Timestamp of success
            $table->datetime('error_at')->nullable();          // Timestamp of failure
            $table->datetime('retry_at')->nullable();          // Next retry time

            $table->timestamps();

            $table->index(['user_id', 'channel_id', 'status']);
        });
    }

    /**
     * =========================================================
     * Section: Drop Notification Channel Logs Table
     * Description: Reverts delivery tracking structure
     * =========================================================
     */
    public function down(): void
    {
        Schema::dropIfExists('notification_channel_logs');
    }
};
