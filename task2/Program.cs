using Npgsql;

string connstring = "Host=localhost;Port=5432;Username=postgres;Password=fur43;Database=xz1";


using var conn = new NpgsqlConnection(connstring);
try
{
    conn.Open();
    Console.WriteLine("Подключено!");




    string sql = """
    update clients
    set phone = @phone
    WHERE client_id = @client_id
    """;

    using var command = new NpgsqlCommand(sql, conn);
    command.Parameters.AddWithValue("phone", "1234567890");
    command.Parameters.AddWithValue("client_id", 1);




    int row_count = command.ExecuteNonQuery();
    Console.WriteLine($"Изменено строк: {row_count}");


}
catch (NpgsqlException ex)
{
    Console.WriteLine($"Ошибка: {ex.Message}");
}
