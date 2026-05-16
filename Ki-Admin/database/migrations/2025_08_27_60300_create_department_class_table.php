<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

/**
 * =========================================================
 * Project   : Support Hub - v1.0.0
 * File Name : YYYY_MM_DD_HHMMSS_create_department_class_table.php
 * 
 * Author    : Michael Battle
 * Created On: 2025-07-11
 * Updated On: 2025-07-11
 * 
 * Description:
 * Pivot table linking departments to classes, enabling each
 * department to offer multiple classes without redundancy.
 * Supports dynamic per‐school configuration via department_school.
 * 
 * =========================================================
 */

return new class extends Migration
{
    /**
     * =========================================================
     * Section: Create department_class Pivot Table
     * =========================================================
     */
    public function up(): void
    {
        Schema::create('department_class', function (Blueprint $table) {
            $table->unsignedBigInteger('department_id');
            $table->unsignedBigInteger('class_id');

            // Composite primary key enforces uniqueness
            $table->primary(['department_id', 'class_id']);

            // Foreign key constraints
            $table->foreign('department_id')
                  ->references('id')
                  ->on('departments')
                  ->onDelete('cascade');

            $table->foreign('class_id')
                  ->references('id')
                  ->on('classes')
                  ->onDelete('cascade');
        });
    }

    /**
     * =========================================================
     * Section: Drop department_class Pivot Table
     * =========================================================
     */
    public function down(): void
    {
        Schema::dropIfExists('department_class');
    }
};
