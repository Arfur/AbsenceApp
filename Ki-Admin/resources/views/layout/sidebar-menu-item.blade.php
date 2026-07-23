
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
        // consider ancestor chain if provided by the composer
        $activeMenuAncestors = $activeMenuAncestors ?? [];

        $debugLine = "MenuItem Render: [{$item['id']}] '" . ($item['title'] ?? '-') . "' | Level: " . ($level ?? 0) .
            " | activeMenuId: " . ($activeMenuId ?? '-') .
            " | isActive: " . ((isset($activeMenuId) && ($activeMenuId === $item['id'] || in_array($item['id'], $activeMenuAncestors))) ? 'YES' : 'NO') .
            " | collapseId: " . ($collapseId ?? '-') .
            " | url: " . ($item['url'] ?? '-') .
            " | route_name: " . ($item['route_name'] ?? '-') .
            " | menuUrl: " . ($menuUrl ?? '-') . "\n";
        file_put_contents($menuItemDebugFile, $debugLine, FILE_APPEND | LOCK_EX);

        // Blade debug output for collapse state
        $htmlDebugFile = storage_path('sidebar_html_debug.txt');
        if ($collapseId) {
            $isActiveAncestorOrSelf = (isset($activeMenuId) && ($activeMenuId === $item['id'] || in_array($item['id'], $activeMenuAncestors)));
            $collapseClass = $isActiveAncestorOrSelf ? 'collapse show' : 'collapse';
            $ariaExpanded = $isActiveAncestorOrSelf ? 'true' : 'false';
            $htmlDebugLine = "HTML Render: [{$item['id']}] '" . ($item['title'] ?? '-') . "' | collapseId: {$collapseId} | class: {$collapseClass} | aria-expanded: {$ariaExpanded}\n";
            file_put_contents($htmlDebugFile, $htmlDebugLine, FILE_APPEND | LOCK_EX);
        }
    @endphp
    @php
            // Ensure level is defined (top-level if not passed)
            $level = $level ?? 0;
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
        // Description: Expand menu if it's the exact active item OR an ancestor
        // composer provides $activeMenuId and $activeMenuAncestors
        // =========================================================
        $activeMenuAncestors = $activeMenuAncestors ?? [];
        $isCurrentItem = isset($activeMenuId) && $activeMenuId === $item['id'];
        $isAncestor = isset($activeMenuId) && in_array($item['id'], $activeMenuAncestors);
        $isActive = $isCurrentItem || $isAncestor;

        // Only use the accordion parent for top-level groups to avoid nested
        // collapses closing unrelated menus. This keeps accordion behavior
        // scoped to the first level.
        $useAccordionParent = ($level === 0);
    @endphp
    <li class="{{ $liClass }} {{ $isCurrentItem ? 'active' : (isset($isAncestor) && $isAncestor ? 'active-parent' : '') }}" style="border-left:none;">
        <a href="{{ $menuUrl }}" 
           @if($hasChildren) data-bs-toggle="collapse" data-bs-target="#{{ $collapseId }}" aria-expanded="{{ $isActive ? 'true' : 'false' }}" @endif
           style="font-size: 1rem;">
            @include('layout.menu-icon-partial', ['item' => $item])
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
    <ul class="collapse{{ $isActive ? ' show' : '' }}" id="{{ $collapseId }}" @if($useAccordionParent) data-bs-parent="#sidebarAccordion" @endif>
                @foreach($item['children'] as $child)
            @include('layout.sidebar-menu-item', ['item' => $child, 'level' => ($level ?? 0) + 1, 'activeMenuId' => $activeMenuId, 'activeMenuAncestors' => $activeMenuAncestors])
                @endforeach
            </ul>
        @endif
    </li>
@endif
