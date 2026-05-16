<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

/**
 * =========================================================
 * Project   : ki-admin - v1.0.0
 * File Name : 2025_08_08_101000_create_schools_table.php
 * 
 * Author    : Michael Battle
 * Created On: 08/Aug/2025
 * 
 * Description:
 * Stores school records with external reference, metadata, 
 * and optional head user linkage.
 * 
 * Origin:
 * Adapted from support-hub schools migration.
 * Used in department mapping, user affiliation, and dashboard filtering.
 * =========================================================
 */

return new class extends Migration
{
    /**
     * =========================================================
     * Section: Create Schools Table
     * Description: Stores school metadata and head user linkage
     * =========================================================
     */
    public function up(): void
    {
        Schema::create('schools', function (Blueprint $table) {
            $table->id();
            $table->string('school_ref')->unique();                  // External/public reference
            $table->string('name');
            $table->string('code')->unique();
            $table->text('description')->nullable();

            $table->unsignedBigInteger('head_user_id')->nullable(); // FK placeholder, constraint deferred

            $table->string('status')->default('active');
            $table->timestamps();

            $table->index(['head_user_id', 'status']);
        });
    }

    /**
     * =========================================================
     * Section: Drop Schools Table
     * Description: Reverts school structure and relationships
     * =========================================================
     */
    public function down(): void
    {
        Schema::dropIfExists('schools');
    }
};
