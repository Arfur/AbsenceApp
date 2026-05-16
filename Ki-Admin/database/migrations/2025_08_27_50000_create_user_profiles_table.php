<?php
/**
 * =========================================================
 * Project   : ki-admin - v1.0.0
 * File Name : 2025_08_07_100900_create_user_profiles_table.php
 * Author    : Michael Battle
 * Created On: 07/Aug/2025
 * Description: Creates the user_profiles table for extended user info
 * Origin: Adapted from support-hub user_profiles migration
 * =========================================================
 */

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

return new class extends Migration {
    public function up(): void
    {
        Schema::create('user_profiles', function (Blueprint $table) {
            $table->id();
            $table->unsignedBigInteger('user_id')->comment('FK to users table');
            $table->string('first_name')->nullable();
            $table->string('last_name')->nullable();
            $table->string('preferred_name')->nullable();
            $table->string('title')->nullable();
            $table->date('date_of_birth')->nullable();
            $table->string('profile_picture_url')->nullable();
            $table->text('bio')->nullable();
            $table->string('gender')->nullable();
            $table->string('timezone')->nullable();
            $table->string('language')->nullable();
            $table->unsignedBigInteger('department_id')->nullable();
            $table->unsignedBigInteger('job_title_id')->nullable();
            $table->unsignedBigInteger('school_id')->nullable();
            $table->timestamps();
            
            $table->foreign('user_id')->references('id')->on('users')->onDelete('cascade');
            $table->foreign('department_id')->references('id')->on('departments')->onDelete('set null');
            $table->foreign('job_title_id')->references('id')->on('job_titles')->onDelete('set null');
        });
    }
    public function down(): void
    {
        Schema::dropIfExists('user_profiles');
    }
};
