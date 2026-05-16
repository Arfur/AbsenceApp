<?php
/**
 * =========================================================
 * Project   : ki-admin - v1.0.0
 * File Name : MenuItem.php
 * Author    : Michael Battle
 * Created On: 09/Aug/2025
 * Description: MenuItem Eloquent model for dynamic menu system
 * Origin: Role-based menu system implementation
 * =========================================================
 */

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;
use Illuminate\Database\Eloquent\Relations\HasMany;
use Illuminate\Database\Eloquent\Relations\BelongsTo;
use Illuminate\Database\Eloquent\Relations\BelongsToMany;

class MenuItem extends Model
{
    use HasFactory;

    protected $table = 'menu_items';

    protected $fillable = [
        'title',
        'slug',
        'is_visible',
        'url',
        'route_name',
        'icon',
        'order',
        'is_external',
        'parent_id',
        'display_order',
        'menu_group',
    ];

    protected $casts = [
        'is_visible' => 'boolean',
        'is_external' => 'boolean',
        'order' => 'integer',
        'parent_id' => 'integer',
        'display_order' => 'integer',
    ];

    /**
     * Get the parent menu item
     */
    public function parent(): BelongsTo
    {
        return $this->belongsTo(MenuItem::class, 'parent_id');
    }

    /**
     * Get all child menu items
     */
    public function children(): HasMany
    {
        return $this->hasMany(MenuItem::class, 'parent_id')->orderBy('display_order');
    }

    /**
     * Get all descendant menu items (recursive)
     */
    public function descendants(): HasMany
    {
        return $this->children()->with('descendants');
    }

    /**
     * Get users who have access to this menu item
     */
    public function users(): BelongsToMany
    {
        return $this->belongsToMany(User::class, 'user_menu_items')
                    ->withPivot(['is_granted', 'custom_order', 'is_custom'])
                    ->withTimestamps();
    }

    /**
     * Get role menu items for this menu item
     */
    public function roleMenuItems(): HasMany
    {
        return $this->hasMany(RoleMenuItem::class);
    }

    /**
     * Scope: Get only visible menu items
     */
    public function scopeVisible($query)
    {
        return $query->where('is_visible', true);
    }

    /**
     * Scope: Get only top-level menu items (no parent)
     */
    public function scopeTopLevel($query)
    {
        return $query->whereNull('parent_id');
    }

    /**
     * Scope: Get items by menu group
     */
    public function scopeByGroup($query, $group)
    {
        return $query->where('menu_group', $group);
    }

    /**
     * Scope: Order by display order
     */
    public function scopeOrdered($query)
    {
        return $query->orderBy('display_order')->orderBy('order');
    }

    /**
     * Get hierarchical menu structure
     */
    public static function getHierarchical($group = 'main')
    {
        return self::with(['children' => function ($query) {
            $query->visible()->ordered();
        }])
        ->visible()
        ->topLevel()
        ->byGroup($group)
        ->ordered()
        ->get();
    }

    /**
     * Check if menu item has children
     */
    public function hasChildren(): bool
    {
        return $this->children()->exists();
    }

    /**
     * Get full breadcrumb path
     */
    public function getBreadcrumb(): array
    {
        $breadcrumb = [];
        $current = $this;
        
        while ($current) {
            array_unshift($breadcrumb, $current);
            $current = $current->parent;
        }
        
        return $breadcrumb;
    }

    /**
     * Get the URL for this menu item
     */
    public function getUrlAttribute($value): ?string
    {
        if ($this->route_name) {
            try {
                return route($this->route_name);
            } catch (\Exception $e) {
                return $value;
            }
        }
        
        return $value;
    }

    /**
     * Check if this menu item is accessible by a specific role type
     */
    public function isAccessibleByRole(string $roleType): bool
    {
        return $this->roleMenuItems()
                    ->where('role_type', $roleType)
                    ->where('is_granted', true)
                    ->where('status', 'active')
                    ->exists();
    }

    /**
     * Check if this menu item is accessible by a specific user
     */
    public function isAccessibleByUser(User $user): bool
    {
        return $this->users()
                    ->where('users.id', $user->id)
                    ->wherePivot('is_granted', true)
                    ->wherePivot('status', 'active')
                    ->exists();
    }
}
