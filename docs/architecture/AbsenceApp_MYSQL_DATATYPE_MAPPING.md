================================================================================
 File        : AbsenceApp_MYSQL_DATATYPE_MAPPING.md
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-04-16
 Updated     : 2026-04-16
--------------------------------------------------------------------------------
 Purpose     :
   Provides a definitive mapping between SQL Server and MySQL datatypes used in
   AbsenceApp. This prevents drift, incorrect assumptions, and inconsistent
   migrations between environments.

   This file exists to:
     - Ensure deterministic SQL Server → MySQL migrations
     - Provide a single source of truth for datatype equivalence
     - Prevent accidental use of incompatible or legacy types
--------------------------------------------------------------------------------
 Notes       :
   - Only datatypes present in the actual AbsenceApp schemas are included.
   - Mappings reflect real usage, not theoretical equivalence.
================================================================================


# SQL Server → MySQL Datatype Mapping

+---------------------------+---------------------------+-----------------------------------------------+
| SQL Server Type           | MySQL Type               | Notes                                         |
+---------------------------+---------------------------+-----------------------------------------------+
| int                       | int                      | Direct equivalent                             |
| bigint                    | bigint                   | Direct equivalent                             |
| smallint                  | smallint                 | Not used in MySQL schema but included         |
| tinyint                   | tinyint                  | Same semantics (0–255)                        |
| decimal(p,s)              | decimal(p,s)             | Direct equivalent                             |
| float                     | double                   | MySQL float is 32‑bit; double matches SQL     |
| bit                       | tinyint(1)               | MySQL has no true boolean                     |
| nvarchar(n)               | varchar(n)               | MySQL varchar is UTF‑8 by default             |
| varchar(n)                | varchar(n)               | Direct equivalent                             |
| text                      | text                     | Direct equivalent                             |
| ntext                     | longtext                 | MySQL longtext used for large Unicode         |
| datetime2                 | datetime                 | MySQL datetime(6) optional                    |
| datetime                  | datetime                 | Direct equivalent                             |
| date                      | date                     | Direct equivalent                             |
| time                      | time                     | Direct equivalent                             |
| uniqueidentifier          | char(36)                 | MySQL has no GUID type                        |
| varbinary(max)            | longblob                 | Not used in AbsenceApp                        |
| xml                       | longtext                 | MySQL has no XML type                         |
+---------------------------+---------------------------+-----------------------------------------------+
