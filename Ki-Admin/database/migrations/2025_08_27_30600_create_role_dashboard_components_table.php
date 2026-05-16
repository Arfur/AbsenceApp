<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

/**
 * =========================================================
 * Project   : Support Hub - v1.0.0
 * File Name : 2025_07_08_002157_create_role_dashboard_components_table.php
 * 
 * Author    : Michael Battle
 * Created On: 08/July/2025
 * Updated On: 08/July/2025
 * 
 * Description:
 * Pivot table linking roles to dashboard components. Enables 
 * dynamic layout rendering based on role visibility and 
 * dashboard personalization.
 * 
 * Origin:
 * Dashboard group. Used in role-based filtering, layout 
 * configuration, and access control.
 * 
 * Changes:
 * - Implemented agreed field structure
 * - Composite uniqueness constraint applied
 * - Included timestamps and audit fields
 * =========================================================
 */

return new class extends Migration
{
    /**
     * =========================================================
     * Section: Create Role Dashboard Components Table
     * Description: Maps dashboard widgets to roles with visibility and audit metadata
     * =========================================================
     */
    public function up(): void
    {
        Schema::create('role_dashboard_components', function (Blueprint $table) {
            $table->id();

            $table->unsignedBigInteger('role_type_id');
            $table->foreign('role_type_id')->references('id')->on('role_types')->onDelete('cascade');
            $table->foreignId('db_component_id')->constrained('dashboard_components')->onDelete('cascade');

            $table->datetime('assigned_at')->nullable();   // Timestamp of assignment
            $table->string('status')->nullable();          // active, hidden, expired
            $table->text('notes')->nullable();             // Commentary or audit trail

            $table->timestamps();

            $table->unique(['role_type_id', 'db_component_id'], 'role_dash_comp_unique');
        });
    }

    /**
     * =========================================================
     * Section: Drop Role Dashboard Components Table
     * Description: Reverts role-component mapping structure
     * =========================================================
     */
    public function down(): void
    {
        Schema::dropIfExists('role_dashboard_components');
    }
};
