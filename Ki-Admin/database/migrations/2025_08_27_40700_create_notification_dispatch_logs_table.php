<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

/**
 * =========================================================
 * Project   : Support Hub - v1.0.0
 * File Name : 2025_07_08_002170_create_notification_dispatch_logs_table.php
 * 
 * Author    : Michael Battle
 * Created On: 08/July/2025
 * Updated On: 08/July/2025
 * 
 * Description:
 * Records final dispatch status of notifications across 
 * channels. Supports delivery analytics, SLA tracking, 
 * and error diagnostics.
 * 
 * Origin:
 * Notifications group. Used in dashboards, compliance 
 * reporting, and performance monitoring.
 * 
 * Changes:
 * - Implemented agreed field structure
 * - Indexed key fields for performance
 * - Included timestamps and error metadata
 * =========================================================
 */

return new class extends Migration
{
    /**
     * =========================================================
     * Section: Create Notification Dispatch Logs Table
     * Description: Records final delivery status and metadata for notifications
     * =========================================================
     */
    public function up(): void
    {
        Schema::create('notification_dispatch_logs', function (Blueprint $table) {
            $table->id();

            $table->foreignId('notification_id')->constrained('user_notifications')->onDelete('cascade');
            $table->foreignId('user_id')->constrained()->onDelete('cascade');
            $table->foreignId('channel_id')->constrained('template_channels')->onDelete('cascade');

            $table->string('status')->default('sent');         // Final delivery status
            $table->datetime('dispatched_at')->nullable();     // When dispatched
            $table->integer('duration_ms')->nullable();        // Time taken in ms
            $table->string('error_code')->nullable();          // Provider error code
            $table->text('error_message')->nullable();         // Provider error message

            $table->timestamps();

            $table->index(['user_id', 'channel_id', 'status']);
        });
    }

    /**
     * =========================================================
     * Section: Drop Notification Dispatch Logs Table
     * Description: Reverts dispatch tracking structure
     * =========================================================
     */
    public function down(): void
    {
        Schema::dropIfExists('notification_dispatch_logs');
    }
};
