<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

/**
 * =========================================================
 * Project   : Support Hub - v1.0.0
 * File Name : YYYY_MM_DD_HHMMSS_create_contact_details_table.php
 * 
 * Author    : Michael Battle
 * Created On: [Set current date]
 * Updated On: [Set current date]
 * 
 * Description:
 * Stores detailed contact and emergency data linked to users. 
 * Supports location filtering, regulatory compliance, and 
 * communication routing.
 * 
 * Origin:
 * Extended user metadata module.
 * 
 * Changes:
 * - Added address fields and emergency contact data
 * - Linked to user record via FK
 * =========================================================
 */

return new class extends Migration
{
    /**
     * =========================================================
     * Section: Create Contact Details Table
     * Description: Stores user-specific location and emergency info
     * =========================================================
     */
    public function up(): void
    {
        Schema::create('contact_details', function (Blueprint $table) {
            $table->id();
            $table->foreignId('user_id')->constrained('users')->onDelete('cascade');

            $table->string('phone_number')->nullable();
            $table->string('alternate_email')->nullable();

            $table->string('address_line_1')->nullable();
            $table->string('address_line_2')->nullable();
            $table->string('city')->nullable();
            $table->string('county_or_region')->nullable();
            $table->string('postal_code')->nullable();
            $table->string('country')->nullable();

            $table->string('emergency_contact_name')->nullable();
            $table->string('emergency_contact_phone')->nullable();
            $table->string('relationship_to_user')->nullable();

            $table->text('notes')->nullable();
            $table->timestamps();

            $table->index(['user_id', 'country', 'city']);
        });
    }

    /**
     * =========================================================
     * Section: Drop Contact Details Table
     * Description: Rolls back user location and contact metadata
     * =========================================================
     */
    public function down(): void
    {
        Schema::dropIfExists('contact_details');
    }
};
