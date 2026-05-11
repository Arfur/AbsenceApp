import mysql.connector
conn = mysql.connector.connect(host='127.0.0.1', port=3306, database='absenceapp', user='root', password='Calm1309!')
cur = conn.cursor()

print('=== rolemenuitems columns ===')
cur.execute("DESCRIBE rolemenuitems")
for row in cur.fetchall():
    print(row)

print()
print('=== Current state check ===')
cur.execute("SELECT Id, Route, Label FROM menuitems WHERE Id IN (402010, 402020, 402030)")
print('menuitems:', cur.fetchall())
cur.execute("SELECT Id, Route FROM AppPages WHERE Id IN (402010, 402020, 401010)")
print('AppPages (402010, 402020, 401010):', cur.fetchall())
cur.execute("SELECT MenuItemId, RoleId, IsEnabled FROM rolemenuitems WHERE MenuItemId=402030")
print('rolemenuitems 402030:', cur.fetchall())

cur.close()
conn.close()
print('Done.')
