<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

/**
 * =========================================================
 * Project   : Support Hub - v1.0.0
 * File Name : 2025_07_08_002156_create_dashboard_components_table.php
 * 
 * Author    : Michael Battle
 * Created On: 08/July/2025
 * Updated On: 08/July/2025
 * 
 * Description:
 * Stores dashboard widgets, panels, and blocks for 
 * dynamic rendering and layout control.
 * 
 * Origin:
 * Dashboard group. Used in role-based dashboards, 
 * user personalization, and layout configuration.
 * 
 * Changes:
 * - Implemented agreed field structure
 * - Indexed key fields for performance
 * - Included timestamps and visibility flags
 * =========================================================
 */

return new class extends Migration
{
    public function up(): void
    {
        Schema::create('dashboard_components', function (Blueprint $table) {
            $table->id();

            $table->string('name')->unique();              // Internal identifier
            $table->string('display_name');                // UI label
            $table->string('type');                        // Widget, panel, chart, etc.
            $table->string('module');                      // Logical grouping
            $table->text('description')->nullable();       // Tooltip or summary
            $table->integer('default_position')->default(0); // Sort index
            $table->boolean('is_global')->default(false);  // Shared across users
            $table->string('status')->default('active');   // Status flag

            $table->timestamps();

            $table->index(['module', 'type']);
        });
    }

    public function down(): void
    {
        Schema::dropIfExists('dashboard_components');
    }
};
