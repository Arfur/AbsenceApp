<?php
/**
 * =========================================================
 * Project   : ki-admin - v1.0.0
 * File Name : 2025_08_07_100800_create_job_groups_table.php
 * Author    : Michael Battle
 * Created On: 07/Aug/2025
 * Description: Creates the job_groups table for grouping job titles
 * Origin: Adapted from support-hub job_groups migration
 * =========================================================
 */

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

return new class extends Migration {
    public function up(): void
    {
        Schema::create('job_groups', function (Blueprint $table) {
            $table->id();
            $table->string('name')->unique()->comment('Job group name');
            $table->string('code')->nullable()->comment('Job group code');
            $table->text('description')->nullable()->comment('Job group description');
            $table->timestamps();
        });
    }
    public function down(): void
    {
        Schema::dropIfExists('job_groups');
    }
};
