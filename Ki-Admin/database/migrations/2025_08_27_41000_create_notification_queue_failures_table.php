<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

/**
 * =========================================================
 * Project   : Support Hub - v1.0.0
 * File Name : 2025_07_08_002172_create_notification_queue_failures_table.php
 * 
 * Author    : Michael Battle
 * Created On: 08/July/2025
 * Updated On: 08/July/2025
 * 
 * Description:
 * Stores failed notification queue jobs with diagnostics, 
 * retry metadata, and resolution tracking.
 * 
 * Origin:
 * Notifications group. Used in queue monitoring, 
 * error dashboards, and delivery recovery.
 * 
 * Changes:
 * - Implemented agreed field structure
 * - Indexed key fields for performance
 * - Included timestamps and resolution flags
 * =========================================================
 */

return new class extends Migration
{
    /**
     * =========================================================
     * Section: Create Notification Queue Failures Table
     * Description: Tracks failed queue jobs with diagnostics and resolution metadata
     * =========================================================
     */
    public function up(): void
    {
        Schema::create('notification_queue_failures', function (Blueprint $table) {
            $table->id();

            $table->foreignId('queue_id')->constrained('notification_queue')->onDelete('cascade');
            $table->foreignId('user_id')->constrained()->onDelete('cascade');
            $table->foreignId('channel_id')->constrained('template_channels')->onDelete('cascade');
            $table->foreignId('notification_id')->constrained('user_notifications')->onDelete('cascade');

            $table->text('failure_reason')->nullable();         // Explanation of failure
            $table->longText('exception_trace')->nullable();    // Stack trace or dump
            $table->datetime('failed_at')->nullable();          // Timestamp of failure
            $table->datetime('retry_scheduled_at')->nullable(); // Planned retry time
            $table->boolean('is_resolved')->default(false);     // Resolution flag
            $table->datetime('resolved_at')->nullable();        // When resolved

            $table->timestamps();

            $table->index(['user_id', 'channel_id', 'is_resolved']);
        });
    }

    /**
     * =========================================================
     * Section: Drop Notification Queue Failures Table
     * Description: Reverts failure tracking structure
     * =========================================================
     */
    public function down(): void
    {
        Schema::dropIfExists('notification_queue_failures');
    }
};
