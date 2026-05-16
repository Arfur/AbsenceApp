
{{--
    =========================================================
    Sidebar Menu Item Partial (Dynamic)
    Refactored to match static sample sidebar markup and logic
    - Uses 'another-level' for items with children, 'no-sub' for leaf items
    - Bootstrap collapse attributes for submenus
    - SVG icons, badges, and submenu structure consistent with sample
    - Unlimited nesting via recursion
    - Detailed comments for maintainability
    =========================================================
--}}

@if(isset($item['menu_group']) && $item['menu_group'] === 'section_title')
    {{-- Section Title (matches sample sidebar) --}}
    <li class="menu-title">
        <span>{{ $item['title'] ?? 'Section' }}</span>
    </li>
@else
    {{-- Main Menu Item --}}
    @php
        // Enhanced debug output for menu/submenu rendering
        $menuItemDebugFile = storage_path('sidebar_menuitem_debug.txt');
        $collapseId = !empty($item['children']) ? 'menu_' . $item['id'] : null;
        $menuUrl = '#';
        if (!empty($item['url'])) {
            $menuUrl = $item['url'];
        } elseif (!empty($item['route_name'])) {
            try {
                $menuUrl = route($item['route_name']);
            } catch (\Exception $e) {
                $menuUrl = route('dashboard');
            }
        }
        $debugLine = "MenuItem Render: [{$item['id']}] '" . ($item['title'] ?? '-') . "' | Level: " . ($level ?? 0) .
            " | activeMenuId: " . ($activeMenuId ?? '-') .
            " | isActive: " . ((isset($activeMenuId) && $activeMenuId === $item['id']) ? 'YES' : 'NO') .
            " | collapseId: " . ($collapseId ?? '-') .
            " | url: " . ($item['url'] ?? '-') .
            " | route_name: " . ($item['route_name'] ?? '-') .
            " | menuUrl: " . ($menuUrl ?? '-') . "\n";
        file_put_contents($menuItemDebugFile, $debugLine, FILE_APPEND | LOCK_EX);

        // Blade debug output for collapse state
        $htmlDebugFile = storage_path('sidebar_html_debug.txt');
        if ($collapseId) {
            $collapseClass = ((isset($activeMenuId) && $activeMenuId === $item['id']) ? 'collapse show' : 'collapse');
            $ariaExpanded = ((isset($activeMenuId) && $activeMenuId === $item['id']) ? 'true' : 'false');
            $htmlDebugLine = "HTML Render: [{$item['id']}] '" . ($item['title'] ?? '-') . "' | collapseId: {$collapseId} | class: {$collapseClass} | aria-expanded: {$ariaExpanded}\n";
            file_put_contents($htmlDebugFile, $htmlDebugLine, FILE_APPEND | LOCK_EX);
        }
    @endphp
    @php
            $hasChildren = !empty($item['children']);
            $liClass = $hasChildren ? 'another-level' : 'no-sub';
            $menuUrl = '#';
            if (!empty($item['url'])) {
                $menuUrl = $item['url'];
            } elseif (!empty($item['route_name'])) {
                try {
                    $menuUrl = route($item['route_name']);
                } catch (\Exception $e) {
                    $menuUrl = route('dashboard');
                }
            }
            $collapseId = $hasChildren ? 'menu_' . $item['id'] : null;
            // =========================================================
            // Section: Active Menu Context (chg0229)
            // Description: Expand only menu matching $activeMenuId
            // =========================================================
            $isActive = isset($activeMenuId) && $activeMenuId === $item['id'];
    @endphp
    <li class="{{ $liClass }} {{ $isActive ? 'active' : '' }}" style="border-left:none;">
        <a
            href="{{ $hasChildren ? '#' . $collapseId : $menuUrl }}"
            @if($hasChildren)
                    data-bs-toggle="collapse" aria-expanded="{{ $isActive ? 'true' : 'false' }}"
            @endif
            style="font-size: 1rem;"
        >
            <!--
                * =========================================================
                * Change Ref: chg0252
                * Added On : 28/Aug/2025
                * Description: Render menu icon from `menu_items.icon` for
                *              all menu levels. Use asset() for robust URL
                *              resolution and provide a minimal placeholde
                *              when no icon is present.
                * ========================================================= -->
            @if(!empty($item['icon']))
                @php
                    // chg0255: support both FontAwesome class strings and sprite ids
                    $iconVal = trim($item['icon']);
                    // detect FontAwesome class tokens like "fas fa-...", "far fa-...", "fab fa-..."
                    $isFa = preg_match('/\bfa[brs]?[\b-]?/', $iconVal) || preg_match('/\bfas\b|\bfar\b|\bfab\b/', $iconVal);
                @endphp

                @if($isFa)
                    <span class="menu-icon d-inline-block me-2 align-middle">
                        <i class="{{ $iconVal }} f-s-16" aria-hidden="true"></i>
                    </span>
                @else
                    <span class="menu-icon d-inline-block me-2 align-middle">
                        <svg class="icon" stroke="currentColor" stroke-width="1.5" style="width:1.25em;height:1.25em;vertical-align:middle;">
                            <use href="#{{ $iconVal }}" xlink:href="#{{ $iconVal }}"></use>
                        </svg>
                    </span>
                @endif
            @else
                {{-- Placeholder to keep alignment when icon missing --}}
                <span class="menu-icon-placeholder d-inline-block me-2 align-middle" style="width:1.25em;height:1.25em;display:inline-block;"></span>
            @endif
            {{ $item['title'] ?? 'Menu Item' }}
            {{-- Badge (if present, matches sample sidebar) --}}
            @if(!empty($item['badge']))
                <span class="badge {{ $item['badge_class'] ?? 'bg-danger badge-dashboard badge-notification ms-2' }}">{{ $item['badge'] }}</span>
            @endif
        </a>
        {{-- Submenu (recursive, matches sample markup) --}}
        @if($hasChildren)
                <!--
                    * =========================================================
                    * Section: Bootstrap Accordion Logic (chg0225)
                    * Section: Active Menu Context (chg0229)
                    * Description:
                    * Enforce single menu expansion using Bootstrap accordion
                    * Expand only menu matching $activeMenuId
                    * Adds data-bs-parent="#sidebarAccordion" to collapsible <ul>
                    * ========================================================= -->
                <ul class="collapse{{ $isActive ? ' show' : '' }}" id="{{ $collapseId }}" data-bs-parent="#sidebarAccordion">
                @foreach($item['children'] as $child)
                        @include('layout.sidebar-menu-item', ['item' => $child, 'level' => ($level ?? 0) + 1, 'activeMenuId' => $activeMenuId])
                @endforeach
            </ul>
        @endif
    </li>
@endif
