<?php

/**
 * =========================================================
 * Project   : ki-admin - v1.0.0
 * File Name : 2025_08_04_202019_create_role_types_table.php
 * 
 * Author    : Michael Battle
 * Created On: 04/Aug/2025
 * Updated On: 04/Aug/2025
 * 
 * Description:
 * Creates role_types table with predefined roles for the Support Hub.
 * Includes user, admin, and super_admin roles with proper hierarchy.
 * 
 * Origin:
 * Role management system for Support Hub authentication
 * 
 * Changes:
 * - Created role_types table structure
 * - Added default role data seeding
 * - Established role hierarchy with priority system
 * =========================================================
 */

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;
use Illuminate\Support\Facades\DB;

return new class extends Migration
{
    /**
     * Run the migrations.
     */
    public function up(): void
    {
        Schema::create('role_types', function (Blueprint $table) {
            $table->id();
            $table->string('name')->unique()->comment('Unique role identifier (slug)');
            $table->string('display_name')->comment('Human-readable role name');
            $table->text('description')->nullable()->comment('Role description and capabilities');
            $table->boolean('is_system_role')->default(false)->comment('System-defined role (cannot be deleted)');
            $table->boolean('is_default')->default(false)->comment('Default role for new registrations');
            $table->integer('priority')->default(1)->comment('Role hierarchy priority (higher = more permissions)');
            $table->timestamps();
            
            // Indexes for performance
            $table->index('name');
            $table->index('is_default');
            $table->index('priority');
        });

        // Seed default role types
        $this->seedDefaultRoles();
    }

    /**
     * Reverse the migrations.
     */
    public function down(): void
    {
        Schema::dropIfExists('role_types');
    }

    /**
     * Seed the default role types
     */
    private function seedDefaultRoles(): void
    {
        DB::table('role_types')->insert([
            [
                'id' => 1,
                'name' => 'user',
                'display_name' => 'User',
                'description' => 'Standard account with limited dashboard access and basic support ticket creation',
                'is_system_role' => false,
                'is_default' => true,
                'priority' => 1,
                'created_at' => '2025-07-28 17:47:13',
                'updated_at' => '2025-07-28 17:47:13',
            ],
            [
                'id' => 2,
                'name' => 'admin',
                'display_name' => 'Admin',
                'description' => 'Staff-level access with management tools but restricted system settings',
                'is_system_role' => true,
                'is_default' => false,
                'priority' => 50,
                'created_at' => '2025-07-28 17:47:13',
                'updated_at' => '2025-07-28 17:47:13',
            ],
            [
                'id' => 3,
                'name' => 'super_admin',
                'display_name' => 'Super Admin',
                'description' => 'Full platform control, including settings, users, and system administration',
                'is_system_role' => true,
                'is_default' => false,
                'priority' => 100,
                'created_at' => '2025-07-28 17:47:13',
                'updated_at' => '2025-07-28 17:47:13',
            ],
        ]);
    }
};
