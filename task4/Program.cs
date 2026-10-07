using Npgsql;

string connstring = "Host=localhost;Port=5432;Username=postgres;Password=fur43;Database=xz1";


using var conn = new NpgsqlConnection(connstring);
try
{
    conn.Open();
    Console.WriteLine("Подключено!");

    // удаление записи по id
    string sql = """
    delete from clients
    where client_id = @client_id
    """;

    using var command = new NpgsqlCommand(sql, conn);
    command.Parameters.AddWithValue("client_id", 17);

    int row_count = command.ExecuteNonQuery();
    Console.WriteLine($"Удалено строк: {row_count}");

}
catch (NpgsqlException ex)
{
    Console.WriteLine($"Ошибка: {ex.Message}");
}