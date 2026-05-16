<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

/**
 * =========================================================
 * Project   : Support Hub - v1.0.0
 * File Name : 2025_07_08_002162_create_notification_templates_table.php
 * 
 * Author    : Michael Battle
 * Created On: 08/July/2025
 * Updated On: 08/July/2025
 * 
 * Description:
 * Stores reusable notification templates for alerts, 
 * reminders, and transactional messages. Supports 
 * dynamic rendering and multi-channel delivery.
 * 
 * Origin:
 * Notifications group. Used in user alerts, system 
 * messaging, and queue-based delivery.
 * 
 * Changes:
 * - Implemented agreed field structure
 * - Indexed key fields for performance
 * - Included timestamps and activation toggle
 * =========================================================
 */

return new class extends Migration
{
    /**
     * =========================================================
     * Section: Create Notification Templates Table
     * Description: Stores reusable message templates with delivery metadata
     * =========================================================
     */
    public function up(): void
    {
        Schema::create('notification_templates', function (Blueprint $table) {
            $table->id();

            $table->string('name')->unique();             // Internal slug
            $table->string('title');                      // Default subject
            $table->text('body');                         // Default message
            $table->string('type')->default('info');      // Category
            $table->string('module')->nullable();         // Origin module
            $table->string('channel')->default('database'); // Delivery method
            $table->boolean('is_active')->default(true);  // Enable/disable

            $table->timestamps();

            $table->index(['type', 'channel', 'module']);
        });
    }

    /**
     * =========================================================
     * Section: Drop Notification Templates Table
     * Description: Reverts template storage structure
     * =========================================================
     */
    public function down(): void
    {
        Schema::dropIfExists('notification_templates');
    }
};
