<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

/**
 * =========================================================
 * Project   : Support Hub - v1.0.0
 * File Name : 2025_07_08_002160_create_user_settings_table.php
 * 
 * Author    : Michael Battle
 * Created On: 08/July/2025
 * Updated On: 11/July/2025
 * 
 * Description:
 * Stores per-user overrides for system settings. Enables 
 * personalized configuration, feature toggles, audit tracking, 
 * and dynamic metadata control.
 * 
 * Notes:
 * - Composite uniqueness to prevent duplicate setting entries
 * - Extended fields added for key/value config management
 * =========================================================
 */

return new class extends Migration
{
    /**
     * =========================================================
     * Section: Create User Settings Table
     * Description: Maps system settings to users with override values and metadata
     * =========================================================
     */
    public function up(): void
    {
        Schema::create('user_settings', function (Blueprint $table) {
            $table->id();

            // Core associations
            $table->foreignId('user_id')->constrained()->onDelete('cascade');
            $table->foreignId('setting_id')->constrained('settings')->onDelete('cascade');

            // Override mechanics
            $table->text('override_value')->nullable();        // User-specific override
            $table->boolean('is_active')->default(true);       // Toggle enable/disable
            $table->text('notes')->nullable();                 // Commentary or audit trail

            // Extended metadata
            $table->string('setting_key')->nullable();         // e.g. dark_mode
            $table->text('setting_value')->nullable();         // Raw base value
            $table->string('value_type')->nullable();          // Data type (boolean, string, json)
            $table->text('description')->nullable();           // Purpose of setting
            $table->boolean('is_locked')->default(false);      // UI lock flag
            $table->string('status')->default('active');       // Lifecycle indicator

            // Audit timestamps
            $table->timestamps();

            // Enforce uniqueness: one override per user-setting combo
            $table->unique(['user_id', 'setting_id']);
        });
    }

    /**
     * =========================================================
     * Section: Drop User Settings Table
     * Description: Reverts the user-setting override structure
     * =========================================================
     */
    public function down(): void
    {
        Schema::dropIfExists('user_settings');
    }
};
