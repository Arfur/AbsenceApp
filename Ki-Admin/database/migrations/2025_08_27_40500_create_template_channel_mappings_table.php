<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

/**
 * =========================================================
 * Project   : Support Hub - v1.0.0
 * File Name : 2025_07_08_002167_create_template_channel_mappings_table.php
 * 
 * Author    : Michael Battle
 * Created On: 08/July/2025
 * Updated On: 08/July/2025
 * 
 * Description:
 * Maps notification templates to delivery channels. Supports 
 * multi-channel broadcasting, fallback logic, and admin control.
 * 
 * Origin:
 * Notifications group. Used in template routing, delivery 
 * configuration, and channel prioritization.
 * 
 * Changes:
 * - Implemented agreed field structure
 * - Composite uniqueness constraint applied
 * - Included timestamps and audit metadata
 * =========================================================
 */

return new class extends Migration
{
    /**
     * =========================================================
     * Section: Create Template Channel Mappings Table
     * Description: Links templates to delivery channels with priority and activation flags
     * =========================================================
     */
    public function up(): void
    {
        Schema::create('template_channel_mappings', function (Blueprint $table) {
            $table->id();

            $table->foreignId('template_id')->constrained('notification_templates')->onDelete('cascade');
            $table->foreignId('channel_id')->constrained('template_channels')->onDelete('cascade');

            $table->boolean('is_enabled')->default(true);     // Toggle for activation
            $table->integer('priority_level')->default(0);    // Fallback or sorting
            $table->text('notes')->nullable();                // Commentary or audit trail

            $table->timestamps();

            $table->unique(['template_id', 'channel_id']);
        });
    }

    /**
     * =========================================================
     * Section: Drop Template Channel Mappings Table
     * Description: Reverts template-to-channel routing structure
     * =========================================================
     */
    public function down(): void
    {
        Schema::dropIfExists('template_channel_mappings');
    }
};
