$binDir = 'C:\DevAbsence2\AbsenceAppV2\src\AbsenceApp.EfHost\bin\Debug\net8.0'

# Load v7 logging abstractions first (MySqlConnector requires it)
$loggingV7 = "$env:USERPROFILE\.nuget\packages\microsoft.extensions.logging.abstractions\7.0.1\lib\net7.0\Microsoft.Extensions.Logging.Abstractions.dll"
[System.Reflection.Assembly]::LoadFile($loggingV7) | Out-Null

# Load all DLLs in the bin folder to resolve dependencies
Get-ChildItem $binDir -Filter '*.dll' | ForEach-Object {
    try { [System.Reflection.Assembly]::LoadFile($_.FullName) | Out-Null } catch {}
}

$asm = [System.Reflection.Assembly]::LoadFile("$binDir\MySqlConnector.dll")
$connType = $asm.GetType('MySqlConnector.MySqlConnection')
"connType resolved: $($connType -ne $null)"
if ($connType -eq $null) { exit 1 }

$conn = [Activator]::CreateInstance($connType, @('Server=127.0.0.1;Port=3306;Database=absenceapp;User=root;Password=Calm1309!;CharSet=utf8mb4'))
$conn.Open()

$cmd = $conn.CreateCommand()

# Total rows
$cmd.CommandText = 'SELECT COUNT(*) FROM designtokens'
"TOTAL_ROWS=$($cmd.ExecuteScalar())"

# Any CurrentValue overrides?
$cmd.CommandText = "SELECT COUNT(*) FROM designtokens WHERE CurrentValue IS NOT NULL AND CurrentValue != ''"
"CURRENT_VALUE_OVERRIDES=$($cmd.ExecuteScalar())"

# All rows
$cmd.CommandText = 'SELECT Id, ComponentGroup, TokenKey, CssVariable, DefaultValue, CurrentValue FROM designtokens ORDER BY Id'
$r = $cmd.ExecuteReader()
while ($r.Read()) {
    $curVal = if ($r.IsDBNull(5)) { 'NULL' } else { $r.GetString(5) }
    "Id=$($r.GetInt32(0)) | Grp=$($r.GetString(1)) | Key=$($r.GetString(2)) | CSS=$($r.GetString(3)) | CurVal=$curVal"
}
$r.Close()
$conn.Close()
Write-Output "DONE"

$cmd = $conn.CreateCommand()

# Total rows
$cmd.CommandText = 'SELECT COUNT(*) FROM designtokens'
"TOTAL_ROWS=$($cmd.ExecuteScalar())"

# Any CurrentValue overrides?
$cmd.CommandText = "SELECT COUNT(*) FROM designtokens WHERE CurrentValue IS NOT NULL AND CurrentValue != ''"
"CURRENT_VALUE_OVERRIDES=$($cmd.ExecuteScalar())"

# All rows
$cmd.CommandText = 'SELECT Id, ComponentGroup, TokenKey, CssVariable, DefaultValue, CurrentValue FROM designtokens ORDER BY Id'
$r = $cmd.ExecuteReader()
while ($r.Read()) {
    $curVal = if ($r.IsDBNull(5)) { 'NULL' } else { $r.GetString(5) }
    "Id=$($r.GetInt32(0)) | Grp=$($r.GetString(1)) | Key=$($r.GetString(2)) | CSS=$($r.GetString(3)) | CurVal=$curVal"
}
$r.Close()
$conn.Close()
Write-Output "DONE"
