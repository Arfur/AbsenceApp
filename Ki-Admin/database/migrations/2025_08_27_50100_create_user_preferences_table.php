<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

/**
 * =========================================================
 * Project   : Support Hub - v1.0.0
 * File Name : 2025_07_25_400330_create_user_preferences_table.php
 * 
 * Author    : Michael Battle
 * Created On: 27/July/2025
 * Updated On: 27/July/2025
 * 
 * Description:
 * Consolidated user preference storage for UI, notification,
 * accessibility, and legacy key-value fallback.
 * 
 * Origin:
 * UX personalization module. Used for layout selection,
 * digest timing, screen reader modes, and opt-in flags.
 * 
 * Changes:
 * - Merged initial preference model with legacy key/value support
 * - Added compound index for fast preference lookup
 * =========================================================
 */

return new class extends Migration
{
    /**
     * =========================================================
     * Section: Create User Preferences Table
     * Description: Stores layout, accessibility, and notification preferences per user
     * =========================================================
     */
    public function up(): void
    {
        Schema::create('user_preferences', function (Blueprint $table) {
            $table->id(); // Primary key
            $table->foreignId('user_id')->constrained('users')->onDelete('cascade'); // FK → users.id

            // UI + layout settings
            $table->string('timezone')->nullable(); // Preferred timezone
            $table->string('language_code')->nullable(); // Language preference
            $table->string('sidebar_theme')->default('light'); // Theme setting
            $table->string('dashboard_layout')->nullable(); // Saved layout config

            // Notifications
            $table->boolean('email_notifications')->default(true); // Email alert toggle
            $table->string('digest_frequency')->nullable(); // Frequency e.g. daily

            // Accessibility
            $table->boolean('popup_tips_enabled')->default(true); // Tips toggle
            $table->string('font_size_preference')->nullable(); // small, medium, large
            $table->string('default_landing_page')->nullable(); // Landing route
            // Accessibility mode (uncomment if needed)
            // $table->string('accessibility_mode')->nullable(); // Visual accessibility mode

            // Legacy key/value support
            $table->string('preference_key')->nullable(); // Custom key name
            $table->text('preference_value')->nullable(); // Associated value

            $table->timestamps(); // created_at + updated_at
            $table->index(['user_id', 'language_code', 'sidebar_theme']); // Optimized lookups
        });
    }

    /**
     * =========================================================
     * Section: Drop User Preferences Table
     * Description: Removes the entire preferences schema and UX configuration
     * =========================================================
     */
    public function down(): void
    {
        Schema::dropIfExists('user_preferences');
    }
};