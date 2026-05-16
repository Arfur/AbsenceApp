<?php

namespace App\Providers;

use Illuminate\Support\ServiceProvider;
use Illuminate\Support\Facades\View;
use App\Http\View\Composers\SidebarMenuComposer;

class AppServiceProvider extends ServiceProvider
{
    /**
     * Register any application services.
     */
    public function register(): void
    {
        //
    }

    /**
     * Bootstrap any application services.
     */
    public function boot(): void
    {
        // Register SidebarMenuComposer for automatic menu injection
        View::composer([
            'layout.master',           // Main layout template
            'layout.sidebar',          // Sidebar component
            'partial.sidebar',         // Partial sidebar views
            'components.sidebar',      // Sidebar components
            'dashboard.dashboard',     // Main dashboard view
            'test.*'                   // Test views for development
        ], SidebarMenuComposer::class);
    }
}
