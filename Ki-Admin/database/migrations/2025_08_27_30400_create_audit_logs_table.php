<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

/**
 * =========================================================
 * Project   : Support Hub - v1.0.0
 * File Name : 2025_07_08_002155_create_audit_logs_table.php
 * 
 * Author    : Michael Battle
 * Created On: 08/July/2025
 * Updated On: 08/July/2025
 * 
 * Description:
 * Tracks user actions, system events, and changes across 
 * the application for audit and compliance purposes.
 * 
 * Origin:
 * Audit & Logging group. Used in dashboards, reports, 
 * and access reviews.
 * 
 * Changes:
 * - Implemented agreed field structure
 * - Included metadata for traceability
 * - Indexed key fields for performance
 * =========================================================
 */

return new class extends Migration
{
    public function up(): void
    {
        Schema::create('audit_logs', function (Blueprint $table) {
            $table->id();

            $table->foreignId('user_id')->nullable()->constrained()->onDelete('set null');
            $table->string('event_type');               // Action type
            $table->string('event_context');            // Area affected
            $table->unsignedBigInteger('target_id')->nullable(); // Entity ID
            $table->string('target_description')->nullable();    // Human-readable summary
            $table->text('change_summary')->nullable(); // What changed

            $table->ipAddress('ip_address')->nullable();
            $table->text('user_agent')->nullable();
            $table->string('severity_level')->default('info'); // info, warning, critical

            $table->timestamps();

            $table->index(['event_type', 'event_context']);
        });
    }

    public function down(): void
    {
        Schema::dropIfExists('audit_logs');
    }
};
