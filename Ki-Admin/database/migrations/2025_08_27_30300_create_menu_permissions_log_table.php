<?php
/**
 * Migration: Create menu_permissions_log table for Menu Management UI
 * chngnnnn: Created by GitHub Copilot on 2025-08-21
 * This migration defines the log table for menu permission changes and audit trail.
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
        Schema::create('menu_permissions_log', function (Blueprint $table) {
            $table->id();
            $table->unsignedBigInteger('menu_item_id');
            $table->string('action');
            $table->unsignedBigInteger('user_id');
            $table->string('role')->nullable();
            $table->text('change_details')->nullable();
            $table->timestamps();
            $table->foreign('menu_item_id')->references('id')->on('menu_items')->onDelete('cascade');
            $table->foreign('user_id')->references('id')->on('users')->onDelete('cascade');
        });
    }

    /**
     * Reverse the migrations.
     * @return void
     */
    public function down()
    {
        Schema::dropIfExists('menu_permissions_log');
    }
};
