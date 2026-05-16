<?php
/**
 * =========================================================
 * Project   : ki-admin - v1.0.0
 * File Name : 2025_08_07_100400_create_permission_role_table.php
 * Author    : Michael Battle
 * Created On: 07/Aug/2025
 * Description: Creates the permission_role pivot table
 * Origin: Adapted from support-hub permission_role migration
 * =========================================================
 */

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

return new class extends Migration {
    public function up(): void
    {
        Schema::create('permission_role', function (Blueprint $table) {
            $table->id();
            $table->unsignedBigInteger('role_type_id');
            $table->unsignedBigInteger('permission_id');
            $table->foreign('role_type_id')->references('id')->on('role_types')->onDelete('cascade');
            $table->foreign('permission_id')->references('id')->on('permissions')->onDelete('cascade');
            $table->timestamps();
        });
    }
    public function down(): void
    {
        Schema::dropIfExists('permission_role');
    }
};
