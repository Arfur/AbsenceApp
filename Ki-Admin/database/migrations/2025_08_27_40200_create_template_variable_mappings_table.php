<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

/**
 * =========================================================
 * Project   : Support Hub - v1.0.0
 * File Name : 2025_07_08_002164_create_template_variable_mappings_table.php
 * 
 * Author    : Michael Battle
 * Created On: 08/July/2025
 * Updated On: 08/July/2025
 * 
 * Description:
 * Stores runtime values mapped to template placeholders. 
 * Enables dynamic rendering of notifications with 
 * context-aware injection and audit tracking.
 * 
 * Origin:
 * Notifications group. Used in personalization, 
 * template rendering, and message formatting.
 * 
 * Changes:
 * - Implemented agreed field structure
 * - Indexed key fields for performance
 * - Included timestamps and dynamic flags
 * =========================================================
 */

return new class extends Migration
{
    /**
     * =========================================================
     * Section: Create Template Variable Mappings Table
     * Description: Maps runtime values to template placeholders for dynamic rendering
     * =========================================================
     */
    public function up(): void
    {
        Schema::create('template_variable_mappings', function (Blueprint $table) {
            $table->id();

            $table->foreignId('template_id')->constrained('notification_templates')->onDelete('cascade');

            $table->string('variable_name');                   // Placeholder name
            $table->text('mapped_value')->nullable();          // Actual value
            $table->string('value_type')->default('string');   // Format
            $table->string('source_context')->nullable();      // Origin of value
            $table->boolean('is_dynamic')->default(false);     // Runtime evaluation flag

            $table->timestamps();

            $table->unique(['template_id', 'variable_name']);
        });
    }

    /**
     * =========================================================
     * Section: Drop Template Variable Mappings Table
     * Description: Reverts variable-to-value mapping structure
     * =========================================================
     */
    public function down(): void
    {
        Schema::dropIfExists('template_variable_mappings');
    }
};
