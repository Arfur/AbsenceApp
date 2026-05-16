{{--
/**
 * =========================================================
 * Project   : ki-admin - v1.0.0
 * File Name : sidebar.blade.php
 * 
 * Author    : Michael Battle
 * Created On: 05/Aug/2025
 * Updated On: 05/Aug/2025
 * 
 * Description:
 * Main navigation sidebar with role-based dynamic menu generation.
 * Displays different menu items based on user's role permissions.
 * 
 * Origin:
 * Main layout sidebar for ki-admin Support Hub
 * 
 * Changes:
 * - Implemented dynamic menu generation using MenuService
 * - Added role-based menu filtering and section titles
 * - Replaced static menu with configurable menu system
 * - Added fallback menu for users without proper roles
 * =========================================================
 */
--}}

<!-- Menu Navigation starts -->
<nav>
    {{-- =========================================================
        Section: Inline SVG Sprite Fallback (chg0254)
        Description: Inline a hidden copy of the project's SVG sprite so
        that <use xlink:href="/assets/svg/_sprite.svg#id"> will resolve
        locally even if external referencing is blocked (CORS/CSP).
        This is a small, reversible change for robustness and diagnostics.
    ========================================================= --}}
    <div style="width:0;height:0;overflow:hidden;position:absolute;" aria-hidden="true">
        {!! file_get_contents(public_path('assets/svg/_sprite.svg')) !!}
    </div>
     {{-- =========================================================
         Section: App Logo
         Description: Displays the application logo and toggle button
         ========================================================= --}}
     <div class="app-logo">
        <a class="logo d-inline-block" href="{{route('index')}}">
            <img alt="#" src="{{asset('../assets/images/logo/1.png')}}">
        </a>

        <span class="bg-light-primary toggle-semi-nav d-flex-center">
                <i class="ti ti-chevron-right"></i>
            </span>

          {{-- =========================================================
              Section: User Profile
              Description: Displays user avatar, name, and profile menu
              ========================================================= --}}
          <div class="d-flex align-items-center nav-profile p-3">
                <span class="h-45 w-45 d-flex-center b-r-10 position-relative bg-danger m-auto">
                    <img alt="avatar" class="img-fluid b-r-10" src="{{asset('../assets/images/avatar/woman.jpg')}}">
                    <span class="position-absolute top-0 end-0 p-1 bg-success border border-light rounded-circle"></span>
                </span>
                {{-- =========================================================
                    Section: User Info
                    Description: Shows username and role display name
                    ========================================================= --}}
                <div class="flex-grow-1 ps-2">
                <h6 class="text-primary mb-0">{{ auth()->user()->username ?? explode('@', auth()->user()->email)[0] ?? 'Unknown User' }}</h6>
                <p class="text-muted f-s-12 mb-0">{{ optional(auth()->user()->roleType)->display_name ?? 'User' }}</p>
            </div>


                {{-- =========================================================
                    Section: Profile Menu Dropdown
                    Description: Dropdown for profile, settings, incognito, logout
                    ========================================================= --}}
                <div class="dropdown profile-menu-dropdown">
                <a aria-expanded="false" data-bs-auto-close="true" data-bs-placement="top" data-bs-toggle="dropdown"
                   role="button">
                    <i class="ti ti-settings fs-5"></i>
                </a>
                <ul class="dropdown-menu">
                    <li class="dropdown-item">
                        <a class="f-w-500" href="{{route('profile')}}" target="_blank">
                            <i class="ph-duotone  ph-user-circle pe-1 f-s-20"></i> Profile Details
                        </a>
                    </li>
                    <li class="dropdown-item">
                        <a class="f-w-500" href="{{route('setting')}}" target="_blank">
                            <i class="ph-duotone  ph-gear pe-1 f-s-20"></i> Settings
                        </a>
                    </li>
                    <li class="dropdown-item">
                        <div class="d-flex align-items-center justify-content-between">
                            <div>
                                <a class="f-w-500" href="#">
                                    <i class="ph-duotone  ph-detective pe-1 f-s-20"></i> Incognito
                                </a>
                            </div>
                            <div class="flex-shrink-0">
                                <div class="form-check form-switch">
                                    <input class="form-check-input form-check-primary" id="incognitoSwitch"
                                           type="checkbox">
                                </div>
                            </div>
                        </div>
                    </li>
                    <li class="dropdown-item">
                        <a class="mb-0 text-secondary f-w-500" href="{{route('sign_up')}}" target="_blank">
                            <i class="ph-bold  ph-plus pe-1 f-s-20"></i> Add account
                        </a>
                    </li>

                    <li class="app-divider-v dotted py-1"></li>

                    <li class="dropdown-item">
                        <a class="mb-0 text-danger" href="{{route('sign_in')}}" target="_blank">
                            <i class="ph-duotone  ph-sign-out pe-1 f-s-20"></i> Log Out
                        </a>
                    </li>
                </ul>
            </div>

        </div>
    </div>
    
    {{-- =========================================================
       Section: Sidebar Navigation
       Description: Main sidebar navigation and dynamic menu rendering
       ========================================================= --}}
    <div class="app-nav" id="app-simple-bar">
        @php
            $activeMenuId = null;
            $findActiveMenuId = function($items) use (&$findActiveMenuId) {
                foreach ($items as $item) {
                    if (!empty($item['route_name']) && Route::currentRouteName() === $item['route_name']) {
                        return $item['id'];
                    }
                    if (!empty($item['url']) && Request::is(ltrim($item['url'], '/'))) {
                        return $item['id'];
                    }
                    if (!empty($item['children'])) {
                        $childActive = $findActiveMenuId($item['children']);
                        if ($childActive) return $childActive;
                    }
                }
                return null;
            };
            $activeMenuId = $findActiveMenuId($sidebarMenu);
            // Enhanced sidebar debug output
            $expansionDebugFile = storage_path('sidebar_debug.txt');
            $routeName = Route::currentRouteName();
            $debug = "Sidebar Expansion Debug\n";
            $debug .= "Current Route: {$routeName}\n";
            $debug .= "Active Menu ID: {$activeMenuId}\n";
            foreach ($sidebarMenu as $item) {
                $isExpanded = ($item['id'] == $activeMenuId);
                $debug .= "Menu: [{$item['id']}] {$item['title']} | Expanded: " . ($isExpanded ? 'YES' : 'NO') . "\n";
                if (!empty($item['children'])) {
                    foreach ($item['children'] as $child) {
                        $childExpanded = isset($child['id']) && $child['id'] == $activeMenuId;
                        $debug .= "  Submenu: [" . ($child['id'] ?? '-') . "] " . ($child['title'] ?? '-') . " | Expanded: " . ($childExpanded ? 'YES' : 'NO') . "\n";
                    }
                }
            }
            file_put_contents($expansionDebugFile, $debug, LOCK_EX);
        @endphp
                {{-- =========================================================
                    Section: Active Menu Context (chg0238)
                    Description: Pass $activeMenuId to JS via data-active-menu-id attribute
                    ========================================================= --}}
                @php
                    $activeMenuId = null;
                    $findActiveMenuId = function($items) use (&$findActiveMenuId) {
                        foreach ($items as $item) {
                            if (!empty($item['route_name']) && Route::currentRouteName() === $item['route_name']) {
                                return $item['id'];
                            }
                            if (!empty($item['url']) && Request::is(ltrim($item['url'], '/'))) {
                                return $item['id'];
                            }
                            if (!empty($item['children'])) {
                                $childActive = $findActiveMenuId($item['children']);
                                if ($childActive) return $childActive;
                            }
                        }
                        return null;
                    };
                    $activeMenuId = $findActiveMenuId($sidebarMenu);
                @endphp
                <ul id="sidebarAccordion" class="main-nav p-0 mt-2" data-active-menu-id="{{ $activeMenuId }}">
                @foreach($sidebarMenu as $menuItem)
                    @include('layout.sidebar-menu-item', ['item' => $menuItem, 'activeMenuId' => $activeMenuId])
                @endforeach
                </ul>
    </div>
</nav>
<script>
// =========================================================
// chg0259: Robust client-side active menu discovery and expansion
// Finds the best matching sidebar link for the current URL, marks its
// <li> active and expands any ancestor .collapse containers so icons show.
document.addEventListener('DOMContentLoaded', function () {
    try {
        var anchors = Array.from(document.querySelectorAll('.main-nav a[href]'));
        if (!anchors.length) return;

        var current = window.location.pathname.replace(/\/$/, ''); // normalize

        // Find best match: exact pathname, then prefix match, then href contains
        var best = null;
        function norm(href){ try { var u = new URL(href, window.location.origin); return u.pathname.replace(/\/$/, ''); } catch (e) { return href; } }
        anchors.forEach(function(a){
            var p = norm(a.getAttribute('href'));
            if (!p) return;
            if (p === current) {
                best = a;
            }
        });
        if (!best) {
            // prefix match (longest)
            var longest = 0;
            anchors.forEach(function(a){
                var p = norm(a.getAttribute('href'));
                if (current.indexOf(p) === 0 && p.length > longest) { longest = p.length; best = a; }
            });
        }
        if (!best) {
            // fallback: contains
            anchors.forEach(function(a){ if (a.href && a.href.indexOf(location.href) !== -1) best = a; });
        }

        if (!best) return;

        // Add active on the matching li and expand ancestor collapses
        var li = best.closest('li');
        if (li) li.classList.add('active');

        var parent = li ? li.parentElement : null;
        while (parent && parent !== document.body) {
            if (parent.classList && parent.classList.contains('collapse')) {
                parent.classList.add('show');
                // if the collapse has a toggler anchor, ensure aria-expanded true
                var id = parent.id;
                if (id) {
                    var toggler = document.querySelector('[data-bs-toggle="collapse"][href="#'+id+'"], [data-bs-target="#'+id+'"]');
                    if (toggler) toggler.setAttribute('aria-expanded', 'true');
                }
            }
            // if parent is an li, mark it active as well (so parents show highlight)
            if (parent.tagName === 'LI') parent.classList.add('active');
            parent = parent.parentElement;
        }
    } catch (e) {
        // silent
        console.error('sidebar expand error', e);
    }
});
</script>
<!-- Menu Navigation ends -->
