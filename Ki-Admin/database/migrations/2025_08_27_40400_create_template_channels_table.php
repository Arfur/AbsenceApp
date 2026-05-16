<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

/**
 * =========================================================
 * Project   : Support Hub - v1.0.0
 * File Name : 2025_07_08_002166_create_template_channels_table.php
 * 
 * Author    : Michael Battle
 * Created On: 08/July/2025
 * Updated On: 08/July/2025
 * 
 * Description:
 * Stores available delivery channels for notifications. 
 * Supports metadata, module grouping, and activation toggles.
 * 
 * Origin:
 * Notifications group. Used in template rendering, 
 * delivery routing, and admin configuration.
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
     * Section: Create Template Channels Table
     * Description: Stores delivery methods for notifications with metadata and activation flags
     * =========================================================
     */
    public function up(): void
    {
        Schema::create('template_channels', function (Blueprint $table) {
            $table->id();

            $table->string('name')->unique();             // Internal identifier
            $table->string('display_name');               // UI label
            $table->text('description')->nullable();      // Tooltip or summary
            $table->string('module')->nullable();         // Logical grouping
            $table->boolean('is_active')->default(true);  // Enable/disable toggle

            $table->timestamps();

            $table->index(['module', 'is_active']);
        });
    }

    /**
     * =========================================================
     * Section: Drop Template Channels Table
     * Description: Reverts delivery channel storage structure
     * =========================================================
     */
    public function down(): void
    {
        Schema::dropIfExists('template_channels');
    }
};
