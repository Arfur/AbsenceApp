<?php
/**
 * =========================================================
 * Project   : ki-admin - v1.0.0
 * File Name : SidebarMenuComposer.php
 * 
 * Author    : Michael Battle
 * Created On: 09/Aug/2025
 * Updated On: 09/Aug/2025
 * 
 * Description:
 * View composer for automatic sidebar menu injection across all views.
 * Integrates with the corrected MenuBuilder helper to provide user-specific
 * menu data without requiring manual controller injection.
 * 
 * Origin:
 * Step 4 of MenuBuilder Helper implementation - View Composer creation
 * 
 * Changes:
 * - Created SidebarMenuComposer for automatic menu injection
 * - Integrated with corrected user-specific MenuBuilder
 * - Added error handling and fallback menu support
 * - Implemented caching for performance optimization
 * =========================================================
 */

namespace App\Http\View\Composers;

use Illuminate\View\View;
use Illuminate\Support\Facades\Auth;
use Illuminate\Support\Facades\Cache;
use Illuminate\Support\Facades\Log;
use App\Helpers\MenuBuilder;
use App\Models\User;

class SidebarMenuComposer
{
    /**
     * Bind data to the view.
     *
     * @param View $view
     * @return void
     */
    public function compose(View $view): void
    {
        try {
            $user = Auth::user();
            
            if (!$user) {
                // No authenticated user - provide guest menu or empty array
                $sidebarMenu = $this->getGuestMenu();
                $menuStats = [
                    'total_items' => 0,
                    'user_specific_items' => 0,
                    'is_authenticated' => false
                ];
            } else {
                // Get user-specific menu with caching
                $cacheKey = "sidebar_menu_user_{$user->user_id}";
                
                $sidebarMenu = Cache::remember($cacheKey, 300, function () use ($user) {
                    return MenuBuilder::getSidebarMenu();
                });
                
                // Get menu statistics for debugging
                $menuStats = $this->getMenuStats($user);
                
                // DEBUG STEP 1: Output granted menu IDs for user (for dashboard view)
                $debugStep1 = '';
                if (request()->routeIs('dashboard')) {
                    $grantedIds = \App\Models\UserMenuItem::where('user_id', $user->user_id)
                        ->where('is_granted', true)
                        ->orderBy('custom_order')
                        ->pluck('menu_item_id')
                        ->toArray();
                    $debugStep1 = '<div style="background:#eef; color:#222; padding:8px; margin:8px 0; border:1px solid #99c;">'
                        . '<strong>DEBUG STEP 1:</strong> Granted Menu IDs for User ' . $user->user_id . ':<br>'
                        . implode(', ', $grantedIds)
                        . '</div>';
                }
            }
            
            // Share sidebarMenu globally for debug output in dashboard
            \View::share('sidebarMenu', $sidebarMenu);
            $view->with([
                'sidebarMenu' => $sidebarMenu,
                'menuStats' => $menuStats,
                'currentUser' => $user,
                'debugStep1' => $debugStep1
            ]);
            
        } catch (\Exception $e) {
            Log::error('SidebarMenuComposer error: ' . $e->getMessage());
            
            // Provide fallback menu on error
            $view->with([
                'sidebarMenu' => $this->getFallbackMenu(),
                'menuStats' => [
                    'total_items' => 0,
                    'user_specific_items' => 0,
                    'is_authenticated' => false,
                    'error' => true
                ],
                'currentUser' => null
            ]);
        }
    }
    
    /**
     * Get menu for guest (non-authenticated) users
     *
     * @return array
     */
    private function getGuestMenu(): array
    {
        return [
            [
                'id' => 'guest_login',
                'title' => 'Login',
                'url' => route('sign_in'),
                'icon' => 'ph-sign-in',
                'children' => []
            ],
            [
                'id' => 'guest_register',
                'title' => 'Register',
                'url' => route('sign_up'),
                'icon' => 'ph-user-plus',
                'children' => []
            ]
        ];
    }
    
    /**
     * Get fallback menu when there's an error
     *
     * @return array
     */
    private function getFallbackMenu(): array
    {
        return [
            [
                'id' => 'fallback_dashboard',
                'title' => 'Dashboard',
                'url' => route('index'),
                'icon' => 'ph-house',
                'children' => []
            ]
        ];
    }
    
    /**
     * Get menu statistics for debugging and monitoring
     *
     * @param User $user
     * @return array
     */
    private function getMenuStats(User $user): array
    {
        try {
            $userMenuItemsCount = \App\Models\UserMenuItem::where('user_id', $user->user_id)
                ->where('is_granted', true)
                ->count();
                
            $roleMenuItemsCount = \App\Models\RoleMenuItem::where('role_type_id', $user->role_type_id)
                ->where('is_granted', true)
                ->where('status', 'active')
                ->count();
                
            return [
                'total_items' => $userMenuItemsCount,
                'user_specific_items' => $userMenuItemsCount,
                'role_template_items' => $roleMenuItemsCount,
                'is_authenticated' => true,
                'user_role' => $user->roleType ? $user->roleType->name : 'No Role',
                'cache_enabled' => true
            ];
            
        } catch (\Exception $e) {
            Log::warning('MenuStats error: ' . $e->getMessage());
            
            return [
                'total_items' => 0,
                'user_specific_items' => 0,
                'is_authenticated' => true,
                'error' => true
            ];
        }
    }
    
    /**
     * Clear menu cache for a specific user
     *
     * @param int $userId
     * @return void
     */
    public static function clearUserMenuCache(int $userId): void
    {
        $cacheKey = "sidebar_menu_user_{$userId}";
        Cache::forget($cacheKey);
        
        Log::info("SidebarMenuComposer: Cleared menu cache for user {$userId}");
    }
    
    /**
     * Clear all menu caches
     *
     * @return void
     */
    public static function clearAllMenuCaches(): void
    {
        // Get all users and clear their caches
        $userIds = \App\Models\User::pluck('user_id');
        
        foreach ($userIds as $userId) {
            self::clearUserMenuCache($userId);
        }
        
        Log::info("SidebarMenuComposer: Cleared all menu caches");
    }
}
