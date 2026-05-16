<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

/**
 * =========================================================
 * Project   : Support Hub - v1.0.0
 * File Name : YYYY_MM_DD_HHMMSS_create_classes_table.php
 * 
 * Author    : Michael Battle
 * Created On: 2025-07-11
 * Updated On: 2025-07-11
 * 
 * Description:
 * Stores academic or organizational class entities linked 
 * to schools and departments. Powers user-class mapping.
 * 
 * Origin:
 * Academic module. Integrates with user-class linking and dashboard filtering.
 * 
 * Changes:
 * - Added FK links to schools and departments
 * - Removed unnecessary term field
 * =========================================================
 */

return new class extends Migration
{
    /**
     * =========================================================
     * Section: Create Classes Table
     * Description: Defines core class records linked to academic structure
     * =========================================================
     */
    public function up(): void
    {
        Schema::create('classes', function (Blueprint $table) {
            $table->id();
            $table->foreignId('school_id')->constrained('schools')->onDelete('cascade');
            $table->foreignId('department_id')->constrained('departments')->onDelete('cascade');

            $table->string('code')->unique(); // e.g. C01
            $table->string('name');
            $table->text('description')->nullable();
            $table->string('status')->default('active');

            $table->timestamps();

            $table->index(['school_id', 'department_id', 'status']);
        });
    }

    /**
     * =========================================================
     * Section: Drop Classes Table
     * Description: Rolls back academic class definitions
     * =========================================================
     */
    public function down(): void
    {
        Schema::dropIfExists('classes');
    }
};
