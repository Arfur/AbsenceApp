-- =============================================================================
-- File    : Phase8_GlobalSettingsMenuSeed.sql
-- Version : 1.0.0
-- Created : 2026-05-12
-- -----------------------------------------------------------------------------
-- Purpose : Inserts the Phase 8 global-settings sidebar menu structure into
--           the `menuitemsglobalconfig` table.
--
--           Hierarchy (ItemType values):
--             'category' → top-level sidebar section header
--             'menu'     → clickable group (accordion or direct link)
--             'submenu'  → leaf link under a menu group
--
--           The ID range is calculated at runtime from MAX(Id) + 1000,
--           rounded up to the next 1000-boundary, so this script is safe to
--           run on any DB state.
--
-- Categories added:
--   Dashboard, Apps, Widgets, Component, Advance UI, Icons,
--   Map & Charts, Table & Forms, Pages
--
-- Menu groups added:
--   Dashboard (direct), Apps (direct), Widgets (direct),
--   UI Kits (accordion under Component),
--   Advance UI (accordion), Fontawesome (direct),
--   Map & Charts (accordion), Table & Forms (accordion),
--   Pages (direct → existing /global-settings/pages route)
--
-- Submenus added: see per-group sections below.
--
-- Destructive statements: NONE. INSERT IGNORE only.
-- Schema changes: NONE.
-- =============================================================================

USE absenceapp;

-- =============================================================================
-- STEP 0 — Determine safe ID base
-- @base is rounded up to the next 1000-boundary above MAX(Id) + 1000.
-- This guarantees no collision with any existing rows.
-- =============================================================================

SET @base = (SELECT COALESCE(MAX(Id), 0) FROM menuitemsglobalconfig);
SET @base = (FLOOR(@base / 1000) + 1) * 1000;

-- =============================================================================
-- STEP 1 — Declare all IDs
-- =============================================================================

-- ── Categories (ParentId = NULL) ─────────────────────────────────────────────
SET @cat_dashboard    = @base + 100;
SET @cat_apps         = @base + 200;
SET @cat_widgets      = @base + 300;
SET @cat_component    = @base + 400;
SET @cat_advance_ui   = @base + 500;
SET @cat_icons        = @base + 600;
SET @cat_map_charts   = @base + 700;
SET @cat_table_forms  = @base + 800;
SET @cat_pages        = @base + 900;

-- ── Menu groups (ParentId = category) ────────────────────────────────────────
SET @menu_dashboard   = @base + 110;   -- direct link to /global-settings/dashboard
SET @menu_apps        = @base + 210;   -- direct link to /global-settings/apps
SET @menu_widgets     = @base + 310;   -- direct link to /global-settings/widgets
SET @menu_uikits      = @base + 410;   -- accordion group (UI Kits)
SET @menu_advance_ui  = @base + 510;   -- accordion group (Advance UI)
SET @menu_fontawesome = @base + 610;   -- direct link to /global-settings/icons/fontawesome
SET @menu_map_charts  = @base + 710;   -- accordion group (Map & Charts)
SET @menu_table_forms = @base + 810;   -- accordion group (Table & Forms)
SET @menu_pages       = @base + 910;   -- direct link to /global-settings/pages (existing page)

-- ── UI Kits submenus (ParentId = @menu_uikits) ───────────────────────────────
SET @s_cheatsheet     = @base + 411;
SET @s_alert          = @base + 412;
SET @s_badges         = @base + 413;
SET @s_buttons        = @base + 414;   -- points to existing /global-settings/buttons
SET @s_cards          = @base + 415;
SET @s_dropdown       = @base + 416;
SET @s_grid           = @base + 417;
SET @s_avatar         = @base + 418;
SET @s_tabs           = @base + 419;
SET @s_accordions     = @base + 420;
SET @s_progress       = @base + 421;
SET @s_notifications  = @base + 422;
SET @s_lists          = @base + 423;
SET @s_helper         = @base + 424;
SET @s_background     = @base + 425;
SET @s_divider        = @base + 426;
SET @s_ribbons        = @base + 427;
SET @s_editor         = @base + 428;

-- ── Advance UI submenus (ParentId = @menu_advance_ui) ────────────────────────
SET @s_modals         = @base + 511;
SET @s_offcanvas      = @base + 512;
SET @s_sweat          = @base + 513;
SET @s_scrollbar      = @base + 514;
SET @s_spinners       = @base + 515;
SET @s_animation      = @base + 516;
SET @s_video          = @base + 517;
SET @s_tour           = @base + 518;
SET @s_slider         = @base + 519;
SET @s_bsslider       = @base + 520;
SET @s_scrolly        = @base + 521;
SET @s_tooltip        = @base + 522;
SET @s_rating         = @base + 523;
SET @s_prismjs        = @base + 524;
SET @s_countdown      = @base + 525;
SET @s_countup        = @base + 526;
SET @s_draggable      = @base + 527;
SET @s_treeview       = @base + 528;
SET @s_blockui        = @base + 529;

-- ── Map & Charts submenus (ParentId = @menu_map_charts) ──────────────────────
SET @s_map            = @base + 711;
SET @s_charts         = @base + 712;

-- ── Table & Forms submenus (ParentId = @menu_table_forms) ────────────────────
SET @s_basictable     = @base + 811;
SET @s_darktable      = @base + 812;
SET @s_borderedtable  = @base + 813;

-- =============================================================================
-- STEP 2 — Validation preview (no writes)
-- Uncomment to verify ID range before running inserts.
-- =============================================================================
-- SELECT @base AS id_base,
--        @cat_dashboard AS cat_dashboard, @cat_pages AS cat_pages,
--        @menu_uikits AS menu_uikits, @menu_advance_ui AS menu_advance_ui;

-- =============================================================================
-- STEP 3 — INSERT categories
-- ItemType = 'category', ParentId = NULL, no Route
-- =============================================================================

INSERT IGNORE INTO menuitemsglobalconfig
    (Id, ItemType, Label, Icon, Route, ParentId, SortOrder, IsHidden, IsFlat, Description, CreatedAt, UpdatedAt)
VALUES
    (@cat_dashboard,   'category', 'Dashboard',    'bi-speedometer2',    NULL, NULL, @cat_dashboard,   0, 0, 'Phase 8 — Dashboard category',    NOW(), NOW()),
    (@cat_apps,        'category', 'Apps',         'bi-app',             NULL, NULL, @cat_apps,        0, 0, 'Phase 8 — Apps category',         NOW(), NOW()),
    (@cat_widgets,     'category', 'Widgets',      'bi-grid-1x2',        NULL, NULL, @cat_widgets,     0, 0, 'Phase 8 — Widgets category',      NOW(), NOW()),
    (@cat_component,   'category', 'Component',    'bi-grid-3x3-gap',    NULL, NULL, @cat_component,   0, 0, 'Phase 8 — Component category',    NOW(), NOW()),
    (@cat_advance_ui,  'category', 'Advance UI',   'bi-lightning',       NULL, NULL, @cat_advance_ui,  0, 0, 'Phase 8 — Advance UI category',   NOW(), NOW()),
    (@cat_icons,       'category', 'Icons',        'bi-emoji-smile',     NULL, NULL, @cat_icons,       0, 0, 'Phase 8 — Icons category',        NOW(), NOW()),
    (@cat_map_charts,  'category', 'Map & Charts', 'bi-bar-chart-line',  NULL, NULL, @cat_map_charts,  0, 0, 'Phase 8 — Map & Charts category', NOW(), NOW()),
    (@cat_table_forms, 'category', 'Table & Forms','bi-table',           NULL, NULL, @cat_table_forms, 0, 0, 'Phase 8 — Table & Forms category',NOW(), NOW()),
    (@cat_pages,       'category', 'Pages',        'bi-file-earmark',    NULL, NULL, @cat_pages,       0, 0, 'Phase 8 — Pages category',        NOW(), NOW());

-- =============================================================================
-- STEP 4 — INSERT menu groups
-- ItemType = 'menu', ParentId = category Id
-- Groups with Route = direct link; groups without Route = accordion
-- =============================================================================

INSERT IGNORE INTO menuitemsglobalconfig
    (Id, ItemType, Label, Icon, Route, ParentId, SortOrder, IsHidden, IsFlat, Description, CreatedAt, UpdatedAt)
VALUES
    -- Direct-link menus (Route is set; no submenus needed)
    (@menu_dashboard,   'menu', 'Dashboard',    'bi-speedometer2', '/global-settings/dashboard',         @cat_dashboard,   @menu_dashboard,   0, 1, 'Dashboard home page',               NOW(), NOW()),
    (@menu_apps,        'menu', 'Apps',         'bi-app',          '/global-settings/apps',              @cat_apps,        @menu_apps,        0, 1, 'Apps overview page',                NOW(), NOW()),
    (@menu_widgets,     'menu', 'Widgets',      'bi-grid-1x2',     '/global-settings/widgets',           @cat_widgets,     @menu_widgets,     0, 1, 'Widgets overview page',             NOW(), NOW()),
    (@menu_fontawesome, 'menu', 'Fontawesome',  'bi-bootstrap',    '/global-settings/icons/fontawesome', @cat_icons,       @menu_fontawesome, 0, 1, 'Fontawesome icon library page',     NOW(), NOW()),
    (@menu_pages,       'menu', 'Pages',        'bi-file-earmark', '/global-settings/pages',             @cat_pages,       @menu_pages,       0, 1, 'Pages reference (existing)',         NOW(), NOW()),

    -- Accordion menus (no Route; items added in STEP 5–8)
    (@menu_uikits,      'menu', 'UI Kits',      'bi-grid-3x3-gap-fill', NULL, @cat_component,   @menu_uikits,      0, 0, 'UI Kits component group',           NOW(), NOW()),
    (@menu_advance_ui,  'menu', 'Advance UI',   'bi-lightning',         NULL, @cat_advance_ui,  @menu_advance_ui,  0, 0, 'Advance UI feature group',          NOW(), NOW()),
    (@menu_map_charts,  'menu', 'Map & Charts', 'bi-bar-chart-line',    NULL, @cat_map_charts,  @menu_map_charts,  0, 0, 'Map & Charts group',                NOW(), NOW()),
    (@menu_table_forms, 'menu', 'Table & Forms','bi-table',             NULL, @cat_table_forms, @menu_table_forms, 0, 0, 'Table & Forms group',               NOW(), NOW());

-- =============================================================================
-- STEP 5 — INSERT UI Kits submenus
-- ItemType = 'submenu', ParentId = @menu_uikits
-- Note: Buttons points to the existing /global-settings/buttons route.
-- =============================================================================

INSERT IGNORE INTO menuitemsglobalconfig
    (Id, ItemType, Label, Icon, Route, ParentId, SortOrder, IsHidden, IsFlat, CreatedAt, UpdatedAt)
VALUES
    (@s_cheatsheet,    'submenu', 'Cheatsheet',    'bi-journal-code',         '/global-settings/ui-kits/cheatsheet',    @menu_uikits, @s_cheatsheet,    0, 0, NOW(), NOW()),
    (@s_alert,         'submenu', 'Alert',         'bi-exclamation-triangle',  '/global-settings/ui-kits/alert',         @menu_uikits, @s_alert,         0, 0, NOW(), NOW()),
    (@s_badges,        'submenu', 'Badges',        'bi-tag',                   '/global-settings/ui-kits/badges',        @menu_uikits, @s_badges,        0, 0, NOW(), NOW()),
    (@s_buttons,       'submenu', 'Buttons',       'bi-stop-btn',              '/global-settings/buttons',               @menu_uikits, @s_buttons,       0, 0, NOW(), NOW()),
    (@s_cards,         'submenu', 'Cards',         'bi-card-heading',          '/global-settings/ui-kits/cards',         @menu_uikits, @s_cards,         0, 0, NOW(), NOW()),
    (@s_dropdown,      'submenu', 'Dropdown',      'bi-chevron-down',          '/global-settings/ui-kits/dropdown',      @menu_uikits, @s_dropdown,      0, 0, NOW(), NOW()),
    (@s_grid,          'submenu', 'Grid',          'bi-grid',                  '/global-settings/ui-kits/grid',          @menu_uikits, @s_grid,          0, 0, NOW(), NOW()),
    (@s_avatar,        'submenu', 'Avatar',        'bi-person-circle',         '/global-settings/ui-kits/avatar',        @menu_uikits, @s_avatar,        0, 0, NOW(), NOW()),
    (@s_tabs,          'submenu', 'Tabs',          'bi-folder',                '/global-settings/ui-kits/tabs',          @menu_uikits, @s_tabs,          0, 0, NOW(), NOW()),
    (@s_accordions,    'submenu', 'Accordions',    'bi-layout-text-sidebar',   '/global-settings/ui-kits/accordions',    @menu_uikits, @s_accordions,    0, 0, NOW(), NOW()),
    (@s_progress,      'submenu', 'Progress',      'bi-hourglass-split',       '/global-settings/ui-kits/progress',      @menu_uikits, @s_progress,      0, 0, NOW(), NOW()),
    (@s_notifications, 'submenu', 'Notifications', 'bi-bell',                  '/global-settings/ui-kits/notifications', @menu_uikits, @s_notifications, 0, 0, NOW(), NOW()),
    (@s_lists,         'submenu', 'Lists',         'bi-list-ul',               '/global-settings/ui-kits/lists',         @menu_uikits, @s_lists,         0, 0, NOW(), NOW()),
    (@s_helper,        'submenu', 'Helper Classes','bi-tools',                 '/global-settings/ui-kits/helper-classes',@menu_uikits, @s_helper,        0, 0, NOW(), NOW()),
    (@s_background,    'submenu', 'Background',    'bi-image',                 '/global-settings/ui-kits/background',    @menu_uikits, @s_background,    0, 0, NOW(), NOW()),
    (@s_divider,       'submenu', 'Divider',       'bi-dash-lg',               '/global-settings/ui-kits/divider',       @menu_uikits, @s_divider,       0, 0, NOW(), NOW()),
    (@s_ribbons,       'submenu', 'Ribbons',       'bi-bookmark-star',         '/global-settings/ui-kits/ribbons',       @menu_uikits, @s_ribbons,       0, 0, NOW(), NOW()),
    (@s_editor,        'submenu', 'Editor',        'bi-pencil-square',         '/global-settings/ui-kits/editor',        @menu_uikits, @s_editor,        0, 0, NOW(), NOW());

-- =============================================================================
-- STEP 6 — INSERT Advance UI submenus
-- ItemType = 'submenu', ParentId = @menu_advance_ui
-- =============================================================================

INSERT IGNORE INTO menuitemsglobalconfig
    (Id, ItemType, Label, Icon, Route, ParentId, SortOrder, IsHidden, IsFlat, CreatedAt, UpdatedAt)
VALUES
    (@s_modals,    'submenu', 'Modals',             'bi-window',                   '/global-settings/advance-ui/modals',            @menu_advance_ui, @s_modals,    0, 0, NOW(), NOW()),
    (@s_offcanvas, 'submenu', 'Offcanvas Toggle',   'bi-layout-sidebar',           '/global-settings/advance-ui/offcanvas-toggle',  @menu_advance_ui, @s_offcanvas, 0, 0, NOW(), NOW()),
    (@s_sweat,     'submenu', 'Sweat Alert',        'bi-patch-exclamation',        '/global-settings/advance-ui/sweat-alert',       @menu_advance_ui, @s_sweat,     0, 0, NOW(), NOW()),
    (@s_scrollbar, 'submenu', 'Scrollbar',          'bi-arrows-expand',            '/global-settings/advance-ui/scrollbar',         @menu_advance_ui, @s_scrollbar, 0, 0, NOW(), NOW()),
    (@s_spinners,  'submenu', 'Spinners',           'bi-arrow-repeat',             '/global-settings/advance-ui/spinners',          @menu_advance_ui, @s_spinners,  0, 0, NOW(), NOW()),
    (@s_animation, 'submenu', 'Animation',          'bi-stars',                    '/global-settings/advance-ui/animation',         @menu_advance_ui, @s_animation, 0, 0, NOW(), NOW()),
    (@s_video,     'submenu', 'Video Embed',        'bi-play-circle',              '/global-settings/advance-ui/video-embed',       @menu_advance_ui, @s_video,     0, 0, NOW(), NOW()),
    (@s_tour,      'submenu', 'Tour',               'bi-map',                      '/global-settings/advance-ui/tour',              @menu_advance_ui, @s_tour,      0, 0, NOW(), NOW()),
    (@s_slider,    'submenu', 'Slider',             'bi-sliders',                  '/global-settings/advance-ui/slider',            @menu_advance_ui, @s_slider,    0, 0, NOW(), NOW()),
    (@s_bsslider,  'submenu', 'Bootstrap Slider',  'bi-sliders2',                 '/global-settings/advance-ui/bootstrap-slider',  @menu_advance_ui, @s_bsslider,  0, 0, NOW(), NOW()),
    (@s_scrolly,   'submenu', 'Scrolly',            'bi-arrow-down-circle',        '/global-settings/advance-ui/scrolly',           @menu_advance_ui, @s_scrolly,   0, 0, NOW(), NOW()),
    (@s_tooltip,   'submenu', 'Tooltip & Popovers', 'bi-chat-right-text',          '/global-settings/advance-ui/tooltip-popovers',  @menu_advance_ui, @s_tooltip,   0, 0, NOW(), NOW()),
    (@s_rating,    'submenu', 'Rating',             'bi-star',                     '/global-settings/advance-ui/rating',            @menu_advance_ui, @s_rating,    0, 0, NOW(), NOW()),
    (@s_prismjs,   'submenu', 'Prismjs',            'bi-code-slash',               '/global-settings/advance-ui/prismjs',           @menu_advance_ui, @s_prismjs,   0, 0, NOW(), NOW()),
    (@s_countdown, 'submenu', 'Count Down',         'bi-stopwatch',                '/global-settings/advance-ui/count-down',        @menu_advance_ui, @s_countdown, 0, 0, NOW(), NOW()),
    (@s_countup,   'submenu', 'Count Up',           'bi-hourglass',                '/global-settings/advance-ui/count-up',          @menu_advance_ui, @s_countup,   0, 0, NOW(), NOW()),
    (@s_draggable, 'submenu', 'Draggable',          'bi-arrows-move',              '/global-settings/advance-ui/draggable',         @menu_advance_ui, @s_draggable, 0, 0, NOW(), NOW()),
    (@s_treeview,  'submenu', 'Tree View',          'bi-diagram-3',                '/global-settings/advance-ui/tree-view',         @menu_advance_ui, @s_treeview,  0, 0, NOW(), NOW()),
    (@s_blockui,   'submenu', 'Block UI',           'bi-layout-sidebar-reverse',   '/global-settings/advance-ui/block-ui',          @menu_advance_ui, @s_blockui,   0, 0, NOW(), NOW());

-- =============================================================================
-- STEP 7 — INSERT Map & Charts submenus
-- ItemType = 'submenu', ParentId = @menu_map_charts
-- =============================================================================

INSERT IGNORE INTO menuitemsglobalconfig
    (Id, ItemType, Label, Icon, Route, ParentId, SortOrder, IsHidden, IsFlat, CreatedAt, UpdatedAt)
VALUES
    (@s_map,    'submenu', 'Map',    'bi-map',      '/global-settings/map-charts/map',    @menu_map_charts, @s_map,    0, 0, NOW(), NOW()),
    (@s_charts, 'submenu', 'Charts', 'bi-bar-chart','/global-settings/map-charts/charts', @menu_map_charts, @s_charts, 0, 0, NOW(), NOW());

-- =============================================================================
-- STEP 8 — INSERT Table & Forms submenus
-- ItemType = 'submenu', ParentId = @menu_table_forms
-- =============================================================================

INSERT IGNORE INTO menuitemsglobalconfig
    (Id, ItemType, Label, Icon, Route, ParentId, SortOrder, IsHidden, IsFlat, CreatedAt, UpdatedAt)
VALUES
    (@s_basictable,    'submenu', 'Basic Table',    'bi-table',     '/global-settings/table-forms/basic-table',    @menu_table_forms, @s_basictable,    0, 0, NOW(), NOW()),
    (@s_darktable,     'submenu', 'Dark Table',     'bi-moon',      '/global-settings/table-forms/dark-table',     @menu_table_forms, @s_darktable,     0, 0, NOW(), NOW()),
    (@s_borderedtable, 'submenu', 'Bordered Table', 'bi-border-all','/global-settings/table-forms/bordered-table', @menu_table_forms, @s_borderedtable, 0, 0, NOW(), NOW());

-- =============================================================================
-- STEP 9 — Validation SELECT (review rows inserted)
-- Run after execution to confirm all rows are present.
-- =============================================================================

SELECT Id, ItemType, Label, Route, ParentId, SortOrder
FROM   menuitemsglobalconfig
WHERE  Id >= @base
ORDER  BY SortOrder;
