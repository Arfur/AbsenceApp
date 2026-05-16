<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

/**
 * =========================================================
 * Project   : Support Hub - v1.0.0
 * File Name : YYYY_MM_DD_HHMMSS_create_term_dates_table.php
 * 
 * Author    : Michael Battle
 * Created On: [Set current date]
 * Updated On: [Set current date]
 * 
 * Description:
 * Defines academic or organizational terms linked to schools. 
 * Enables segmentation for class filtering, dashboard logic, 
 * and analytics.
 * 
 * Origin:
 * Academic scheduling and filtering module.
 * 
 * Changes:
 * - Linked terms to schools
 * - Introduced active flag and lifecycle status
 * =========================================================
 */

return new class extends Migration
{
    /**
     * =========================================================
     * Section: Create Term Dates Table
     * Description: Stores academic periods with lifecycle flags
     * =========================================================
     */
    public function up(): void
    {
        Schema::create('term_dates', function (Blueprint $table) {
            $table->id();
            $table->foreignId('school_id')->constrained('schools')->onDelete('cascade');

            $table->string('name');          // e.g. Spring 2026
            $table->string('code')->unique()->nullable(); // optional internal code
            $table->date('start_date');
            $table->date('end_date');

            $table->boolean('is_active')->default(false);
            $table->string('status')->default('active');
            $table->text('notes')->nullable();

            $table->timestamps();

            $table->index(['school_id', 'is_active', 'status']);
        });
    }

    /**
     * =========================================================
     * Section: Drop Term Dates Table
     * Description: Removes structured academic scheduling records
     * =========================================================
     */
    public function down(): void
    {
        Schema::dropIfExists('term_dates');
    }
};
