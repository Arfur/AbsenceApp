"""
Phase8_Execute.py
=================
Connects to absenceapp MySQL/MariaDB and replaces the entire
menuitemsglobalconfig table with the Phase 8 Ki-Admin structure.

Tables modified : menuitemsglobalconfig  (INSERT + DELETE only)
Schema changes  : NONE
"""

import mysql.connector
from datetime import datetime

# ──────────────────────────────────────────────────────────────────────────────
# Connection
# ──────────────────────────────────────────────────────────────────────────────
conn = mysql.connector.connect(
    host='127.0.0.1', port=3306,
    database='absenceapp',
    user='root', password='Calm1309!',
    autocommit=False
)
cur = conn.cursor()
NOW = datetime.now().strftime('%Y-%m-%d %H:%M:%S')

# ──────────────────────────────────────────────────────────────────────────────
# PHASE 8 ID SCHEME
# Base: 200000  (safely above current MAX 103030)
# ──────────────────────────────────────────────────────────────────────────────
# ── Categories ────────────────────────────────────────────────────────────────
CAT_DASHBOARD    = 200100
CAT_APPS         = 200200
CAT_WIDGETS      = 200300
CAT_COMPONENT    = 200400
CAT_ADVANCE_UI   = 200500
CAT_ICONS        = 200600
CAT_MAP_CHARTS   = 200700
CAT_TABLE_FORMS  = 200800
CAT_PAGES        = 200900

# ── Menu groups ───────────────────────────────────────────────────────────────
MENU_DASHBOARD   = 200110   # direct link
MENU_APPS        = 200210   # direct link
MENU_WIDGETS     = 200310   # direct link
MENU_UIKITS      = 200410   # accordion
MENU_ADVANCE_UI  = 200510   # accordion
MENU_FONTAWESOME = 200610   # direct link
MENU_MAP_CHARTS  = 200710   # accordion
MENU_TABLE_FORMS = 200810   # accordion
MENU_PAGES       = 200910   # direct link

# ── UI Kits submenus (parent = MENU_UIKITS = 200410) ─────────────────────────
S_CHEATSHEET    = 200411
S_ALERT         = 200412
S_BADGES        = 200413
S_BUTTONS       = 200414
S_CARDS         = 200415
S_DROPDOWN      = 200416
S_GRID          = 200417
S_AVATAR        = 200418
S_TABS          = 200419
S_ACCORDIONS    = 200420
S_PROGRESS      = 200421
S_NOTIFICATIONS = 200422
S_LISTS         = 200423
S_HELPER        = 200424
S_BACKGROUND    = 200425
S_DIVIDER       = 200426
S_RIBBONS       = 200427
S_EDITOR        = 200428

# ── Advance UI submenus (parent = MENU_ADVANCE_UI = 200510) ──────────────────
S_MODALS        = 200511
S_OFFCANVAS     = 200512
S_SWEAT         = 200513
S_SCROLLBAR     = 200514
S_SPINNERS      = 200515
S_ANIMATION     = 200516
S_VIDEO         = 200517
S_TOUR          = 200518
S_SLIDER        = 200519
S_BSSLIDER      = 200520
S_SCROLLY       = 200521
S_TOOLTIP       = 200522
S_RATING        = 200523
S_PRISMJS       = 200524
S_COUNTDOWN     = 200525
S_COUNTUP       = 200526
S_DRAGGABLE     = 200527
S_TREEVIEW      = 200528
S_BLOCKUI       = 200529

# ── Map & Charts submenus (parent = MENU_MAP_CHARTS = 200710) ────────────────
S_MAP           = 200711
S_CHARTS        = 200712

# ── Table & Forms submenus (parent = MENU_TABLE_FORMS = 200810) ──────────────
S_BASICTABLE    = 200811
S_DARKTABLE     = 200812
S_BORDEREDTABLE = 200813

# ──────────────────────────────────────────────────────────────────────────────
# ROWS TO INSERT
# Columns: Id, ItemType, Label, Icon, Route, ParentId, SortOrder, IsHidden,
#          IsFlat, Description, CreatedAt, UpdatedAt
# ──────────────────────────────────────────────────────────────────────────────
rows = [
    # ── Categories ──────────────────────────────────────────────────────────
    (CAT_DASHBOARD,   'category', 'Dashboard',    'bi-speedometer2',    None, None,            CAT_DASHBOARD,   0, 0, 'Phase 8 – Dashboard category',     NOW, NOW),
    (CAT_APPS,        'category', 'Apps',         'bi-app',             None, None,            CAT_APPS,        0, 0, 'Phase 8 – Apps category',          NOW, NOW),
    (CAT_WIDGETS,     'category', 'Widgets',      'bi-grid-1x2',        None, None,            CAT_WIDGETS,     0, 0, 'Phase 8 – Widgets category',       NOW, NOW),
    (CAT_COMPONENT,   'category', 'Component',    'bi-grid-3x3-gap',    None, None,            CAT_COMPONENT,   0, 0, 'Phase 8 – Component category',     NOW, NOW),
    (CAT_ADVANCE_UI,  'category', 'Advance UI',   'bi-lightning',       None, None,            CAT_ADVANCE_UI,  0, 0, 'Phase 8 – Advance UI category',    NOW, NOW),
    (CAT_ICONS,       'category', 'Icons',        'bi-emoji-smile',     None, None,            CAT_ICONS,       0, 0, 'Phase 8 – Icons category',         NOW, NOW),
    (CAT_MAP_CHARTS,  'category', 'Map & Charts', 'bi-bar-chart-line',  None, None,            CAT_MAP_CHARTS,  0, 0, 'Phase 8 – Map & Charts category',  NOW, NOW),
    (CAT_TABLE_FORMS, 'category', 'Table & Forms','bi-table',           None, None,            CAT_TABLE_FORMS, 0, 0, 'Phase 8 – Table & Forms category', NOW, NOW),
    (CAT_PAGES,       'category', 'Pages',        'bi-file-earmark',    None, None,            CAT_PAGES,       0, 0, 'Phase 8 – Pages category',         NOW, NOW),

    # ── Menu groups ──────────────────────────────────────────────────────────
    # Direct-link menus (IsFlat=1, Route set, no submenus)
    (MENU_DASHBOARD,   'menu', 'Dashboard',    'bi-speedometer2',       '/global-settings/dashboard',         CAT_DASHBOARD,   MENU_DASHBOARD,   0, 1, 'Dashboard home',          NOW, NOW),
    (MENU_APPS,        'menu', 'Apps',         'bi-app',                '/global-settings/apps',              CAT_APPS,        MENU_APPS,        0, 1, 'Apps overview',           NOW, NOW),
    (MENU_WIDGETS,     'menu', 'Widgets',      'bi-grid-1x2',           '/global-settings/widgets',           CAT_WIDGETS,     MENU_WIDGETS,     0, 1, 'Widgets overview',        NOW, NOW),
    (MENU_FONTAWESOME, 'menu', 'Fontawesome',  'bi-bootstrap',          '/global-settings/icons/fontawesome', CAT_ICONS,       MENU_FONTAWESOME, 0, 1, 'Fontawesome icons',       NOW, NOW),
    (MENU_PAGES,       'menu', 'Pages',        'bi-file-earmark',       '/global-settings/pages',             CAT_PAGES,       MENU_PAGES,       0, 1, 'Pages (existing)',        NOW, NOW),
    # Accordion menus (IsFlat=0, no Route)
    (MENU_UIKITS,      'menu', 'UI Kits',      'bi-grid-3x3-gap-fill',  None,                                 CAT_COMPONENT,   MENU_UIKITS,      0, 0, 'UI Kits group',           NOW, NOW),
    (MENU_ADVANCE_UI,  'menu', 'Advance UI',   'bi-lightning',          None,                                 CAT_ADVANCE_UI,  MENU_ADVANCE_UI,  0, 0, 'Advance UI group',        NOW, NOW),
    (MENU_MAP_CHARTS,  'menu', 'Map & Charts', 'bi-bar-chart-line',     None,                                 CAT_MAP_CHARTS,  MENU_MAP_CHARTS,  0, 0, 'Map & Charts group',      NOW, NOW),
    (MENU_TABLE_FORMS, 'menu', 'Table & Forms','bi-table',              None,                                 CAT_TABLE_FORMS, MENU_TABLE_FORMS, 0, 0, 'Table & Forms group',     NOW, NOW),

    # ── UI Kits submenus ─────────────────────────────────────────────────────
    (S_CHEATSHEET,    'submenu', 'Cheatsheet',    'bi-journal-code',         '/global-settings/ui-kits/cheatsheet',    MENU_UIKITS, S_CHEATSHEET,    0, 0, None, NOW, NOW),
    (S_ALERT,         'submenu', 'Alert',         'bi-exclamation-triangle', '/global-settings/ui-kits/alert',         MENU_UIKITS, S_ALERT,         0, 0, None, NOW, NOW),
    (S_BADGES,        'submenu', 'Badges',        'bi-tag',                  '/global-settings/ui-kits/badges',        MENU_UIKITS, S_BADGES,        0, 0, None, NOW, NOW),
    (S_BUTTONS,       'submenu', 'Buttons',       'bi-stop-btn',             '/global-settings/buttons',               MENU_UIKITS, S_BUTTONS,       0, 0, None, NOW, NOW),
    (S_CARDS,         'submenu', 'Cards',         'bi-card-heading',         '/global-settings/ui-kits/cards',         MENU_UIKITS, S_CARDS,         0, 0, None, NOW, NOW),
    (S_DROPDOWN,      'submenu', 'Dropdown',      'bi-chevron-down',         '/global-settings/ui-kits/dropdown',      MENU_UIKITS, S_DROPDOWN,      0, 0, None, NOW, NOW),
    (S_GRID,          'submenu', 'Grid',          'bi-grid',                 '/global-settings/ui-kits/grid',          MENU_UIKITS, S_GRID,          0, 0, None, NOW, NOW),
    (S_AVATAR,        'submenu', 'Avatar',        'bi-person-circle',        '/global-settings/ui-kits/avatar',        MENU_UIKITS, S_AVATAR,        0, 0, None, NOW, NOW),
    (S_TABS,          'submenu', 'Tabs',          'bi-folder',               '/global-settings/ui-kits/tabs',          MENU_UIKITS, S_TABS,          0, 0, None, NOW, NOW),
    (S_ACCORDIONS,    'submenu', 'Accordions',    'bi-layout-text-sidebar',  '/global-settings/ui-kits/accordions',    MENU_UIKITS, S_ACCORDIONS,    0, 0, None, NOW, NOW),
    (S_PROGRESS,      'submenu', 'Progress',      'bi-hourglass-split',      '/global-settings/ui-kits/progress',      MENU_UIKITS, S_PROGRESS,      0, 0, None, NOW, NOW),
    (S_NOTIFICATIONS, 'submenu', 'Notifications', 'bi-bell',                 '/global-settings/ui-kits/notifications', MENU_UIKITS, S_NOTIFICATIONS, 0, 0, None, NOW, NOW),
    (S_LISTS,         'submenu', 'Lists',         'bi-list-ul',              '/global-settings/ui-kits/lists',         MENU_UIKITS, S_LISTS,         0, 0, None, NOW, NOW),
    (S_HELPER,        'submenu', 'Helper Classes','bi-tools',                '/global-settings/ui-kits/helper-classes',MENU_UIKITS, S_HELPER,        0, 0, None, NOW, NOW),
    (S_BACKGROUND,    'submenu', 'Background',    'bi-image',                '/global-settings/ui-kits/background',    MENU_UIKITS, S_BACKGROUND,    0, 0, None, NOW, NOW),
    (S_DIVIDER,       'submenu', 'Divider',       'bi-dash-lg',              '/global-settings/ui-kits/divider',       MENU_UIKITS, S_DIVIDER,       0, 0, None, NOW, NOW),
    (S_RIBBONS,       'submenu', 'Ribbons',       'bi-bookmark-star',        '/global-settings/ui-kits/ribbons',       MENU_UIKITS, S_RIBBONS,       0, 0, None, NOW, NOW),
    (S_EDITOR,        'submenu', 'Editor',        'bi-pencil-square',        '/global-settings/ui-kits/editor',        MENU_UIKITS, S_EDITOR,        0, 0, None, NOW, NOW),

    # ── Advance UI submenus ──────────────────────────────────────────────────
    (S_MODALS,    'submenu', 'Modals',             'bi-window',                  '/global-settings/advance-ui/modals',           MENU_ADVANCE_UI, S_MODALS,    0, 0, None, NOW, NOW),
    (S_OFFCANVAS, 'submenu', 'Offcanvas Toggle',   'bi-layout-sidebar',          '/global-settings/advance-ui/offcanvas-toggle', MENU_ADVANCE_UI, S_OFFCANVAS, 0, 0, None, NOW, NOW),
    (S_SWEAT,     'submenu', 'Sweat Alert',        'bi-patch-exclamation',       '/global-settings/advance-ui/sweat-alert',      MENU_ADVANCE_UI, S_SWEAT,     0, 0, None, NOW, NOW),
    (S_SCROLLBAR, 'submenu', 'Scrollbar',          'bi-arrows-expand',           '/global-settings/advance-ui/scrollbar',        MENU_ADVANCE_UI, S_SCROLLBAR, 0, 0, None, NOW, NOW),
    (S_SPINNERS,  'submenu', 'Spinners',           'bi-arrow-repeat',            '/global-settings/advance-ui/spinners',         MENU_ADVANCE_UI, S_SPINNERS,  0, 0, None, NOW, NOW),
    (S_ANIMATION, 'submenu', 'Animation',          'bi-stars',                   '/global-settings/advance-ui/animation',        MENU_ADVANCE_UI, S_ANIMATION, 0, 0, None, NOW, NOW),
    (S_VIDEO,     'submenu', 'Video Embed',        'bi-play-circle',             '/global-settings/advance-ui/video-embed',      MENU_ADVANCE_UI, S_VIDEO,     0, 0, None, NOW, NOW),
    (S_TOUR,      'submenu', 'Tour',               'bi-map',                     '/global-settings/advance-ui/tour',             MENU_ADVANCE_UI, S_TOUR,      0, 0, None, NOW, NOW),
    (S_SLIDER,    'submenu', 'Slider',             'bi-sliders',                 '/global-settings/advance-ui/slider',           MENU_ADVANCE_UI, S_SLIDER,    0, 0, None, NOW, NOW),
    (S_BSSLIDER,  'submenu', 'Bootstrap Slider',  'bi-sliders2',                '/global-settings/advance-ui/bootstrap-slider', MENU_ADVANCE_UI, S_BSSLIDER,  0, 0, None, NOW, NOW),
    (S_SCROLLY,   'submenu', 'Scrolly',            'bi-arrow-down-circle',       '/global-settings/advance-ui/scrolly',          MENU_ADVANCE_UI, S_SCROLLY,   0, 0, None, NOW, NOW),
    (S_TOOLTIP,   'submenu', 'Tooltip & Popovers', 'bi-chat-right-text',         '/global-settings/advance-ui/tooltip-popovers', MENU_ADVANCE_UI, S_TOOLTIP,   0, 0, None, NOW, NOW),
    (S_RATING,    'submenu', 'Rating',             'bi-star',                    '/global-settings/advance-ui/rating',           MENU_ADVANCE_UI, S_RATING,    0, 0, None, NOW, NOW),
    (S_PRISMJS,   'submenu', 'Prismjs',            'bi-code-slash',              '/global-settings/advance-ui/prismjs',          MENU_ADVANCE_UI, S_PRISMJS,   0, 0, None, NOW, NOW),
    (S_COUNTDOWN, 'submenu', 'Count Down',         'bi-stopwatch',               '/global-settings/advance-ui/count-down',       MENU_ADVANCE_UI, S_COUNTDOWN, 0, 0, None, NOW, NOW),
    (S_COUNTUP,   'submenu', 'Count Up',           'bi-hourglass',               '/global-settings/advance-ui/count-up',         MENU_ADVANCE_UI, S_COUNTUP,   0, 0, None, NOW, NOW),
    (S_DRAGGABLE, 'submenu', 'Draggable',          'bi-arrows-move',             '/global-settings/advance-ui/draggable',        MENU_ADVANCE_UI, S_DRAGGABLE, 0, 0, None, NOW, NOW),
    (S_TREEVIEW,  'submenu', 'Tree View',          'bi-diagram-3',               '/global-settings/advance-ui/tree-view',        MENU_ADVANCE_UI, S_TREEVIEW,  0, 0, None, NOW, NOW),
    (S_BLOCKUI,   'submenu', 'Block UI',           'bi-layout-sidebar-reverse',  '/global-settings/advance-ui/block-ui',         MENU_ADVANCE_UI, S_BLOCKUI,   0, 0, None, NOW, NOW),

    # ── Map & Charts submenus ────────────────────────────────────────────────
    (S_MAP,    'submenu', 'Map',    'bi-map',       '/global-settings/map-charts/map',    MENU_MAP_CHARTS, S_MAP,    0, 0, None, NOW, NOW),
    (S_CHARTS, 'submenu', 'Charts', 'bi-bar-chart', '/global-settings/map-charts/charts', MENU_MAP_CHARTS, S_CHARTS, 0, 0, None, NOW, NOW),

    # ── Table & Forms submenus ───────────────────────────────────────────────
    (S_BASICTABLE,    'submenu', 'Basic Table',    'bi-table',      '/global-settings/table-forms/basic-table',    MENU_TABLE_FORMS, S_BASICTABLE,    0, 0, None, NOW, NOW),
    (S_DARKTABLE,     'submenu', 'Dark Table',     'bi-moon',       '/global-settings/table-forms/dark-table',     MENU_TABLE_FORMS, S_DARKTABLE,     0, 0, None, NOW, NOW),
    (S_BORDEREDTABLE, 'submenu', 'Bordered Table', 'bi-border-all', '/global-settings/table-forms/bordered-table', MENU_TABLE_FORMS, S_BORDEREDTABLE, 0, 0, None, NOW, NOW),
]

# ──────────────────────────────────────────────────────────────────────────────
# PRINT GENERATED SQL (for review log)
# ──────────────────────────────────────────────────────────────────────────────
print("=" * 80)
print("GENERATED SQL — Phase 8 menuitemsglobalconfig rebuild")
print("=" * 80)

print("\n-- STEP 1: DELETE all existing rows")
cur.execute("SELECT Id FROM menuitemsglobalconfig ORDER BY Id")
old_ids = [r[0] for r in cur.fetchall()]
print(f"DELETE FROM menuitemsglobalconfig WHERE Id IN ({', '.join(str(i) for i in old_ids)});")
print(f"-- ({len(old_ids)} rows to delete)")

print("\n-- STEP 2: INSERT Phase 8 structure")
insert_sql = """
INSERT INTO menuitemsglobalconfig
    (Id, ItemType, Label, Icon, Route, ParentId, SortOrder, IsHidden, IsFlat, Description, CreatedAt, UpdatedAt)
VALUES (%s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s)
"""
for r in rows:
    route_str = f"'{r[4]}'" if r[4] else 'NULL'
    parent_str = str(r[5]) if r[5] else 'NULL'
    desc_str = f"'{r[10]}'" if r[10] else 'NULL'
    print(f"  ({r[0]}, '{r[1]}', '{r[2]}', '{r[3]}', {route_str}, {parent_str}, {r[6]}, {r[7]}, {r[8]}, {desc_str}, NOW(), NOW()),")

print(f"\n-- ({len(rows)} rows to insert)")

# ──────────────────────────────────────────────────────────────────────────────
# EXECUTE in a single transaction
# ──────────────────────────────────────────────────────────────────────────────
print("\n" + "=" * 80)
print("EXECUTING...")
print("=" * 80)

try:
    # Step 1: Delete all existing rows
    if old_ids:
        placeholders = ', '.join(['%s'] * len(old_ids))
        cur.execute(f"DELETE FROM menuitemsglobalconfig WHERE Id IN ({placeholders})", old_ids)
        print(f"  DELETE: {cur.rowcount} rows removed")

    # Step 2: Insert all new rows
    cur.executemany(insert_sql, rows)
    print(f"  INSERT: {cur.rowcount} rows added")

    conn.commit()
    print("  COMMIT: OK")

except Exception as e:
    conn.rollback()
    print(f"  ERROR — rolling back: {e}")
    conn.close()
    raise

# ──────────────────────────────────────────────────────────────────────────────
# VALIDATION SELECT 1 — full hierarchy
# ──────────────────────────────────────────────────────────────────────────────
print("\n" + "=" * 80)
print("VALIDATION SELECT 1: Full hierarchy ordered by SortOrder")
print("=" * 80)
cur.execute("""
    SELECT Id, ItemType, ParentId, Label, Route, SortOrder, IsFlat
    FROM   menuitemsglobalconfig
    ORDER  BY SortOrder
""")
rows_out = cur.fetchall()
print(f"{'Id':>7}  {'ItemType':<10} {'ParentId':>8}  {'Label':<30} {'Route':<45} {'SortOrder':>9} {'IsFlat':>6}")
print("-" * 120)
for r in rows_out:
    pid = str(r[2]) if r[2] is not None else 'NULL'
    route = str(r[4]) if r[4] is not None else 'NULL'
    print(f"{r[0]:>7}  {str(r[1]):<10} {pid:>8}  {str(r[3]):<30} {route:<45} {str(r[5]):>9} {str(r[6]):>6}")
print(f"\nTotal rows: {len(rows_out)}")

# ──────────────────────────────────────────────────────────────────────────────
# VALIDATION SELECT 2 — orphaned rows check
# ──────────────────────────────────────────────────────────────────────────────
print("\n" + "=" * 80)
print("VALIDATION SELECT 2: Orphaned rows (ParentId references non-existent Id)")
print("=" * 80)
cur.execute("""
    SELECT Id, ItemType, ParentId, Label
    FROM   menuitemsglobalconfig
    WHERE  ParentId IS NOT NULL
      AND  ParentId NOT IN (SELECT Id FROM menuitemsglobalconfig)
""")
orphans = cur.fetchall()
if orphans:
    print("WARNING — orphaned rows found:")
    for r in orphans:
        print(f"  Id={r[0]}  ItemType={r[1]}  ParentId={r[2]}  Label={r[3]}")
else:
    print("  OK — no orphaned rows.")

# ──────────────────────────────────────────────────────────────────────────────
# SUMMARY
# ──────────────────────────────────────────────────────────────────────────────
categories_count = sum(1 for r in rows_out if r[1] == 'category')
menus_count      = sum(1 for r in rows_out if r[1] == 'menu')
submenus_count   = sum(1 for r in rows_out if r[1] == 'submenu')

print("\n" + "=" * 80)
print("SUMMARY")
print("=" * 80)
print(f"  Categories : {categories_count}")
print(f"  Menus      : {menus_count}")
print(f"  Submenus   : {submenus_count}")
print(f"  Total rows : {len(rows_out)}")
print(f"  Orphans    : {len(orphans)}")
print(f"  Schema     : NO CHANGES (INSERT + DELETE only)")
print("  STATUS     : COMPLETE")

conn.close()
