
using Npgsql;

string connstring = "Host=localhost;Port=5432;Username=postgres;Password=fur43;Database=xz1";


using var conn = new NpgsqlConnection(connstring);
try
{
    conn.Open();
    Console.WriteLine("Подключено!");




    string sql = """
    SELECT client_id, first_name, last_name, phone, status 
    FROM clients
    WHERE status = @status
    """;

    using var command = new NpgsqlCommand(sql, conn);
    command.Parameters.AddWithValue("status", "active");


    using var reader = command.ExecuteReader();
    while (reader.Read())
    {
        int client_id = reader.GetInt32(0);
        string first_name = reader.GetString(1);
        string last_name = reader.GetString(2);
        string status = reader.GetString(4);
        string phone = reader.GetString(3);
        Console.WriteLine($"{client_id} {first_name} {last_name} {phone} {status}");
    }


}
catch (NpgsqlException ex)
{
    Console.WriteLine($"Ошибка: {ex.Message}");
}