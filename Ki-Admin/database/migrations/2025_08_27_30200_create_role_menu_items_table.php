<?php
/**
 * =========================================================
 * Project   : ki-admin - v1.0.0
 * File Name : 2025_08_09_011946_create_role_menu_items_table.php
 * Author    : Michael Battle
 * Created On: 09/Aug/2025
 * Description: Creates the role_menu_items table for role-based menu permissions
 * Origin: Role-based menu system implementation
 * =========================================================
 */

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

return new class extends Migration
{
    /**
     * Run the migrations.
     */
    public function up(): void
    {
        Schema::create('role_menu_items', function (Blueprint $table) {
            $table->id();
            $table->unsignedBigInteger('role_type_id')->nullable();
            $table->string('role_type')->index();
            $table->unsignedBigInteger('menu_item_id');
            $table->boolean('is_granted')->default(true);
            // Per-role default menu marker; application logic should ensure only one default per role_type
            $table->boolean('is_default')->default(false);
            $table->timestamp('assigned_at')->nullable();
            $table->string('status')->default('active');
            $table->text('notes')->nullable();
            $table->timestamps();
            
            $table->foreign('role_type_id')->references('id')->on('role_types')->onDelete('cascade');
            $table->foreign('menu_item_id')->references('id')->on('menu_items')->onDelete('cascade');
            
            $table->unique(['role_type', 'menu_item_id']);
            $table->index(['role_type', 'is_granted']);
            $table->index(['role_type', 'is_default']);
        });
    }

    /**
     * Reverse the migrations.
     */
    public function down(): void
    {
        Schema::dropIfExists('role_menu_items');
    }
};
