<?php
/**
 * =========================================================
 * Project   : ki-admin - v1.0.0
 * File Name : 2025_08_07_100500_create_user_menu_items_table.php
 * Author    : Michael Battle
 * Created On: 07/Aug/2025
 * Description: Creates the user_menu_items pivot table
 * Origin: Adapted from support-hub user_menu_items migration
 * =========================================================
 */

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

return new class extends Migration {
    public function up(): void
    {
        Schema::create('user_menu_items', function (Blueprint $table) {
            $table->id();
            $table->unsignedBigInteger('user_id');
            $table->unsignedBigInteger('menu_item_id');
            $table->boolean('is_granted')->default(true);
            // Per-user default marker; ensure only one default per user via application logic
            $table->boolean('is_default')->default(false);
            $table->integer('custom_order')->nullable();
            $table->boolean('is_custom')->default(false);
            $table->timestamps();
            
            $table->foreign('user_id')->references('id')->on('users')->onDelete('cascade');
            $table->foreign('menu_item_id')->references('id')->on('menu_items')->onDelete('cascade');
            
            // Prevent duplicate grants for same user-menu combination
            $table->unique(['user_id', 'menu_item_id']);
            $table->index(['user_id', 'is_granted']);
            $table->index(['user_id', 'is_default']);
            $table->index(['custom_order']);
        });
    }
    public function down(): void
    {
        Schema::dropIfExists('user_menu_items');
    }
};
