
using Npgsql;


string connstring = "Host=localhost;Port=5432;Username=postgres;Password=fur43;Database=xz1";


using var conn = new NpgsqlConnection(connstring);
try
{
    conn.Open();
    Console.WriteLine("Подключено!");

    while (true)
    {
        Console.WriteLine("=================================");
        Console.WriteLine("Выберите действие:");
        Console.WriteLine("1 - Добавить клиента");
        Console.WriteLine("2 - Удалить клиента");
        Console.WriteLine("3 - Обновить клиента");
        Console.WriteLine("4 - Показать клиентов которым больше 21 года");
        Console.WriteLine("5 - Выйти");
        Console.WriteLine("=================================");
        string choice = Console.ReadLine();
        switch (choice)
        {
            case "1":
                AddClient(conn);
                break;
            case "2":
                DeleteClient(conn);
                break;
            case "3":
                UpdateClient(conn);
                break;
            case "4":
                ShowClients21(conn);
                break;
            case "5":
                Environment.Exit(0);
                break;

            default:
                Console.WriteLine("Неверный ввод!");
                break;
        }
    }
}
catch (NpgsqlException ex)
{
    Console.WriteLine($"Ошибка: {ex.Message}");
}


// метод добавление клиента
static void AddClient(NpgsqlConnection conn)
{
    string sql = """
        INSERT INTO clients (first_name, last_name, birth_date, phone, email, registration_date, status)
        VALUES (@first_name, @last_name, @birth_date, @phone, @email, CURRENT_DATE, 'active')
        RETURNING client_id;
        """;

    // Добавлять клиента (данные из консоли должно брать)
    Console.WriteLine("Введите имя клиента: ");
    string first_name = Console.ReadLine();
    // проверка ну пустое значение
    while (string.IsNullOrWhiteSpace(first_name))
    {
        Console.WriteLine("Имя не может быть пустым! Введите снова: ");
        first_name = Console.ReadLine();
    }
    Console.WriteLine("Введите фамилию клиента: ");
    string last_name = Console.ReadLine();
    while (string.IsNullOrWhiteSpace(last_name))
    {
        Console.WriteLine("Фамилия не может быть пустой! Введите снова: ");
        last_name = Console.ReadLine();
    }
    Console.WriteLine("Введите дату рождения клиента (например, 2000-01-15): ");
    DateTime birth_date;
    // проверяет что ввели именно дату
    while (!DateTime.TryParse(Console.ReadLine(), out birth_date))
    {
        Console.WriteLine("Неверный формат даты. Попробуйте снова.");
    }


    Console.WriteLine("Введите email клиента: ");
    string email = Console.ReadLine();
    // проверка ну пустое значение
    while (string.IsNullOrWhiteSpace(email))
    {
        Console.WriteLine("Email не может быть пустым! Введите снова: ");
        email = Console.ReadLine();
    }


    Console.WriteLine("Введите телефон клиента(без скобок и +): ");
    long phone;
    // проверяет что ввели именно число, long - это тип данных для больших чисел
    while (!long.TryParse(Console.ReadLine(), out phone))
    {
        Console.WriteLine("Неверный формат телефона. Попробуйте снова.");
    }


    using var command = new NpgsqlCommand(sql, conn);
    command.Parameters.AddWithValue("first_name", first_name);
    command.Parameters.AddWithValue("last_name", last_name);
    command.Parameters.AddWithValue("birth_date", birth_date);
    command.Parameters.AddWithValue("email", email);
    command.Parameters.AddWithValue("phone", phone.ToString());



    // обработка ошибок
    try
    {
        int client_id = (int)command.ExecuteScalar();
        Console.WriteLine($"Клиент успешно добавлен с ID: {client_id}");
    }
    // "23505" - это код ошибки, который означает, что email уже существует(дяп, проверка делала ии)
    catch (PostgresException ex) when (ex.SqlState == "23505")
    {
        Console.WriteLine($"Ошибка: Клиент с почтой '{email}' уже зарегистрирован в базе данных!");
    }


}

// метод удаления клиента
static void DeleteClient(NpgsqlConnection conn)
{
    string sql = """
    delete from clients
    where client_id = @client_id;
    """;

    Console.WriteLine("Введите ID клиента для удаления: ");
    int client_id = int.Parse(Console.ReadLine());
    // проверка ну пустое значение
    while (string.IsNullOrWhiteSpace(client_id.ToString()))
    {
        Console.WriteLine("ID не может быть пустым! Введите снова: ");
        client_id = int.Parse(Console.ReadLine());
    }

    using var command = new NpgsqlCommand(sql, conn);
    command.Parameters.AddWithValue("client_id", client_id);

    int row_count = command.ExecuteNonQuery();
    Console.WriteLine($"Удалено строк: {row_count}");

}


// метод обновления клиента
static void UpdateClient(NpgsqlConnection conn)
{
    string sql = """
    UPDATE clients
    SET phone = @phone, email = @email
    WHERE client_id = @client_id;
    """;

    Console.Write("Введите ID клиента для обновления: ");
    int client_id;
    // проверка ну пустое значение (ИИ)
    while (!int.TryParse(Console.ReadLine(), out client_id) || client_id < 0)
    {
        Console.WriteLine("Неверный формат ID. Введите положительное число: ");
    }

    // проверяем, есть ли такой клиент в базе(ИИ)
    string checkSql = "SELECT first_name, last_name FROM clients WHERE client_id = @id";
    using (var checkCmd = new NpgsqlCommand(checkSql, conn))
    {
        checkCmd.Parameters.AddWithValue("id", client_id);
        using var reader = checkCmd.ExecuteReader();

        if (!reader.Read())
        {
            Console.WriteLine($"Клиент с ID {client_id} не найден в базе данных!");
            return; // сразу прерываем метод, не запрашивая телефон и email
        }
        string fullName = $"{reader.GetString(0)} {reader.GetString(1)}";
        Console.WriteLine($"Найден клиент: {fullName}");
    }


    Console.Write("Введите новый телефон(без скобок и +): ");
    long phone;
    while (!long.TryParse(Console.ReadLine(), out phone))
    {
        Console.WriteLine("Неверный формат телефона. Попробуйте снова.");
    }
    Console.Write("Введите новый email: ");
    string email = Console.ReadLine();

    // проверка ну пустое значение
    while (string.IsNullOrWhiteSpace(email))
    {
        Console.WriteLine("Email не может быть пустым! Введите снова: ");
        email = Console.ReadLine();
    }

    using var command = new NpgsqlCommand(sql, conn);
    command.Parameters.AddWithValue("client_id", client_id);
    command.Parameters.AddWithValue("phone", phone.ToString());
    command.Parameters.AddWithValue("email", email);

    try
    {
        command.ExecuteNonQuery();
        Console.WriteLine("Данные клиента успешно обновлены!");
    }
    catch (PostgresException ex) when (ex.SqlState == "23505")
    {
        Console.WriteLine($"Ошибка: Почта '{email}' уже занята другим клиентом!");
    }
}

// вывод пользователей кому больше 21 года
static void ShowClients21(NpgsqlConnection conn)
{
    // extract - извлечение данных(на пример мы взяли год из выражения и переводит в age)
    // age - возвращает интервал (разницу между датами)
    // current_date - текущая дата
    // birth_date - дата рождения
    // ::int - это приведение типа в int
    string sql = """
    SELECT client_id, first_name, EXTRACT(YEAR FROM age(CURRENT_DATE, birth_date))::int AS age, status
    FROM clients
    WHERE status = 'active' 
      AND EXTRACT(YEAR FROM age(CURRENT_DATE, birth_date)) >= 21
    ORDER BY client_id;
    """;

    using var command = new NpgsqlCommand(sql, conn);
    using var reader = command.ExecuteReader();
    Console.WriteLine("ID | ИМЯ | ВОЗРАСТ | СТАТУС");
    while (reader.Read())
    {
        int client_id = reader.GetInt32(0);
        string first_name = reader.GetString(1);
        int age = reader.GetInt32(2);
        string status = reader.GetString(3);
        Console.WriteLine($"{client_id} | {first_name} | {age} | {status}");
    }
}