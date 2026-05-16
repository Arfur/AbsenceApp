<?php
/**
 * =========================================================
 * Project   : ki-admin - v1.0.0
 * File Name : MenuBuilder.php
 * 
 * Author    : Michael Battle
 * Created On: 09/Aug/2025
 * Updated On: 09/Aug/2025
 * 
 * Description:
 * Builds a hierarchical sidebar menu based on role-based permissions
 * and user-specific grants. Combines role_menu_items and user_menu_items
 * to create dynamic menus for authenticated users.
 * 
 * Key Features:
 * - Role-based menu permissions from role_menu_items
 * - User-specific overrides from user_menu_items
 * - Hierarchical menu structure building
 * - Proper ordering and visibility controls
 * - Fallback to default menus when no permissions exist
 * 
 * Adapted from: support-hub MenuBuilder with role-based enhancements
 * =========================================================
 */

namespace App\Helpers;

use Illuminate\Support\Facades\Auth;
use Illuminate\Support\Facades\Log;
use App\Models\MenuItem;
use App\Models\UserMenuItem;
use App\Models\RoleMenuItem;
use App\Models\User;

class MenuBuilder
{
    /**
     * Get allowed menu tree for authenticated user.
     * Combines role-based permissions and user-specific grants.
     *
     * @return array
     */
    public static function getSidebarMenu(): array
    {
        $user = Auth::user();
        if (!$user) {
            Log::info('MenuBuilder - No authenticated user found');
            return [];
        }

        return self::buildMenuForUser($user);
    }

    /**
     * Get menu structure for a specific user (useful for testing)
     *
     * @param User|int $user
     * @return array
     */
    public static function getMenuForUser($user): array
    {
        if (is_numeric($user)) {
            $user = User::findOrFail($user);
        }

        return self::buildMenuForUser($user);
    }

    /**
     * Build menu for a specific user without changing authentication context
     * Uses only user_menu_items table (not role-based permissions)
     *
     * @param User $user
     * @return array
     */
    private static function buildMenuForUser(User $user): array
    {
        Log::info('MenuBuilder - Building menu for user: ' . $user->email . ' (user_id: ' . $user->user_id . ')');

        /* =====================================================
         * SECTION 1: Fetch Granted Menu IDs from UserMenuItem
         * -----------------------------------------------------
         * Pull all menu_item_id values for which this user has
         * is_granted = true, ordering by custom_order.
         * ===================================================== */
        $grantedIds = UserMenuItem::where('user_id', $user->user_id)
            ->where('is_granted', true)
            ->orderBy('custom_order')
            ->pluck('menu_item_id')
            ->toArray();

        Log::info('MenuBuilder - User-specific granted IDs count: ' . count($grantedIds));
        Log::info('MenuBuilder - First 10 granted IDs: ' . json_encode(array_slice($grantedIds, 0, 10)));

        // Recursively collect all granted descendants
        $allGrantedIds = self::collectAllDescendantIds($grantedIds);

        // DEBUG STEP 1: Output granted menu IDs for user
        $debugIds = $grantedIds;
        \View::share('debugStep1', '<div style="background:#eef; color:#222; padding:8px; margin:8px 0; border:1px solid #99c;">'
            . '<strong>DEBUG STEP 1:</strong> Granted Menu IDs for User ' . $user->user_id . ':<br>'
            . implode(', ', $debugIds)
            . '</div>');

        // DEBUG STEP 2: Output granted menu titles for user (expanded set)
        $grantedMenuTitles = [];
        if (!empty($allGrantedIds)) {
            $grantedMenuTitles = \App\Models\MenuItem::whereIn('id', $allGrantedIds)->pluck('title')->toArray();
        }
        \View::share('grantedMenuNames', $grantedMenuTitles);

        /* =====================================================
         * SECTION 2: Load Menu Items
         * -----------------------------------------------------
         * If no explicit grants, load default top-level 'main' 
         * menus; otherwise load only the granted items.
         * ===================================================== */
        if (empty($grantedIds)) {
            Log::info('MenuBuilder - Using fallback menu (no granted IDs)');
            $menuItems = MenuItem::whereNull('parent_id')
                ->where('menu_group', 'main')
                ->where('is_visible', true)
                ->orderBy('order')
                ->get();
        } else {
            Log::info('MenuBuilder - Loading all granted menu items and descendants');
            $menuItems = MenuItem::whereIn('id', $allGrantedIds)
                ->where('is_visible', true)
                ->orderBy('parent_id')
                ->orderBy('order')
                ->get();
            Log::info('MenuBuilder - Loaded menu items with all descendants, count: ' . $menuItems->count());
        }

        return self::buildTreeFromItems($menuItems);

    }

    /**
     * Recursively collect all descendant IDs for a set of parent IDs
     * @param array $parentIds
     * @return array
     */
    private static function collectAllDescendantIds(array $parentIds): array
    {
        $allIds = $parentIds;
        $queue = $parentIds;
        while (!empty($queue)) {
            $children = MenuItem::whereIn('parent_id', $queue)->pluck('id')->toArray();
            $newChildren = array_diff($children, $allIds);
            if (empty($newChildren)) {
                break;
            }
            $allIds = array_merge($allIds, $newChildren);
            $queue = $newChildren;
        }
        return array_unique($allIds);
    }

    /**
     * Build hierarchical tree from menu items collection
     *
     * @param \Illuminate\Database\Eloquent\Collection $menuItems
     * @return array
     */
    private static function buildTreeFromItems($menuItems): array
    {
        $allItems = [];
        foreach ($menuItems as $item) {
            $allItems[$item->id] = [
                'id'          => $item->id,
                'title'       => $item->title,
                'slug'        => $item->slug,
                'url'         => $item->url,
                'route_name'  => $item->route_name,
                'icon'        => $item->icon,
                'parent_id'   => $item->parent_id,
                'order'       => $item->order,
                'is_external' => $item->is_external,
                'menu_group'  => $item->menu_group,
                'children'    => [],
            ];
        }

        // Recursively build tree
        $tree = [];
        foreach ($allItems as $item) {
            if (!$item['parent_id'] || !isset($allItems[$item['parent_id']])) {
                $tree[$item['id']] = self::attachChildren($item, $allItems);
            }
        }

        uasort($tree, function($a, $b) {
            return $a['order'] <=> $b['order'];
        });

        return array_values($tree);
    }

    /**
     * Recursively attach children to a menu item
     * @param array $item
     * @param array $allItems
     * @return array
     */
    private static function attachChildren(array $item, array $allItems): array
    {
        $children = [];
        foreach ($allItems as $child) {
            if ($child['parent_id'] === $item['id']) {
                $children[] = self::attachChildren($child, $allItems);
            }
        }
        usort($children, function($a, $b) {
            return $a['order'] <=> $b['order'];
        });
        $item['children'] = $children;
        return $item;
    }

    /**
     * Check if user has access to a specific menu item
     *
     * @param int $menuItemId
     * @param User|null $user
     * @return bool
     */
    public static function hasMenuAccess(int $menuItemId, User $user = null): bool
    {
        $user = $user ?? Auth::user();
        
        if (!$user) {
            return false;
        }

        return $user->hasMenuAccess($menuItemId);
    }

    /**
     * Get flat list of accessible menu item IDs for current user
     *
     * @return array
     */
    public static function getAccessibleMenuIds(): array
    {
        $user = Auth::user();
        if (!$user) {
            return [];
        }

        $menu = self::getSidebarMenu();
        $ids = [];

        self::extractMenuIds($menu, $ids);

        return $ids;
    }

    /**
     * Recursively extract menu IDs from menu tree
     *
     * @param array $menuItems
     * @param array &$ids
     */
    private static function extractMenuIds(array $menuItems, array &$ids): void
    {
        foreach ($menuItems as $item) {
            $ids[] = $item['id'];
            
            if (!empty($item['children'])) {
                self::extractMenuIds($item['children'], $ids);
            }
        }
    }

    /**
     * Generate breadcrumb trail for a menu item
     *
     * @param int $menuItemId
     * @return array
     */
    public static function getBreadcrumb(int $menuItemId): array
    {
        $breadcrumb = [];
        $menuItem = MenuItem::find($menuItemId);

        while ($menuItem) {
            array_unshift($breadcrumb, [
                'id' => $menuItem->id,
                'title' => $menuItem->title,
                'url' => $menuItem->url,
                'route_name' => $menuItem->route_name,
            ]);

            $menuItem = $menuItem->parent;
        }

        return $breadcrumb;
    }
}
