<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

/**
 * =========================================================
 * Project   : Support Hub - v1.0.0
 * File Name : 2025_07_08_002171_create_notification_queue_audit_logs_table.php
 * 
 * Author    : Michael Battle
 * Created On: 08/July/2025
 * Updated On: 08/July/2025
 * 
 * Description:
 * Tracks changes and interventions on queued notification jobs. 
 * Supports rollback visibility, diagnostics, and compliance reporting.
 * 
 * Origin:
 * Notifications group. Used in queue monitoring, audit dashboards, 
 * and operational transparency.
 * 
 * Changes:
 * - Implemented agreed field structure
 * - Indexed key fields for performance
 * - Included timestamps and audit metadata
 * =========================================================
 */

return new class extends Migration
{
    /**
     * =========================================================
     * Section: Create Notification Queue Audit Logs Table
     * Description: Tracks edits and actions on queued jobs with audit metadata
     * =========================================================
     */
    public function up(): void
    {
        Schema::create('notification_queue_audit_logs', function (Blueprint $table) {
            $table->id();

            $table->foreignId('queue_id')->constrained('notification_queue')->onDelete('cascade');
            $table->foreignId('user_id')->nullable()->constrained()->onDelete('set null');

            $table->string('action_type');                  // created, updated, cancelled, etc.
            $table->text('change_summary')->nullable();     // Description of change
            $table->ipAddress('ip_address')->nullable();    // Source IP
            $table->text('user_agent')->nullable();         // Browser/device info
            $table->string('severity_level')->default('info'); // info, warning, critical

            $table->timestamps();

            $table->index(['queue_id', 'action_type', 'severity_level'], 'queue_action_severity_idx');
        });
    }

    /**
     * =========================================================
     * Section: Drop Notification Queue Audit Logs Table
     * Description: Reverts audit tracking structure for queued jobs
     * =========================================================
     */
    public function down(): void
    {
        Schema::dropIfExists('notification_queue_audit_logs');
    }
};
