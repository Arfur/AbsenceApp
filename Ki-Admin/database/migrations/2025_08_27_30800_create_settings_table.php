<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

/**
 * =========================================================
 * Project   : Support Hub - v1.0.0
 * File Name : 2025_07_08_002159_create_settings_table.php
 * 
 * Author    : Michael Battle
 * Created On: 08/July/2025
 * Updated On: 08/July/2025
 * 
 * Description:
 * Stores system-wide configuration values, feature flags, 
 * and module-specific settings. Supports dynamic behavior 
 * across the platform.
 * 
 * Origin:
 * Settings group. Used in admin panels, feature toggles, 
 * and module-level configuration.
 * 
 * Changes:
 * - Implemented agreed field structure
 * - Indexed key and module for fast lookup
 * - Included timestamps and activation toggle
 * =========================================================
 */

return new class extends Migration
{
    /**
     * =========================================================
     * Section: Create Settings Table
     * Description: Stores configuration values and metadata for modules and features
     * =========================================================
     */
    public function up(): void
    {
        Schema::create('settings', function (Blueprint $table) {
            $table->id();

            $table->string('key')->unique();             // Unique setting identifier
            $table->text('value')->nullable();           // Stored value
            $table->string('type')->default('string');   // Data format
            $table->string('module')->nullable();        // Logical grouping
            $table->text('description')->nullable();     // Admin-facing summary
            $table->boolean('is_active')->default(true); // Toggle for enable/disable

            $table->timestamps();

            $table->index(['module', 'type']);
        });
    }

    /**
     * =========================================================
     * Section: Drop Settings Table
     * Description: Reverts configuration storage structure
     * =========================================================
     */
    public function down(): void
    {
        Schema::dropIfExists('settings');
    }
};
