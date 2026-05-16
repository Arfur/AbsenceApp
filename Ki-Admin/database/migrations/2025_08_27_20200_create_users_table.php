<?php
/**
 * =========================================================
 * Project   : ki-admin - v1.0.0
 * File Name : 2025_08_07_100100_create_users_table.php
 * Author    : Michael Battle
 * Created On: 07/Aug/2025
 * Description: Creates the users table with FK to role_types
 * Origin: Adapted from support-hub users migration
 * =========================================================
 */

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

return new class extends Migration {
    public function up(): void
    {
        Schema::create('users', function (Blueprint $table) {
            // Primary key
            $table->id();

            // Custom user_id field (used for linking to other tables, incremented by 1)
            $table->integer('user_id')->unsigned()->unique()->comment('Custom user ID, incremented by 1, used for linking');

            // Username and email
            $table->string('username')->index()->comment('Unique username for login and display');
            $table->string('email')->unique()->index()->comment('User email address, must be unique');
            $table->timestamp('email_verified_at')->nullable()->comment('Timestamp when email was verified');
            $table->string('password')->comment('Hashed user password');

            // Status field
            $table->string('status', 20)->default('active')->comment('Current user status: active|suspended|pending');

            // Remember token for authentication
            $table->string('remember_token', 100)->nullable()->comment('Token for "remember me" functionality');

            // Role relationship
            $table->unsignedBigInteger('role_type_id')->nullable()->index()->comment('Foreign key to role_types table');
            $table->foreign('role_type_id')->references('id')->on('role_types')->onDelete('set null');

            // OTP fields for verification
            $table->string('otp_code', 6)->nullable()->comment('5-6 digit verification code for OTP');
            $table->timestamp('otp_expires_at')->nullable()->comment('OTP expiration time');
            $table->boolean('is_verified')->default(0)->comment('Email verification status via OTP');

            // Timestamps
            $table->timestamps();
        });
    }
    public function down(): void
    {
        Schema::dropIfExists('users');
    }
};
