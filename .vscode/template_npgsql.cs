using Npgsql;

string connstring = "Host=localhost;Port=5432;Username=postgres;Password=fur43;Database=xz1";


using var conn = new NpgsqlConnection(connstring);
try
{
    conn.Open();
    Console.WriteLine("Подключено!");
    string sql = @"
    
    
    ";
    
}
catch (NpgsqlException ex)
{
    Console.WriteLine($"Ошиека: {ex.Message}");
}
