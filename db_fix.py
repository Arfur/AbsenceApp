import mysql.connector

cn = mysql.connector.connect(
    host='127.0.0.1', port=3306,
    user='root', password='Calm1309!',
    database='absenceapp'
)
cur = cn.cursor()

# Step C: Delete rolemenuitems rows for 401000 and 401010
cur.execute("DELETE FROM rolemenuitems WHERE MenuItemId IN (401000, 401010)")
print(f"rolemenuitems deleted: {cur.rowcount} rows")

# Delete menuitems — submenu before menu to respect FK constraints
cur.execute("DELETE FROM menuitems WHERE Id IN (401010, 401000)")
print(f"menuitems deleted: {cur.rowcount} rows")

# Step D: Rename 402000 to Student Attendance
cur.execute("UPDATE menuitems SET Label='Student Attendance', GroupName='Student Attendance', UpdatedAt=NOW() WHERE Id=402000")
print(f"menuitems updated: {cur.rowcount} rows")

cn.commit()

print()
print("=== VERIFICATION: ATTENDANCE category and children ===")
cur.execute("SELECT Id, Label, ItemType, ParentId, GroupName FROM menuitems WHERE Id=400000 OR ParentId=400000 ORDER BY Id")
for r in cur.fetchall():
    print(r)

print()
print("=== VERIFICATION: 402000 and its children ===")
cur.execute("SELECT Id, Label, ItemType, ParentId, GroupName FROM menuitems WHERE Id=402000 OR ParentId=402000 ORDER BY Id")
for r in cur.fetchall():
    print(r)

print()
print("=== VERIFICATION: No rolemenuitems for 401000/401010 remain ===")
cur.execute("SELECT COUNT(*) FROM rolemenuitems WHERE MenuItemId IN (401000, 401010)")
print(f"Remaining rows: {cur.fetchone()[0]}")

print()
print("=== VERIFICATION: rolemenuitems for 402000 ===")
cur.execute("SELECT Id, RoleId, MenuItemId, IsEnabled FROM rolemenuitems WHERE MenuItemId=402000 ORDER BY RoleId")
for r in cur.fetchall():
    print(r)

cn.close()
print()
print("ALL DONE")
