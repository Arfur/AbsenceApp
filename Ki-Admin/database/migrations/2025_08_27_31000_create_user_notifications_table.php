<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

/**
 * =========================================================
 * Project   : Support Hub - v1.0.0
 * File Name : 2025_07_08_002161_create_user_notifications_table.php
 * 
 * Author    : Michael Battle
 * Created On: 08/July/2025
 * Updated On: 08/July/2025
 * 
 * Description:
 * Stores user-scoped notifications with delivery metadata, 
 * read tracking, and UI personalization flags.
 * 
 * Origin:
 * Notifications group. Used in alerts, reminders, and 
 * transactional messaging across modules.
 * 
 * Changes:
 * - Merged agreed fields with delivery and UI enhancements
 * - Indexed key fields for performance
 * - Included timestamps and audit metadata
 * =========================================================
 */

return new class extends Migration
{
    /**
     * =========================================================
     * Section: Create User Notifications Table
     * Description: Stores alerts and messages for users with delivery, read, and UI metadata
     * =========================================================
     */
    public function up(): void
    {
        Schema::create('user_notifications', function (Blueprint $table) {
            $table->id();

            $table->foreignId('user_id')->constrained()->onDelete('cascade');

            $table->string('channel')->default('database'); // Delivery method
            $table->string('type')->default('info');        // Category
            $table->string('title');                        // Subject
            $table->text('body');                           // Message content

            $table->boolean('is_read')->default(false);     // Read/unread toggle
            $table->boolean('is_pinned')->default(false);   // UI priority flag
            $table->string('action_link')->nullable();      // CTA URL or route

            $table->datetime('scheduled_at')->nullable();   // When to send
            $table->datetime('sent_at')->nullable();        // When sent
            $table->datetime('read_at')->nullable();        // When read

            $table->timestamps();

            $table->index(['user_id', 'type', 'channel', 'is_read']);
        });
    }

    /**
     * =========================================================
     * Section: Drop User Notifications Table
     * Description: Reverts user notification storage structure
     * =========================================================
     */
    public function down(): void
    {
        Schema::dropIfExists('user_notifications');
    }
};
