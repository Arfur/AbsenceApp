<?php
/**
 * =========================================================
 * Project   : ki-admin - v1.0.0
 * File Name : UserMenuItem.php
 * Author    : Michael Battle
 * Created On: 09/Aug/2025
 * Description: UserMenuItem pivot model for user-specific menu permissions
 * Origin: Role-based menu system implementation
 * =========================================================
 */

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;
use Illuminate\Database\Eloquent\Relations\BelongsTo;

class UserMenuItem extends Model
{
    use HasFactory;

    protected $table = 'user_menu_items';

    protected $fillable = [
        'user_id',
        'menu_item_id',
        'is_granted',
        'custom_order',
        'is_custom',
    ];

    protected $casts = [
        'user_id' => 'integer',
        'menu_item_id' => 'integer',
        'is_granted' => 'boolean',
        'custom_order' => 'integer',
        'is_custom' => 'boolean',
    ];

    /**
     * Get the user that owns this menu permission
     */
    public function user(): BelongsTo
    {
        return $this->belongsTo(User::class, 'user_id', 'user_id');
    }

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
     * Scope: Get only custom permissions
     */
    public function scopeCustom($query)
    {
        return $query->where('is_custom', true);
    }

    /**
     * Scope: Get permissions for a specific user
     */
    public function scopeForUser($query, $userId)
    {
        return $query->where('user_id', $userId);
    }

    /**
     * Scope: Get permissions for a specific menu item
     */
    public function scopeForMenuItem($query, $menuItemId)
    {
        return $query->where('menu_item_id', $menuItemId);
    }

    /**
     * Check if this permission is currently granted
     */
    public function isGranted(): bool
    {
        return $this->is_granted;
    }

    /**
     * Get user's granted menu items
     */
    public static function getUserMenuItems($userId, $onlyGranted = true)
    {
        $query = self::with('menuItem')
                    ->forUser($userId);
        
        if ($onlyGranted) {
            $query->granted();
        }
        
        return $query->get()->pluck('menuItem');
    }

    /**
     * Grant menu access to user
     */
    public static function grantAccess($userId, $menuItemId, $customOrder = null)
    {
        return self::updateOrCreate(
            [
                'user_id' => $userId,
                'menu_item_id' => $menuItemId,
            ],
            [
                'is_granted' => true,
                'custom_order' => $customOrder,
                'is_custom' => true,
            ]
        );
    }

    /**
     * Revoke menu access from user
     */
    public static function revokeAccess($userId, $menuItemId)
    {
        return self::updateOrCreate(
            [
                'user_id' => $userId,
                'menu_item_id' => $menuItemId,
            ],
            [
                'is_granted' => false,
                'is_custom' => true,
            ]
        );
    }
}
