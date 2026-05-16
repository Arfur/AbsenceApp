<?php

/**
 * =========================================================
 * Project   : ki-admin - v1.0.0
 * File Name : RoleType.php
 * 
 * Author    : Michael Battle
 * Created On: 04/Aug/2025
 * Updated On: 04/Aug/2025
 * 
 * Description:
 * RoleType model for managing user roles and permissions.
 * Handles role hierarchy and system role protections.
 * 
 * Origin:
 * Role management system for Support Hub
 * 
 * Changes:
 * - Created RoleType model with relationships
 * - Added role hierarchy methods
 * - Implemented system role protections
 * =========================================================
 */

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;

class RoleType extends Model
{
    use HasFactory;

    /**
     * The attributes that are mass assignable.
     *
     * @var array
     */
    protected $fillable = [
        'name',
        'display_name',
        'description',
        'is_system_role',
        'is_default',
        'priority',
    ];

    /**
     * The attributes that should be cast.
     *
     * @var array
     */
    protected $casts = [
        'is_system_role' => 'boolean',
        'is_default' => 'boolean',
        'priority' => 'integer',
    ];

    /* =========================================================
     * Section: Relationships
     * ========================================================= */

    /**
     * Users with this role type
     */
    public function users()
    {
        return $this->hasMany(User::class, 'role_type_id');
    }

    /* =========================================================
     * Section: Query Scopes
     * ========================================================= */

    /**
     * Get system roles
     */
    public function scopeSystemRoles($query)
    {
        return $query->where('is_system_role', true);
    }

    /**
     * Get non-system roles
     */
    public function scopeCustomRoles($query)
    {
        return $query->where('is_system_role', false);
    }

    /**
     * Get default role
     */
    public function scopeDefault($query)
    {
        return $query->where('is_default', true);
    }

    /**
     * Order by priority (highest first)
     */
    public function scopeByPriority($query)
    {
        return $query->orderBy('priority', 'desc');
    }

    /* =========================================================
     * Section: Helper Methods
     * ========================================================= */

    /**
     * Check if this role has higher priority than another
     */
    public function hasHigherPriorityThan(RoleType $otherRole): bool
    {
        return $this->priority > $otherRole->priority;
    }

    /**
     * Check if this role can be deleted
     */
    public function canBeDeleted(): bool
    {
        return !$this->is_system_role && !$this->is_default && $this->users()->count() === 0;
    }

    /**
     * Get the default role type
     */
    public static function getDefault(): ?RoleType
    {
        return static::where('is_default', true)->first();
    }

    /**
     * Get role by name
     */
    public static function getByName(string $name): ?RoleType
    {
        return static::where('name', $name)->first();
    }
}
