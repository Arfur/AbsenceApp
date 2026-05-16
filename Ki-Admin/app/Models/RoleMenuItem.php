<?php
/**
 * =========================================================
 * Project   : ki-admin - v1.0.0
 * File Name : RoleMenuItem.php
 * Author    : Michael Battle
 * Created On: 09/Aug/2025
 * Description: RoleMenuItem model for role-based menu permissions
 * Origin: Role-based menu system implementation
 * =========================================================
 */

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;
use Illuminate\Database\Eloquent\Relations\BelongsTo;

class RoleMenuItem extends Model
{
    use HasFactory;

    protected $table = 'role_menu_items';

    protected $fillable = [
        'role_type_id',
        'role_type',
        'menu_item_id',
        'is_granted',
        'assigned_at',
        'status',
        'notes',
    ];

    protected $casts = [
        'role_type_id' => 'integer',
        'menu_item_id' => 'integer',
        'is_granted' => 'boolean',
        'assigned_at' => 'datetime',
    ];

    /**
     * Get the menu item for this permission
     */
    public function menuItem(): BelongsTo
    {
        return $this->belongsTo(MenuItem::class);
    }

    /**
     * Scope: Get only granted permissions
     */
    public function scopeGranted($query)
    {
        return $query->where('is_granted', true);
    }

    /**
     * Scope: Get only active permissions
     */
    public function scopeActive($query)
    {
        return $query->where('status', 'active');
    }

    /**
     * Scope: Get permissions for a specific role type
     */
    public function scopeForRoleType($query, $roleType)
    {
        return $query->where('role_type', $roleType);
    }

    /**
     * Scope: Get permissions for a specific role ID
     */
    public function scopeForRole($query, $roleId)
    {
        return $query->where('role_type_id', $roleId);
    }

    /**
     * Scope: Get permissions for a specific menu item
     */
    public function scopeForMenuItem($query, $menuItemId)
    {
        return $query->where('menu_item_id', $menuItemId);
    }

    /**
     * Check if this permission is currently active and granted
     */
    public function isActivelyGranted(): bool
    {
        return $this->is_granted && $this->status === 'active';
    }

    /**
     * Get menu items for a specific role type
     */
    public static function getMenuItemsForRoleType($roleType, $onlyActive = true)
    {
        $query = self::with('menuItem')
                    ->forRoleType($roleType)
                    ->granted();
        
        if ($onlyActive) {
            $query->active();
        }
        
        return $query->get()->pluck('menuItem');
    }

    /**
     * Get hierarchical menu items for a specific role type
     */
    public static function getHierarchicalMenuForRoleType($roleType, $onlyActive = true)
    {
        $menuItems = self::getMenuItemsForRoleType($roleType, $onlyActive);
        $menuItemIds = $menuItems->pluck('id')->toArray();
        
        return MenuItem::with(['children' => function ($query) use ($menuItemIds) {
            $query->whereIn('id', $menuItemIds)->visible()->ordered();
        }])
        ->whereIn('id', $menuItemIds)
        ->visible()
        ->topLevel()
        ->ordered()
        ->get();
    }

    /**
     * Grant menu access to role type
     */
    public static function grantAccessToRoleType($roleType, $menuItemId, $roleId = null, $notes = null)
    {
        return self::updateOrCreate(
            [
                'role_type' => $roleType,
                'menu_item_id' => $menuItemId,
                'role_type_id' => $roleId,
            ],
            [
                'is_granted' => true,
                'assigned_at' => now(),
                'status' => 'active',
                'notes' => $notes,
            ]
        );
    }

    /**
     * Revoke menu access from role type
     */
    public static function revokeAccessFromRoleType($roleType, $menuItemId, $roleId = null, $notes = null)
    {
        return self::updateOrCreate(
            [
                'role_type' => $roleType,
                'menu_item_id' => $menuItemId,
                'role_type_id' => $roleId,
            ],
            [
                'is_granted' => false,
                'status' => 'revoked',
                'notes' => $notes,
            ]
        );
    }

    /**
     * Check if a role type has access to a specific menu item
     */
    public static function hasAccess($roleType, $menuItemId): bool
    {
        return self::forRoleType($roleType)
                  ->forMenuItem($menuItemId)
                  ->granted()
                  ->active()
                  ->exists();
    }

    /**
     * Get all accessible menu item IDs for a role type
     */
    public static function getAccessibleMenuIds($roleType): array
    {
        return self::forRoleType($roleType)
                  ->granted()
                  ->active()
                  ->pluck('menu_item_id')
                  ->toArray();
    }
}
