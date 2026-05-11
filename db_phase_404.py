import mysql.connector
conn = mysql.connector.connect(host='127.0.0.1', port=3306, database='absenceapp', user='root', password='Calm1309!')
cur = conn.cursor()

# ---------------------------------------------------------------------------
# PHASE 1: Pre-check AppPages for duplicate target routes
# ---------------------------------------------------------------------------
print('=== PHASE 1: AppPages pre-check ===')
cur.execute("SELECT Id, Route FROM AppPages WHERE Route IN ('/v2/attendance', '/v2/attendance/student')")
existing_pages = cur.fetchall()
print('AppPages rows at target routes:', existing_pages)
existing_routes = [r[1] for r in existing_pages]

print()
print('=== PHASE 2: menuitems routes fix ===')
cur.execute("UPDATE menuitems SET Route='/v2/attendance', UpdatedAt=NOW() WHERE Id=402010")
print(f'menuitems 402010 Route updated, rows affected: {cur.rowcount}')
cur.execute("UPDATE menuitems SET Route='/v2/attendance/student', UpdatedAt=NOW() WHERE Id=402020")
print(f'menuitems 402020 Route updated, rows affected: {cur.rowcount}')

print()
print('=== PHASE 2: AppPages conditional UPDATE/DELETE ===')
if '/v2/attendance' in existing_routes:
    cur.execute("DELETE FROM AppPages WHERE Id=402010")
    print(f'AppPages 402010 DELETED (duplicate route already exists), rows: {cur.rowcount}')
else:
    cur.execute("UPDATE AppPages SET Route='/v2/attendance', UpdatedAt=NOW() WHERE Id=402010")
    print(f'AppPages 402010 UPDATED, rows: {cur.rowcount}')

if '/v2/attendance/student' in existing_routes:
    cur.execute("DELETE FROM AppPages WHERE Id=402020")
    print(f'AppPages 402020 DELETED (duplicate route already exists), rows: {cur.rowcount}')
else:
    cur.execute("UPDATE AppPages SET Route='/v2/attendance/student', UpdatedAt=NOW() WHERE Id=402020")
    print(f'AppPages 402020 UPDATED, rows: {cur.rowcount}')

print()
print('=== PHASE 3: Insert Test 404 submenu (402030) ===')
cur.execute("SELECT Id FROM menuitems WHERE Id=402030")
if cur.fetchone():
    print('402030 already exists in menuitems - skipping INSERT')
else:
    cur.execute("""
        INSERT INTO menuitems
            (Id, ParentId, ItemType, Label, Icon, Route, SortOrder, IsHidden,
             Category, GroupName, GroupIcon, IsFlat, Status, Description, CreatedAt, UpdatedAt)
        VALUES
            (402030, 402000, 'submenu', 'Test 404Page', 'bi-exclamation-triangle', '/v2/test/404',
             402030, 0, 'ATTENDANCE', 'Student Attendance', 'bi-mortarboard', 0, 'active',
             'Intentionally triggers 404 page for testing.', NOW(), NOW())
    """)
    print(f'menuitems 402030 INSERTED, rows: {cur.rowcount}')

cur.execute("SELECT RoleId FROM rolemenuitems WHERE MenuItemId=402030")
existing_roles = [r[0] for r in cur.fetchall()]
print(f'Existing rolemenuitems for 402030: {existing_roles}')
for role_id in [1, 2, 3]:
    if role_id not in existing_roles:
        cur.execute(
            "INSERT INTO rolemenuitems (RoleId, MenuItemId, IsEnabled, AssignedAt, AssignedBy) VALUES (%s, 402030, 1, NOW(), 1)",
            (role_id,)
        )
        print(f'rolemenuitems RoleId={role_id} INSERTED, rows: {cur.rowcount}')
    else:
        print(f'rolemenuitems RoleId={role_id} already exists - skipping')

conn.commit()
print()
print('=== COMMITTED ===')

print()
print('=== VERIFICATION ===')
cur.execute("SELECT Id, Route, Label FROM menuitems WHERE Id IN (402010, 402020, 402030)")
print('menuitems:', cur.fetchall())
cur.execute("SELECT Id, Route FROM AppPages WHERE Id IN (402010, 402020)")
print('AppPages remaining (402010,402020):', cur.fetchall())
cur.execute("SELECT MenuItemId, RoleId, IsEnabled FROM rolemenuitems WHERE MenuItemId=402030 ORDER BY RoleId")
print('rolemenuitems 402030:', cur.fetchall())

cur.close()
conn.close()
print('Done.')
