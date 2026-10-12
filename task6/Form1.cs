using System.Data;
using Npgsql;

namespace task6;

public partial class Form1 : Form
{
    private const string ConnString = "Host=localhost;Port=5432;Username=postgres;Password=fur43;Database=xz1";

    public Form1()
    {
        InitializeComponent();
        LoadAllClients();
    }

    private void BtnAdd_Click(object? sender, EventArgs e)
    {
        string firstName = txtAddFirstName.Text.Trim();
        string lastName = txtAddLastName.Text.Trim();
        DateTime birthDate = dtpAddBirthDate.Value.Date;
        string email = txtAddEmail.Text.Trim();
        string phoneStr = txtAddPhone.Text.Trim();
        string namePattern = @"^[a-zA-Zа-яА-ЯёЁ-]+$";

        if (!System.Text.RegularExpressions.Regex.IsMatch(firstName, namePattern))
        {
            MessageBox.Show("Имя может содержать только буквы (цифры и спецсимволы запрещены)!", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (!System.Text.RegularExpressions.Regex.IsMatch(lastName, namePattern))
        {
            MessageBox.Show("Фамилия может содержать только буквы (цифры и спецсимволы запрещены)!", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) || string.IsNullOrWhiteSpace(email))
        {
            MessageBox.Show("Поля имени, фамилии и email обязательны для заполнения!", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (!System.Text.RegularExpressions.Regex.IsMatch(phoneStr, @"^\+?[0-9]{10,15}$"))
        {
            MessageBox.Show("Введите корректный номер телефона (например, +79991234567 или 89991234567)!", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        string sql = """
            INSERT INTO clients (first_name, last_name, birth_date, phone, email, registration_date, status)
            VALUES (@first_name, @last_name, @birth_date, @phone, @email, CURRENT_DATE, 'active')
            RETURNING client_id;
            """;

        try
        {
            using var conn = new NpgsqlConnection(ConnString);
            conn.Open();

            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("first_name", firstName);
            cmd.Parameters.AddWithValue("last_name", lastName);
            cmd.Parameters.AddWithValue("birth_date", birthDate);
            cmd.Parameters.AddWithValue("email", email);
            cmd.Parameters.AddWithValue("phone", phoneStr);

            int clientId = (int)cmd.ExecuteScalar()!;
            MessageBox.Show($"Клиент успешно добавлен с ID: {clientId}", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

            txtAddFirstName.Clear();
            txtAddLastName.Clear();
            txtAddEmail.Clear();
            txtAddPhone.Clear();

            LoadAllClients();
        }
        catch (PostgresException ex) when (ex.SqlState == "23505")
        {
            MessageBox.Show($"Ошибка: Клиент с почтой '{email}' уже существует!", "Дубликат", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка БД: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnUpdate_Click(object? sender, EventArgs e)
    {
        if (!int.TryParse(txtUpdateId.Text.Trim(), out int clientId) || clientId <= 0)
        {
            MessageBox.Show("Введите корректный положительный ID!", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        string email = txtUpdateEmail.Text.Trim();
        string phoneStr = txtUpdatePhone.Text.Trim();

        if (string.IsNullOrWhiteSpace(email) || !System.Text.RegularExpressions.Regex.IsMatch(phoneStr, @"^\+?[0-9]{10,15}$"))
        {
            MessageBox.Show("Проверьте правильность email и телефона!", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            using var conn = new NpgsqlConnection(ConnString);
            conn.Open();

            string checkSql = "SELECT first_name, last_name FROM clients WHERE client_id = @id";
            using (var checkCmd = new NpgsqlCommand(checkSql, conn))
            {
                checkCmd.Parameters.AddWithValue("id", clientId);
                using var reader = checkCmd.ExecuteReader();
                if (!reader.Read())
                {
                    MessageBox.Show($"Клиент с ID {clientId} не найден!", "Не найден", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            string sql = """
                UPDATE clients
                SET phone = @phone, email = @email
                WHERE client_id = @client_id;
                """;

            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("client_id", clientId);
            cmd.Parameters.AddWithValue("phone", phoneStr);
            cmd.Parameters.AddWithValue("email", email);

            cmd.ExecuteNonQuery();
            MessageBox.Show("Данные успешно обновлены!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

            txtUpdateId.Clear();
            txtUpdateEmail.Clear();
            txtUpdatePhone.Clear();

            LoadAllClients();
        }
        catch (PostgresException ex) when (ex.SqlState == "23505")
        {
            MessageBox.Show($"Ошибка: Почта '{email}' уже занята!", "Дубликат", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка БД: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnDelete_Click(object? sender, EventArgs e)
    {
        if (!int.TryParse(txtDeleteId.Text.Trim(), out int clientId) || clientId <= 0)
        {
            MessageBox.Show("Введите корректный ID для удаления!", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        string sql = "DELETE FROM clients WHERE client_id = @client_id;";

        try
        {
            using var conn = new NpgsqlConnection(ConnString);
            conn.Open();

            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("client_id", clientId);

            int rows = cmd.ExecuteNonQuery();
            if (rows > 0)
            {
                MessageBox.Show($"Клиент с ID {clientId} удален!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtDeleteId.Clear();
                LoadAllClients();
            }
            else
            {
                MessageBox.Show($"Клиент с ID {clientId} не найден.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка БД: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnShow21_Click(object? sender, EventArgs e)
    {
        string sql = """
            SELECT client_id, first_name, EXTRACT(YEAR FROM age(CURRENT_DATE, birth_date))::int AS age, status
            FROM clients
            WHERE status = 'active' 
              AND EXTRACT(YEAR FROM age(CURRENT_DATE, birth_date)) >= 21
            ORDER BY client_id;
            """;

        ExecuteQueryToGrid(sql);
    }

    private void BtnShowAll_Click(object? sender, EventArgs e)
    {
        LoadAllClients();
    }

    private void LoadAllClients()
    {
        ExecuteQueryToGrid("SELECT * FROM clients where status = 'active' ORDER BY client_id ;");
    }

    private void ExecuteQueryToGrid(string sql)
    {
        try
        {
            using var conn = new NpgsqlConnection(ConnString);
            conn.Open();

            using var cmd = new NpgsqlCommand(sql, conn);
            using var adapter = new NpgsqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            adapter.Fill(dt);

            dgvClients.DataSource = dt;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка загрузки: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
