<?php
/**
 * =========================================================
 * Project   : ki-admin - v1.0.0
 * File Name : 2025_08_07_100700_create_job_titles_table.php
 * Author    : Michael Battle
 * Created On: 07/Aug/2025
 * Description: Creates the job_titles table for user roles/jobs
 * Origin: Adapted from support-hub job_titles migration
 * =========================================================
 */

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

return new class extends Migration {
    public function up(): void
    {
        Schema::create('job_titles', function (Blueprint $table) {
            $table->id();
            $table->string('title')->unique()->comment('Job title');
            $table->string('code')->nullable()->comment('Job title code');
            $table->text('description')->nullable()->comment('Job title description');
            $table->timestamps();
        });
    }
    public function down(): void
    {
        Schema::dropIfExists('job_titles');
    }
};
