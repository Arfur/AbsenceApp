<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

/**
 * =========================================================
 * Project   : Support Hub - v1.0.0
 * File Name : 2025_07_11_000001_create_department_school_table.php
 * 
 * Author    : Michael Battle
 * Created On: 11/July/2025
 * 
 * Description:
 * Pivot table linking departments to schools. Supports assignment 
 * of universal departments (e.g. Maths, English) across all institutions, 
 * and flexible per-school customization.
 * 
 * Notes:
 * - Establishes many-to-many mapping
 * - Core departments are auto-linked via seeder
 * - Prevents duplicate links with composite uniqueness
 * =========================================================
 */

return new class extends Migration
{
    /**
     * =========================================================
     * Section: Create Pivot Table
     * =========================================================
     */
    public function up(): void
    {
        Schema::create('department_school', function (Blueprint $table) {
            $table->id();

            // FK links
            $table->foreignId('school_id')->constrained()->onDelete('cascade');
            $table->foreignId('department_id')->constrained()->onDelete('cascade');

            $table->timestamps();

            // Unique constraint to prevent duplicate links
            $table->unique(['school_id', 'department_id']);
        });
    }

    /**
     * =========================================================
     * Section: Drop Pivot Table
     * =========================================================
     */
    public function down(): void
    {
        Schema::dropIfExists('department_school');
    }
};
