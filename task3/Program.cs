using Npgsql;

string connstring = "Host=localhost;Port=5432;Username=postgres;Password=fur43;Database=xz1";


using var conn = new NpgsqlConnection(connstring);
try
{
    conn.Open();
    Console.WriteLine("Подключено!");

    string sql = """
    insert into clients
    (first_name, last_name, birth_date, phone, email, registration_date, status) 
    values 
    (@first_name, @last_name, @birth_date, @phone, @email, @regDate, @status)
    """;

    using var command = new NpgsqlCommand(sql, conn);
    command.Parameters.AddWithValue("first_name", "Heber");
    command.Parameters.AddWithValue("last_name", "Heck");
    command.Parameters.AddWithValue("birth_date", new DateTime(1990, 1, 1));
    command.Parameters.AddWithValue("phone", "1234567890");
    command.Parameters.AddWithValue("email", "[EMAIL_ADDRESS]");
    command.Parameters.AddWithValue("regDate", DateTime.Now);
    command.Parameters.AddWithValue("status", "active");

    int row_count = command.ExecuteNonQuery();
    Console.WriteLine($"Добавлено строк: {row_count}");




}
catch (NpgsqlException ex)
{
    Console.WriteLine($"Ошибка: {ex.Message}");
}
