"""
===============================================================================
 File        : db_design_tokens.py
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-05-12
 Updated     : 2026-05-12
-------------------------------------------------------------------------------
 Purpose     : Creates the DesignTokens table and inserts all 28 seed rows
               for the Phase A Design Token System.

               Run once against the absenceapp database.  The script is safe
               to re-run; CREATE TABLE uses IF NOT EXISTS and inserts use
               INSERT IGNORE so existing rows are not overwritten.
-------------------------------------------------------------------------------
 Usage       :
   cd C:\DevAbsence1
   .venv\Scripts\python.exe db_design_tokens.py
===============================================================================
"""

import mysql.connector

# ---------------------------------------------------------------------------
# Connection
# ---------------------------------------------------------------------------
conn = mysql.connector.connect(
    host="127.0.0.1",
    port=3306,
    database="absenceapp",
    user="root",
    password="Calm1309!",
)
cursor = conn.cursor()

# ---------------------------------------------------------------------------
# 1. Create table
# ---------------------------------------------------------------------------
cursor.execute("""
CREATE TABLE IF NOT EXISTS DesignTokens (
    Id             INT          NOT NULL AUTO_INCREMENT,
    ComponentGroup VARCHAR(100) NOT NULL,
    TokenKey       VARCHAR(200) NOT NULL,
    CssVariable    VARCHAR(200) NOT NULL,
    DefaultValue   VARCHAR(500) NOT NULL,
    CurrentValue   VARCHAR(500)     NULL,
    Category       VARCHAR(100) NOT NULL,
    Description    VARCHAR(500)     NULL,
    IsActive       TINYINT(1)   NOT NULL DEFAULT 1,
    SortOrder      INT          NOT NULL DEFAULT 0,
    CreatedAt      DATETIME     NOT NULL,
    UpdatedAt      DATETIME     NOT NULL,
    PRIMARY KEY (Id),
    UNIQUE KEY IX_DesignTokens_ComponentGroup_TokenKey (ComponentGroup, TokenKey)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
""")
print("CREATE TABLE: OK")

# ---------------------------------------------------------------------------
# 2. Seed rows — 28 button tokens
#    Columns: Id, ComponentGroup, TokenKey, CssVariable, DefaultValue,
#             CurrentValue, Category, Description, IsActive, SortOrder,
#             CreatedAt, UpdatedAt
# ---------------------------------------------------------------------------
SEED_DATE = "2026-05-12 00:00:00"

rows = [
    # ── Primary (10–13) ──────────────────────────────────────────────────────
    (10, "btn", "primary-bg",       "--ds-btn-primary-bg",       "#0d6efd",    None, "color",     "Primary button background colour",       1, 10, SEED_DATE, SEED_DATE),
    (11, "btn", "primary-text",     "--ds-btn-primary-text",     "#ffffff",    None, "color",     "Primary button text colour",             1, 11, SEED_DATE, SEED_DATE),
    (12, "btn", "primary-border",   "--ds-btn-primary-border",   "#0d6efd",    None, "color",     "Primary button border colour",           1, 12, SEED_DATE, SEED_DATE),
    (13, "btn", "primary-hover-bg", "--ds-btn-primary-hover-bg", "#0b5ed7",    None, "color",     "Primary button hover background colour", 1, 13, SEED_DATE, SEED_DATE),

    # ── Secondary (20–23) ────────────────────────────────────────────────────
    (20, "btn", "secondary-bg",       "--ds-btn-secondary-bg",       "transparent", None, "color", "Secondary button background colour",       1, 20, SEED_DATE, SEED_DATE),
    (21, "btn", "secondary-text",     "--ds-btn-secondary-text",     "#212529",     None, "color", "Secondary button text colour",             1, 21, SEED_DATE, SEED_DATE),
    (22, "btn", "secondary-border",   "--ds-btn-secondary-border",   "#dee2e6",     None, "color", "Secondary button border colour",           1, 22, SEED_DATE, SEED_DATE),
    (23, "btn", "secondary-hover-bg", "--ds-btn-secondary-hover-bg", "#f8f9fa",     None, "color", "Secondary button hover background colour", 1, 23, SEED_DATE, SEED_DATE),

    # ── Success (30–33) ──────────────────────────────────────────────────────
    (30, "btn", "success-bg",       "--ds-btn-success-bg",       "#198754", None, "color", "Success button background colour",       1, 30, SEED_DATE, SEED_DATE),
    (31, "btn", "success-text",     "--ds-btn-success-text",     "#ffffff", None, "color", "Success button text colour",             1, 31, SEED_DATE, SEED_DATE),
    (32, "btn", "success-border",   "--ds-btn-success-border",   "#198754", None, "color", "Success button border colour",           1, 32, SEED_DATE, SEED_DATE),
    (33, "btn", "success-hover-bg", "--ds-btn-success-hover-bg", "#157347", None, "color", "Success button hover background colour", 1, 33, SEED_DATE, SEED_DATE),

    # ── Danger (40–43) ───────────────────────────────────────────────────────
    (40, "btn", "danger-bg",       "--ds-btn-danger-bg",       "#dc3545", None, "color", "Danger button background colour",       1, 40, SEED_DATE, SEED_DATE),
    (41, "btn", "danger-text",     "--ds-btn-danger-text",     "#ffffff", None, "color", "Danger button text colour",             1, 41, SEED_DATE, SEED_DATE),
    (42, "btn", "danger-border",   "--ds-btn-danger-border",   "#dc3545", None, "color", "Danger button border colour",           1, 42, SEED_DATE, SEED_DATE),
    (43, "btn", "danger-hover-bg", "--ds-btn-danger-hover-bg", "#bb2d3b", None, "color", "Danger button hover background colour", 1, 43, SEED_DATE, SEED_DATE),

    # ── Warning (50–53) ──────────────────────────────────────────────────────
    (50, "btn", "warning-bg",       "--ds-btn-warning-bg",       "#ffc107", None, "color", "Warning button background colour",       1, 50, SEED_DATE, SEED_DATE),
    (51, "btn", "warning-text",     "--ds-btn-warning-text",     "#212529", None, "color", "Warning button text colour",             1, 51, SEED_DATE, SEED_DATE),
    (52, "btn", "warning-border",   "--ds-btn-warning-border",   "#ffc107", None, "color", "Warning button border colour",           1, 52, SEED_DATE, SEED_DATE),
    (53, "btn", "warning-hover-bg", "--ds-btn-warning-hover-bg", "#ffca2c", None, "color", "Warning button hover background colour", 1, 53, SEED_DATE, SEED_DATE),

    # ── Info (60–63) ─────────────────────────────────────────────────────────
    (60, "btn", "info-bg",       "--ds-btn-info-bg",       "#0dcaf0", None, "color", "Info button background colour",       1, 60, SEED_DATE, SEED_DATE),
    (61, "btn", "info-text",     "--ds-btn-info-text",     "#212529", None, "color", "Info button text colour",             1, 61, SEED_DATE, SEED_DATE),
    (62, "btn", "info-border",   "--ds-btn-info-border",   "#0dcaf0", None, "color", "Info button border colour",           1, 62, SEED_DATE, SEED_DATE),
    (63, "btn", "info-hover-bg", "--ds-btn-info-hover-bg", "#31d2f2", None, "color", "Info button hover background colour", 1, 63, SEED_DATE, SEED_DATE),

    # ── Structural (70–73) ───────────────────────────────────────────────────
    (70, "btn", "border-radius", "--ds-btn-border-radius", "6px",      None, "structure", "Button border radius",      1, 70, SEED_DATE, SEED_DATE),
    (71, "btn", "font-size",     "--ds-btn-font-size",     "0.875rem", None, "structure", "Button font size",          1, 71, SEED_DATE, SEED_DATE),
    (72, "btn", "padding-y",     "--ds-btn-padding-y",     "7px",      None, "structure", "Button vertical padding",   1, 72, SEED_DATE, SEED_DATE),
    (73, "btn", "padding-x",     "--ds-btn-padding-x",     "16px",     None, "structure", "Button horizontal padding", 1, 73, SEED_DATE, SEED_DATE),
]

insert_sql = """
INSERT IGNORE INTO DesignTokens
    (Id, ComponentGroup, TokenKey, CssVariable, DefaultValue, CurrentValue,
     Category, Description, IsActive, SortOrder, CreatedAt, UpdatedAt)
VALUES
    (%s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s)
"""

cursor.executemany(insert_sql, rows)
conn.commit()
print(f"INSERT seed rows: {cursor.rowcount} rows inserted")

# ---------------------------------------------------------------------------
# 3. Verify
# ---------------------------------------------------------------------------
cursor.execute("SELECT COUNT(*) FROM DesignTokens;")
count = cursor.fetchone()[0]
print(f"DesignTokens row count: {count}")

cursor.execute(
    "SELECT Id, ComponentGroup, TokenKey, CssVariable, DefaultValue "
    "FROM DesignTokens ORDER BY SortOrder;"
)
print("\nSeeded tokens:")
for row in cursor.fetchall():
    print(f"  [{row[0]:>3}] {row[1]}.{row[2]:<25}  {row[3]:<30}  default={row[4]}")

cursor.close()
conn.close()
print("\nDone.")
