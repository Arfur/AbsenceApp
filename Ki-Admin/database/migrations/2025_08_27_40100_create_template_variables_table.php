<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

/**
 * =========================================================
 * Project   : Support Hub - v1.0.0
 * File Name : 2025_07_08_002163_create_template_variables_table.php
 * 
 * Author    : Michael Battle
 * Created On: 08/July/2025
 * Updated On: 08/July/2025
 * 
 * Description:
 * Stores dynamic placeholders used in notification templates. 
 * Supports variable injection, validation, and fallback logic.
 * 
 * Origin:
 * Notifications group. Used in template rendering, 
 * personalization, and message formatting.
 * 
 * Changes:
 * - Implemented agreed field structure
 * - Indexed key fields for performance
 * - Included timestamps and validation flags
 * =========================================================
 */

return new class extends Migration
{
    /**
     * =========================================================
     * Section: Create Template Variables Table
     * Description: Stores dynamic placeholders for notification templates
     * =========================================================
     */
    public function up(): void
    {
        Schema::create('template_variables', function (Blueprint $table) {
            $table->id();

            $table->foreignId('template_id')->constrained('notification_templates')->onDelete('cascade');

            $table->string('name');                          // Variable name
            $table->string('placeholder');                   // Format used in templates
            $table->text('description')->nullable();         // Admin-facing explanation
            $table->string('data_type')->default('string');  // Expected type
            $table->boolean('is_required')->default(false);  // Validation toggle
            $table->text('default_value')->nullable();       // Fallback value

            $table->timestamps();

            $table->unique(['template_id', 'name']);
        });
    }

    /**
     * =========================================================
     * Section: Drop Template Variables Table
     * Description: Reverts variable storage structure
     * =========================================================
     */
    public function down(): void
    {
        Schema::dropIfExists('template_variables');
    }
};
