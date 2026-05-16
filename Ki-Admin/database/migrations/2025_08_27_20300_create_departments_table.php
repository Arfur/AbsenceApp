<?php
/**
 * =========================================================
 * Project   : ki-admin - v1.0.0
 * File Name : 2025_08_07_100600_create_departments_table.php
 * Author    : Michael Battle
 * Created On: 07/Aug/2025
 * Description: Creates the departments table for user/job grouping
 * Origin: Adapted from support-hub departments migration
 * =========================================================
 */

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

return new class extends Migration {
    public function up(): void
    {
        Schema::create('departments', function (Blueprint $table) {
            $table->id();
            $table->string('name')->unique()->comment('Department name');
            $table->string('code')->nullable()->comment('Department code');
            $table->text('description')->nullable()->comment('Department description');
            $table->string('status')->default('active')->comment('Department status');
            $table->timestamps();
        });
    }
    public function down(): void
    {
        Schema::dropIfExists('departments');
    }
};
