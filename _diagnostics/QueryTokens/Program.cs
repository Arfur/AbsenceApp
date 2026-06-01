using MySqlConnector;
using System;
using System.Threading.Tasks;

const string connStr = "Server=127.0.0.1;Port=3306;Database=absenceapp;User=root;Password=Calm1309!;CharSet=utf8mb4";

await using var conn = new MySqlConnection(connStr);
await conn.OpenAsync();

var groups = new[] { "card", "form-field", "form-shell", "nav-sidebar", "table", "alert", "icon-btn", "badge-status", "action-btn", "dd" };

foreach (var group in groups)
{
    Console.WriteLine($"\n=== GROUP: {group} ===");
    await using var cmd = conn.CreateCommand();
    cmd.CommandText = $"SELECT Id, TokenKey, CssVariable, DefaultValue, SortOrder FROM designtokens WHERE ComponentGroup = '{group}' ORDER BY SortOrder, Id";
    await using var r = await cmd.ExecuteReaderAsync();
    while (await r.ReadAsync())
        Console.WriteLine($"Id={r.GetInt32(0)} | Key={r.GetString(1)} | CSS={r.GetString(2)} | Def={r.GetString(3)} | Sort={r.GetInt32(4)}");
}
