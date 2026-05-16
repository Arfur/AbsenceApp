<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

/**
 * =========================================================
 * Project   : Support Hub - v1.0.0
 * File Name : YYYY_MM_DD_HHMMSS_create_class_user_table.php
 * 
 * Author    : Michael Battle
 * Created On: [Set current date]
 * Updated On: [Set current date]
 * 
 * Description:
 * Links users to classes using a pivot relationship. Supports 
 * teaching assignments, student enrollment, and contextual 
 * filtering for dashboards and permissions.
 * 
 * Origin:
 * Academic module — built to support flexible user-class mapping.
 * 
 * Changes:
 * - Added FK links to classes and users
 * - Included role tracking and assignment status
 * =========================================================
 */

return new class extends Migration
{
    /**
     * =========================================================
     * Section: Create Class-User Mapping
     * Description: Tracks user roles and assignments within each class
     * =========================================================
     */
    public function up(): void
    {
        Schema::create('class_user', function (Blueprint $table) {
            $table->id();
            $table->foreignId('class_id')->constrained('classes')->onDelete('cascade');
            $table->foreignId('user_id')->constrained('users')->onDelete('cascade');

            $table->string('role_type')->nullable(); // student, teacher, observer
            $table->timestamp('assigned_at')->nullable();
            $table->string('status')->default('active');
            $table->text('notes')->nullable();

            $table->timestamps();

            $table->unique(['class_id', 'user_id']);
            $table->index(['class_id', 'user_id', 'role_type', 'status']);
        });
    }

    /**
     * =========================================================
     * Section: Drop Class-User Mapping
     * Description: Removes role-based class assignments
     * =========================================================
     */
    public function down(): void
    {
        Schema::dropIfExists('class_user');
    }
};
