<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

/**
 * =========================================================
 * Project   : Support Hub - v1.0.0
 * File Name : 2025_07_08_002158_create_user_dashboard_components_table.php
 * 
 * Author    : Michael Battle
 * Created On: 08/July/2025
 * Updated On: 08/July/2025
 * 
 * Description:
 * Pivot table linking users to dashboard components. Enables 
 * personalized layout control, visibility toggles, pinning, 
 * and override logic.
 * 
 * Origin:
 * Dashboard group. Used in user-specific dashboards, 
 * layout configuration, and UI personalization.
 * 
 * Changes:
 * - Implemented agreed field structure
 * - Included timestamps and override metadata
 * =========================================================
 */

return new class extends Migration
{
    /**
     * =========================================================
     * Section: Create User Dashboard Components Table
     * Description: Maps dashboard widgets to users with layout and override metadata
     * =========================================================
     */
    public function up(): void
    {
        Schema::create('user_dashboard_components', function (Blueprint $table) {
            $table->id();

            $table->foreignId('user_id')->constrained()->onDelete('cascade');
            $table->foreignId('component_id')->constrained('dashboard_components')->onDelete('cascade');

            $table->integer('position')->default(0);         // Layout index
            $table->boolean('is_visible')->default(true);    // Visibility toggle
            $table->boolean('is_pinned')->default(false);    // Fixed placement
            $table->json('settings_override')->nullable();   // User-specific config

            $table->timestamps();

            $table->unique(['user_id', 'component_id']);
        });
    }

    /**
     * =========================================================
     * Section: Drop User Dashboard Components Table
     * Description: Reverts user-component mapping structure
     * =========================================================
     */
    public function down(): void
    {
        Schema::dropIfExists('user_dashboard_components');
    }
};
