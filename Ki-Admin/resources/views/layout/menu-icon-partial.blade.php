{{--
    =========================================================
    Menu Icon Partial
    Handles rendering of menu icons (FontAwesome classes or SVG sprites)
    ========================================================= 
--}}
@if(!empty($item['icon']))
    @php
        // support both FontAwesome class strings and sprite ids
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
