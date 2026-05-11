import mysql.connector
conn = mysql.connector.connect(host='127.0.0.1', port=3306, database='absenceapp', user='root', password='Calm1309!')
cur = conn.cursor()

print('=== Sample rolemenuitems for 402000 (model row) ===')
cur.execute("SELECT Id, RoleId, MenuItemId, IsEnabled, AssignedAt, AssignedBy FROM rolemenuitems WHERE MenuItemId=402000 LIMIT 3")
for r in cur.fetchall():
    print(r)

cur.close()
conn.close()
