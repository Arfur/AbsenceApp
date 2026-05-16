<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

/**
 * =========================================================
 * Project   : Support Hub - v1.0.0
 * File Name : 2025_07_08_002165_create_template_audit_logs_table.php
 * 
 * Author    : Michael Battle
 * Created On: 08/July/2025
 * Updated On: 08/July/2025
 * 
 * Description:
 * Tracks changes made to notification templates. Supports 
 * audit trail, rollback visibility, and admin filtering.
 * 
 * Origin:
 * Notifications group. Used in compliance dashboards, 
 * template history, and change tracking.
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
     * Section: Create Template Audit Logs Table
     * Description: Tracks edits and actions on notification templates with audit metadata
     * =========================================================
     */
    public function up(): void
    {
        Schema::create('template_audit_logs', function (Blueprint $table) {
            $table->id();

            $table->foreignId('template_id')->constrained('notification_templates')->onDelete('cascade');
            $table->foreignId('user_id')->nullable()->constrained()->onDelete('set null');

            $table->string('action_type');                  // created, updated, deleted, etc.
            $table->text('change_summary')->nullable();     // Description of change
            $table->ipAddress('ip_address')->nullable();    // Source IP
            $table->text('user_agent')->nullable();         // Browser/device info
            $table->string('severity_level')->default('info'); // info, warning, critical

            $table->timestamps();

            $table->index(['template_id', 'action_type', 'severity_level']);
        });
    }

    /**
     * =========================================================
     * Section: Drop Template Audit Logs Table
     * Description: Reverts template audit tracking structure
     * =========================================================
     */
    public function down(): void
    {
        Schema::dropIfExists('template_audit_logs');
    }
};
