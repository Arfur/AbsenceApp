using MySqlConnector;
var conn = new MySqlConnection("Server=127.0.0.1;Port=3306;Database=absenceapp;Uid=root;Pwd=Calm1309!;");
await conn.OpenAsync();
var cmd = new MySqlCommand("SELECT TABLE_NAME FROM information_schema.TABLES WHERE TABLE_SCHEMA='absenceapp' AND TABLE_NAME IN ('staffdeviceaudits','staffexternalaccountsaudits')", conn);
var r = await cmd.ExecuteReaderAsync();
while (await r.ReadAsync()) Console.WriteLine("CONFIRMED: " + r.GetString(0));
