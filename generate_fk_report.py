#!/usr/bin/env python3
"""
Generate a complete FK inspection JSON for the 'absenceapp' schema.
Requires: pip install pymysql
Run: python generate_fk_report.py
"""
import json
import getpass
import pymysql

host = "localhost"
port = 3306
user = "root"
passwd = "Calm1309!"
db = "absenceapp"

conn = pymysql.connect(host=host, port=port, user=user, password=passwd, db=db, charset='utf8mb4', cursorclass=pymysql.cursors.DictCursor)
out = {"fks": [], "missing_parents": [], "actions": []}

def fetchone(q, params=None):
    with conn.cursor() as cur:
        cur.execute(q, params or ())
        return cur.fetchone()

def fetchall(q, params=None):
    with conn.cursor() as cur:
        cur.execute(q, params or ())
        return cur.fetchall()

# Get all foreign key constraints in this schema
fk_rows = fetchall("""
SELECT rc.CONSTRAINT_NAME, kcu.TABLE_NAME AS ChildTable, kcu.COLUMN_NAME AS ChildColumn,
       rc.REFERENCED_TABLE_NAME AS ParentTable, kcu.REFERENCED_COLUMN_NAME AS ParentColumn
FROM information_schema.REFERENTIAL_CONSTRAINTS rc
JOIN information_schema.KEY_COLUMN_USAGE kcu
  ON rc.CONSTRAINT_SCHEMA = kcu.CONSTRAINT_SCHEMA
  AND rc.CONSTRAINT_NAME = kcu.CONSTRAINT_NAME
WHERE rc.CONSTRAINT_SCHEMA = %s
ORDER BY kcu.TABLE_NAME, kcu.ORDINAL_POSITION
""", (db,))

processed_missing = {}

for fk in fk_rows:
    cname = fk['CONSTRAINT_NAME']
    child = fk['ChildTable']
    childcol = fk['ChildColumn']
    parent = fk['ParentTable']
    parentcol = fk['ParentColumn']

    # SHOW CREATE TABLE for child and parent (if exists)
    child_create = fetchone("SHOW CREATE TABLE `{}`".format(child)) or {}
    child_create_sql = child_create.get('Create Table') if child_create else ""
    parent_create_sql = ""
    parent_exists = "NO"
    parent_engine = ""
    parent_col_type = ""
    parent_indexed = "NO"

    p_exists = fetchone("SELECT TABLE_NAME, ENGINE FROM information_schema.TABLES WHERE TABLE_SCHEMA=%s AND TABLE_NAME=%s", (db, parent))
    if p_exists:
        parent_exists = "YES"
        parent_create = fetchone("SHOW CREATE TABLE `{}`".format(parent))
        parent_create_sql = parent_create.get('Create Table') if parent_create else ""
        parent_engine = p_exists.get('ENGINE') or ""
        # parent column type and index check
        pc = fetchone("SELECT COLUMN_TYPE, COLLATION_NAME FROM information_schema.COLUMNS WHERE TABLE_SCHEMA=%s AND TABLE_NAME=%s AND COLUMN_NAME=%s", (db, parent, parentcol))
        if pc:
            parent_col_type = pc['COLUMN_TYPE']
        idx = fetchone("SELECT 1 FROM information_schema.STATISTICS WHERE TABLE_SCHEMA=%s AND TABLE_NAME=%s AND (COLUMN_NAME=%s) LIMIT 1", (db, parent, parentcol))
        parent_indexed = "YES" if idx else "NO"

    # child column info
    cc = fetchone("SELECT COLUMN_TYPE, COLLATION_NAME FROM information_schema.COLUMNS WHERE TABLE_SCHEMA=%s AND TABLE_NAME=%s AND COLUMN_NAME=%s", (db, child, childcol))
    child_col_type = cc['COLUMN_TYPE'] if cc else ""
    child_engine_row = fetchone("SELECT ENGINE FROM information_schema.TABLES WHERE TABLE_SCHEMA=%s AND TABLE_NAME=%s", (db, child))
    child_engine = child_engine_row['ENGINE'] if child_engine_row else ""

    # signedness and collation comparisons (simple heuristics)
    def signedness(t): return "unsigned" if t and "unsigned" in t else "signed"
    signed_match = "N/A"
    if parent_col_type and child_col_type:
        signed_match = "YES" if signedness(parent_col_type) == signedness(child_col_type) else "NO"
    collation_match = "N/A"
    if cc and pc:
        collation_match = "YES" if (cc['COLLATION_NAME'] or "") == (pc['COLLATION_NAME'] or "") else "NO"

    # orphan count and sample rows
    orphan_count = 0
    orphan_samples = []
    if parent_exists == "YES":
        q_count = "SELECT COUNT(*) AS c FROM `{child}` WHERE `{col}` IS NOT NULL AND `{col}` NOT IN (SELECT `{pcol}` FROM `{parent}`)".format(child=child, col=childcol, pcol=parentcol, parent=parent)
        r = fetchone(q_count)
        orphan_count = int(r['c']) if r else 0
        if orphan_count > 0:
            # get primary key columns for child
            pk_cols = [r['COLUMN_NAME'] for r in fetchall("SELECT COLUMN_NAME FROM information_schema.COLUMNS WHERE TABLE_SCHEMA=%s AND TABLE_NAME=%s AND COLUMN_KEY='PRI' ORDER BY ORDINAL_POSITION", (db, child))]
            pk_select = ", ".join("`{}`".format(c) for c in pk_cols) if pk_cols else "NULL"
            sample_q = "SELECT `{col}` AS child_value, {pk} FROM `{child}` WHERE `{col}` IS NOT NULL AND `{col}` NOT IN (SELECT `{pcol}` FROM `{parent}`) LIMIT 20".format(col=childcol, pk=pk_select, child=child, pcol=parentcol, parent=parent)
            orphan_samples = fetchall(sample_q)

    fk_entry = {
        "ConstraintName": cname,
        "ChildTable": child,
        "ChildColumn": childcol,
        "ParentTable": parent,
        "ParentColumn": parentcol,
        "ParentExists": parent_exists,
        "create_table_child": child_create_sql or "",
        "create_table_parent": parent_create_sql or "",
        "ChildEngine": child_engine,
        "ParentEngine": parent_engine,
        "ChildColumnType": child_col_type,
        "ParentColumnType": parent_col_type,
        "SignednessMatch": signed_match,
        "CollationMatch": collation_match,
        "ParentIndexed": parent_indexed,
        "OrphanRowCount": orphan_count,
        "OrphanSampleRows": orphan_samples or []
    }
    out['fks'].append(fk_entry)

    # collect missing parent create suggestion once
    if parent_exists == "NO" and parent not in processed_missing:
        # minimal create: referenced column type from child if available
        suggested_type = child_col_type or "INT(11)"
        # choose engine/charset from child if available
        engine = child_engine or "InnoDB"
        suggested = f"CREATE TABLE `{parent}` (\n  `{parentcol}` {suggested_type} NOT NULL,\n  PRIMARY KEY (`{parentcol}`)\n) ENGINE={engine} DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;"
        out['missing_parents'].append({
            "ParentTable": parent,
            "Reason": "table missing",
            "SuggestedCreateTable": suggested
        })
        processed_missing[parent] = True

    # actions for mismatches or orphan rows
    problems = []
    if fk_entry['SignednessMatch'] == "NO":
        problems.append(("signedness_mismatch", f"ALTER TABLE `{child}` MODIFY `{childcol}` {fk_entry['ParentColumnType']};"))
    if fk_entry['ChildColumnType'] and fk_entry['ParentColumnType'] and fk_entry['ChildColumnType'] != fk_entry['ParentColumnType']:
        problems.append(("type_mismatch", f"ALTER TABLE `{child}` MODIFY `{childcol}` {fk_entry['ParentColumnType']};"))
    if fk_entry['CollationMatch'] == "NO":
        problems.append(("collation_mismatch", f"ALTER TABLE `{child}` MODIFY `{childcol}` {fk_entry['ChildColumnType']} COLLATE {fk_entry['create_table_parent'] and 'utf8mb4_unicode_ci' or 'utf8mb4_unicode_ci'};"))
    if fk_entry['ParentIndexed'] == "NO" and fk_entry['ParentExists'] == "YES":
        problems.append(("parent_not_indexed", f"ALTER TABLE `{parent}` ADD INDEX (`{parentcol}`);"))
    if orphan_count > 0:
        list_sql = f"SELECT * FROM `{child}` WHERE `{childcol}` IS NOT NULL AND `{childcol}` NOT IN (SELECT `{parentcol}` FROM `{parent}`) LIMIT 100;"
        resolve_sql = f"-- Example non-destructive resolution: update orphan rows to NULL\nUPDATE `{child}` SET `{childcol}` = NULL WHERE `{childcol}` IS NOT NULL AND `{childcol}` NOT IN (SELECT `{parentcol}` FROM `{parent}`);"
        problems.append(("orphan_rows", {"list_sql": list_sql, "resolve_sql": resolve_sql}))

    for prob in problems:
        out['actions'].append({
            "ConstraintName": cname,
            "ChildTable": child,
            "ChildColumn": childcol,
            "ParentTable": parent,
            "ParentColumn": parentcol,
            "Problem": prob[0],
            "SafeFixRecommendation": prob[1]
        })

# write JSON
with open("absenceapp_fks_report.json", "w", encoding="utf8") as f:
    json.dump(out, f, indent=2, ensure_ascii=False)

print("Report written to absenceapp_fks_report.json")
conn.close()
