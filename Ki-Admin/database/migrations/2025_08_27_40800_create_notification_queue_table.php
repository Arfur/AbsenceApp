<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

/**
 * =========================================================
 * Project   : Support Hub - v1.0.0
 * File Name : 2025_07_08_002169_create_notification_queue_table.php
 * 
 * Author    : Michael Battle
 * Created On: 08/July/2025
 * Updated On: 08/July/2025
 * 
 * Description:
 * Stores queued notification jobs for delivery orchestration. 
 * Supports retry logic, failure tracking, and channel dispatching.
 * 
 * Origin:
 * Notifications group. Used in queue workers, delivery 
 * scheduling, and diagnostics.
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
     * Section: Create Notification Queue Table
     * Description: Stores queued notification jobs with delivery and error metadata
     * =========================================================
     */
    public function up(): void
    {
        Schema::create('notification_queue', function (Blueprint $table) {
            $table->id();

            $table->foreignId('notification_id')->constrained('user_notifications')->onDelete('cascade');
            $table->foreignId('channel_id')->constrained('template_channels')->onDelete('cascade');
            $table->foreignId('user_id')->constrained()->onDelete('cascade');

            $table->string('status')->default('pending');     // Job status
            $table->integer('attempts')->default(0);          // Retry count

            $table->datetime('scheduled_at')->nullable();     // When scheduled
            $table->datetime('dispatched_at')->nullable();    // When dispatched
            $table->datetime('completed_at')->nullable();     // When completed

            $table->text('error_message')->nullable();        // Failure reason

            $table->timestamps();

            $table->index(['status', 'channel_id', 'user_id']);
        });
    }

    /**
     * =========================================================
     * Section: Drop Notification Queue Table
     * Description: Reverts queued notification job structure
     * =========================================================
     */
    public function down(): void
    {
        Schema::dropIfExists('notification_queue');
    }
};
