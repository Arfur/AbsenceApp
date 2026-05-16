<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

/**
 * =========================================================
 * Project   : Support Hub - v1.0.0
 * File Name : YYYY_MM_DD_HHMMSS_create_user_social_accounts_table.php
 * 
 * Author    : Michael Battle
 * Created On: [Set current date]
 * Updated On: [Set current date]
 * 
 * Description:
 * Stores external social media logins tied to internal users.
 * Supports OAuth sign-in, account linking, and login history.
 * 
 * Origin:
 * Authentication + Profile linkage module. Designed to support 
 * extensible provider-based user mapping.
 * 
 * Changes:
 * - Added external provider fields
 * - Enabled FK tracking via `user_id`
 * =========================================================
 */

return new class extends Migration
{
    /**
     * =========================================================
     * Section: Create Social Accounts Table
     * Description: Links users to external authentication providers
     * =========================================================
     */
    public function up(): void
    {
        Schema::create('user_social_accounts', function (Blueprint $table) {
            $table->id();
            $table->foreignId('user_id')->constrained('users')->onDelete('cascade');

            $table->string('provider'); // e.g., google, github, linkedin
            $table->string('provider_user_id');
            $table->string('username')->nullable();
            $table->string('email')->nullable();
            $table->string('avatar_url')->nullable();

            $table->text('token')->nullable();
            $table->timestamp('token_expiry')->nullable();

            $table->boolean('is_primary')->default(false);
            $table->string('status')->default('active');

            $table->timestamps();

            $table->unique(['provider', 'provider_user_id']);
            $table->index(['user_id', 'provider', 'status']);
        });
    }

    /**
     * =========================================================
     * Section: Drop Social Accounts Table
     * Description: Removes external login mapping
     * =========================================================
     */
    public function down(): void
    {
        Schema::dropIfExists('user_social_accounts');
    }
};
