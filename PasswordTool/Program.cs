using BCrypt.Net;

Console.Write("Enter password: ");
string password = Console.ReadLine();

string hash = BCrypt.Net.BCrypt.HashPassword(password, 12);

Console.WriteLine("\nBcrypt Hash:");
Console.WriteLine(hash);
