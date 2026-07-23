<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

class AddIsDefaultToMenuTables extends Migration
{
    /**
     * Run the migrations.
     *
     * @return void
     */
    public function up()
    {
        // Add to menu_items
        if (Schema::hasTable('menu_items')) {
            Schema::table('menu_items', function (Blueprint $table) {
                if (!Schema::hasColumn('menu_items', 'is_default')) {
                    $table->boolean('is_default')->default(0)->index();
                }
            });
        }

        // Add to role_menu_items
        if (Schema::hasTable('role_menu_items')) {
            Schema::table('role_menu_items', function (Blueprint $table) {
                if (!Schema::hasColumn('role_menu_items', 'is_default')) {
                    $table->boolean('is_default')->default(0)->index();
                }
            });
        }

        // Add to user_menu_items
        if (Schema::hasTable('user_menu_items')) {
            Schema::table('user_menu_items', function (Blueprint $table) {
                if (!Schema::hasColumn('user_menu_items', 'is_default')) {
                    $table->boolean('is_default')->default(0)->index();
                }
            });
        }
    }

    /**
     * Reverse the migrations.
     *
     * @return void
     */
    public function down()
    {
        if (Schema::hasTable('menu_items')) {
            Schema::table('menu_items', function (Blueprint $table) {
                if (Schema::hasColumn('menu_items', 'is_default')) {
                    $table->dropIndex(['is_default']);
                    $table->dropColumn('is_default');
                }
            });
        }

        if (Schema::hasTable('role_menu_items')) {
            Schema::table('role_menu_items', function (Blueprint $table) {
                if (Schema::hasColumn('role_menu_items', 'is_default')) {
                    $table->dropIndex(['is_default']);
                    $table->dropColumn('is_default');
                }
            });
        }

        if (Schema::hasTable('user_menu_items')) {
            Schema::table('user_menu_items', function (Blueprint $table) {
                if (Schema::hasColumn('user_menu_items', 'is_default')) {
                    $table->dropIndex(['is_default']);
                    $table->dropColumn('is_default');
                }
            });
        }
    }
}
