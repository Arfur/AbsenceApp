<?php
/**
 * =========================================================
 * Project   : ki-admin - v1.0.0
 * File Name : User.php
 * 
 * Author    : Michael Battle
 * Created On: 03/Aug/2025
 * Updated On: 09/Aug/2025
 * 
 * Description:
 * User model with role-based functionality and email verification.
 * Supports user, admin, super_admin, and support_agent roles.
 * 
 * Origin:
 * Core authentication model for the Support Hub system
 * 
 * Changes:
 * - Added role field and role-based methods
 * - Implemented email verification
 * - Added role scopes and helper methods
 * - Updated OTP generation to 6 digits
 * - Added menu system relationships
 * =========================================================
 */

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Foundation\Auth\User as Authenticatable;
use Illuminate\Notifications\Notifiable;
use Illuminate\Database\Eloquent\Relations\BelongsTo;
use Illuminate\Database\Eloquent\Relations\HasMany;
use Illuminate\Database\Eloquent\Relations\BelongsToMany;

class User extends Authenticatable
{
    /** @use HasFactory<\Database\Factories\UserFactory> */
    use HasFactory, Notifiable;

    /**
     * Boot the model and auto-assign user_id
     */
    protected static function boot()
    {
        parent::boot();
        
        static::creating(function ($user) {
            if (empty($user->user_id)) {
                // Get the next user_id by finding the max and adding 1
                $maxUserId = static::max('user_id') ?: 0;
                $user->user_id = $maxUserId + 1;
            }
        });
    }

    /**
     * The attributes that are mass assignable.
     *
     * @var array<int, string>
     */
    protected $fillable = [
        'user_id',
        'name',
        'email',
        'password',
        'email_verified_at',
        'email_verification_token',
        'role_type_id',
        'otp_code',
        'otp_expires_at',
    ];

    /**
     * The attributes that should be hidden for serialization.
     *
     * @var array<int, string>
     */
    protected $hidden = [
        'password',
        'remember_token',
        'email_verification_token',
        'otp_code',
    ];

    /**
     * Get the attributes that should be cast.
     *
     * @return array<string, string>
     */
    protected function casts(): array
    {
        return [
            'email_verified_at' => 'datetime',
            'password' => 'hashed',
            'otp_expires_at' => 'datetime',
            'role_type_id' => 'integer',
        ];
    }

    /* =========================================================
     * Section: Relationships
     * Description: Eloquent relationships for User model
     * ========================================================= */

    /**
     * Get the role type for this user
     */
    public function roleType(): BelongsTo
    {
        return $this->belongsTo(RoleType::class, 'role_type_id');
    }

    /**
     * Get the menu items this user has access to
     */
    public function menuItems(): BelongsToMany
    {
        return $this->belongsToMany(MenuItem::class, 'user_menu_items', 'user_id', 'menu_item_id', 'user_id', 'id')
                    ->withPivot(['is_granted', 'custom_order', 'is_custom'])
                    ->withTimestamps();
    }

    /**
     * Get the user's granted menu items
     */
    public function grantedMenuItems(): BelongsToMany
    {
        return $this->menuItems()
                    ->wherePivot('is_granted', true);
    }

    /**
     * Get user menu items pivot records
     */
    public function userMenuItems(): HasMany
    {
        return $this->hasMany(UserMenuItem::class, 'user_id', 'user_id');
    }

    /**
     * Get role-based menu items for this user
     */
    public function roleMenuItems()
    {
        return RoleMenuItem::where('role_type_id', $this->role_type_id);
    }

    /**
     * Get all available menu items for this user (combines role + individual grants)
     */
    public function availableMenuItems()
    {
        // Get role-based menu items
        $roleMenuItemIds = RoleMenuItem::where('role_type_id', $this->role_type_id)
                                      ->where('is_granted', true)
                                      ->pluck('menu_item_id')
                                      ->toArray();

        // Get user-specific granted menu items  
        $userMenuItemIds = $this->userMenuItems()
                                ->where('is_granted', true)
                                ->pluck('menu_item_id')
                                ->toArray();

        // Combine and get unique menu item IDs
        $menuItemIds = array_unique(array_merge($roleMenuItemIds, $userMenuItemIds));

        return MenuItem::whereIn('id', $menuItemIds)
                      ->where('is_visible', true)
                      ->orderBy('order')
                      ->get();
    }

    /**
     * Check if user has access to a specific menu item
     */
    public function hasMenuAccess(int $menuItemId): bool
    {
        // Check role-based access first
        $roleAccess = RoleMenuItem::where('role_type_id', $this->role_type_id)
                                 ->where('menu_item_id', $menuItemId)
                                 ->where('is_granted', true)
                                 ->exists();

        if ($roleAccess) {
            return true;
        }

        // Check user-specific access
        return $this->userMenuItems()
                   ->where('menu_item_id', $menuItemId)
                   ->where('is_granted', true)
                   ->exists();
    }

    /**
     * Get hierarchical menu structure for this user
     */
    public function getMenuStructure(): array
    {
        $menuItems = $this->availableMenuItems();
        return $this->buildMenuTree($menuItems);
    }

    /**
     * Build hierarchical menu tree from flat menu items
     */
    private function buildMenuTree($menuItems): array
    {
        $menuTree = [];
        $menuItemsArray = $menuItems->keyBy('id')->toArray();

        foreach ($menuItems as $item) {
            if ($item->parent_id === null) {
                $menuTree[] = $this->buildMenuBranch($item, $menuItemsArray);
            }
        }

        return $menuTree;
    }

    /**
     * Build menu branch recursively
     */
    private function buildMenuBranch($item, array $allItems): array
    {
        $branch = is_object($item) ? $item->toArray() : $item;
        $branch['children'] = [];

        foreach ($allItems as $childItem) {
            if ($childItem['parent_id'] === $branch['id']) {
                $branch['children'][] = $this->buildMenuBranch($childItem, $allItems);
            }
        }

        return $branch;
    }

    /* =========================================================
     * Section: Status and Role Management Methods
     * Description: Helper methods for status and role-based functionality
     * ========================================================= */

    /**
     * Check if user is active
     */
    public function isActive(): bool
    {
        return true; // Implement your own logic
    }

    /**
     * Check if user has a specific role
     */
    public function hasRole(string $role): bool
    {
        return $this->roleType && $this->roleType->name === $role;
    }

    /**
     * Check if user is admin or higher
     */
    public function isAdminOrHigher(): bool
    {
        return in_array($this->role_type_id, [2, 3]); // admin, super_admin
    }

    /**
     * Check if user is super admin
     */
    public function isSuperAdmin(): bool
    {
        return $this->role_type_id === 3;
    }

    /* =========================================================
     * Section: Scopes
     * Description: Query scopes for filtering users
     * ========================================================= */

    /**
     * Scope: Active users only
     */
    public function scopeActive($query)
    {
        return $query; // Implement your own logic
    }

    /**
     * Scope: Users with specific role
     */
    public function scopeWithRole($query, $roleTypeId)
    {
        return $query->where('role_type_id', $roleTypeId);
    }

    /* =========================================================
     * Section: Email Verification Methods
     * Description: Email verification and OTP functionality
     * ========================================================= */

    /**
     * Generate and save OTP code for verification
     */
    public function generateOtpCode(): string
    {
        $otp = str_pad(random_int(0, 999999), 6, '0', STR_PAD_LEFT);
        
        $this->update([
            'otp_code' => $otp,
            'otp_expires_at' => now()->addMinutes(15),
        ]);
        
        return $otp;
    }

    /**
     * Verify the provided OTP code
     */
    public function verifyOtpCode(string $code): bool
    {
        if (!$this->otp_code || !$this->otp_expires_at) {
            return false;
        }

        if ($this->otp_expires_at->isPast()) {
            return false;
        }

        return $this->otp_code === $code;
    }

    /**
     * Mark user as verified and clear OTP
     */
    public function markAsVerified(): void
    {
        $this->update([
            'email_verified_at' => now(),
            'email_verification_token' => null,
            'otp_code' => null,
            'otp_expires_at' => null,
        ]);
    }

    /**
     * Clear OTP data
     */
    public function clearOtpCode(): void
    {
        $this->update([
            'otp_code' => null,
            'otp_expires_at' => null,
        ]);
    }

    /**
     * Assign default menu items to user based on their role
     * Copies role-based templates to user-specific permissions
     * 
     * @return void
     */
    public function assignDefaultMenuItems(): void
    {
        // Get the role type slug for this user
        $roleTypeSlug = $this->roleType ? $this->roleType->slug : null;
        
        if (!$roleTypeSlug) {
            \Log::warning('User assignDefaultMenuItems: No role type found for user ' . $this->id);
            return;
        }

        \Log::info('User assignDefaultMenuItems: Processing for user ' . $this->id . ' with role ' . $roleTypeSlug);

        // Get all granted menu items for this role type
        $roleMenuItems = RoleMenuItem::where('role_type_id', $this->role_type_id)
            ->where('is_granted', true)
            ->where('status', 'active')
            ->get(); // Get full records, not just IDs

        \Log::info('User assignDefaultMenuItems: Found ' . $roleMenuItems->count() . ' role-based menu items');

        if ($roleMenuItems->isEmpty()) {
            \Log::warning('User assignDefaultMenuItems: No role menu items found for role_type_id ' . $this->role_type_id);
            return;
        }

        $assignedCount = 0;
        
        // Assign each granted menu item to the user
        foreach ($roleMenuItems as $roleMenuItem) {
            try {
                \Log::debug("Processing menu_item_id: {$roleMenuItem->menu_item_id}");
                
                $userMenuItem = UserMenuItem::updateOrCreate(
                    [
                        'user_id' => $this->user_id, // Use user_id, not id!
                        'menu_item_id' => $roleMenuItem->menu_item_id
                    ],
                    [
                        'is_granted' => true,
                        'custom_order' => $roleMenuItem->order ?? 0
                    ]
                );
                
                if ($userMenuItem->wasRecentlyCreated) {
                    $assignedCount++;
                    \Log::debug("Created new UserMenuItem for menu_item_id: {$roleMenuItem->menu_item_id}");
                } else {
                    \Log::debug("Updated existing UserMenuItem for menu_item_id: {$roleMenuItem->menu_item_id}");
                }
                
            } catch (\Exception $e) {
                \Log::error("Error creating UserMenuItem for menu_item_id {$roleMenuItem->menu_item_id}: " . $e->getMessage());
            }
        }

        \Log::info('User assignDefaultMenuItems: Assigned ' . $assignedCount . ' new menu items for user ' . $this->user_id);
    }
}
