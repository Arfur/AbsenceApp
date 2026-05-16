<?php

/**
 * =========================================================
 * Migration  : 2025_07_25_200080_create_permission_user_table.php
 * Project    : Support Hub - v1.0.0
 * Author     : Michael Battle
 * Created On : 25/07/2025
 * Updated On : 28/07/2025
 *
 * Description:
 *   Pivot table linking users to permissions with metadata.
 *
 * Notes:
 *   - Retains `id` PK for audit references.
 *   - Ensures BIGINT UNSIGNED types match parent tables.
 *   - Composite uniqueness on user_id + permission_id.
 * =========================================================
 */

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

return new class extends Migration
{
    /* =====================================================
     * Section    : up()
     * Description: Create permission_user pivot table
     * ===================================================== */
    public function up(): void
    {
        Schema::create('permission_user', function (Blueprint $table) {
            $table->bigIncrements('id');

            // FK columns must match parent PK types
            $table->unsignedBigInteger('user_id');
            $table->unsignedBigInteger('permission_id');

            $table->datetime('granted_at')->nullable();    // Timestamp of grant
            $table->datetime('expiry_date')->nullable();   // Optional expiration
            $table->text('notes')->nullable();             // Reason, context, or annotation

            $table->timestamps();

            // Foreign key constraints
            $table->foreign('user_id')
                  ->references('id')->on('users')
                  ->onDelete('cascade');

            $table->foreign('permission_id')
                  ->references('id')->on('permissions')
                  ->onDelete('cascade');

            // Ensure no duplicate grants per user/permission
            $table->unique(['user_id', 'permission_id']);
        });
    }

    /* =====================================================
     * Section    : down()
     * Description: Drop permission_user pivot table
     * ===================================================== */
    public function down(): void
    {
        Schema::dropIfExists('permission_user');
    }
};