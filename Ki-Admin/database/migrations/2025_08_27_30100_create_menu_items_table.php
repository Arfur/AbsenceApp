<?php
/**
 * Migration: Create menu_items table for Menu Management UI
 * chngnnnn: Created by GitHub Copilot on 2025-08-21
 * This migration defines the menu_items table structure for the new menu management system.
 */
use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

return new class extends Migration {
    /**
     * Run the migrations.
     * @return void
     */
    public function up()
    {
        Schema::create('menu_items', function (Blueprint $table) {
            $table->id();
            $table->string('title');
            $table->string('slug')->unique();
            $table->boolean('is_visible')->default(true);
            $table->string('url')->nullable();
            $table->string('route_name')->nullable();
            $table->string('icon')->nullable();
            $table->string('icon_color')->nullable();
            $table->string('tooltip')->nullable();
            $table->integer('badge_count')->nullable();
            $table->boolean('is_collapsible')->default(false);
            $table->string('permission_key')->nullable();
            $table->string('component')->nullable();
            $table->integer('order')->default(0);
            $table->boolean('is_external')->default(false);
            $table->unsignedBigInteger('parent_id')->nullable();
            $table->integer('display_order')->default(0);
            $table->string('menu_group')->nullable();
            $table->boolean('is_favorite')->default(false);
            $table->timestamps();
            $table->foreign('parent_id')->references('id')->on('menu_items')->onDelete('cascade');
        });
    }

    /**
     * Reverse the migrations.
     * @return void
     */
    public function down()
    {
        Schema::dropIfExists('menu_items');
    }
};
