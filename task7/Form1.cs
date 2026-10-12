using System.Data;
using Npgsql;

namespace task6;

public partial class Form1 : Form
{
    private const string ConnString = "Host=localhost;Port=5432;Username=postgres;Password=fur43;Database=xz1";

    public Form1()
    {
        InitializeComponent();
        _repository = new ClientRepository(ConnString);
        this.LoadAllClients += async (s, e) => await LoadAllClientsAsync();

    }

    private async void BtnAdd_Click(object? sender, EventArgs e)
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

        var client = new Client
        {
            FirstName = firstName,
            LastName = lastName,
            BirthDate = birthDate,
            Email = email,
            Phone = phoneStr,
            Status = "active"
        };

        try
        {
            int clientId = await _repository.AddAsync(client);
            MessageBox.Show($"Клиент успешно добавлен с ID: {clientId}", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

            txtAddFirstName.Clear();
            txtAddLastName.Clear();
            txtAddEmail.Clear();
            txtAddPhone.Clear();

            await LoadAllClientsAsync();
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

    private async Task BtnUpdate_Click(object? sender, EventArgs e)
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
            var existingClient = await _repository.GetByIdAsync(clientId);
            if (existingClient == null)
            {
                MessageBox.Show($"Клиент с ID {clientId} не найден!", "Не найден", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

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

            bool updated = await _repository.UpdateAsync(existingClient);
            if (updated)
            {
                MessageBox.Show("Данные успешно обновлены!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtUpdateId.Clear();
                txtUpdateEmail.Clear();
                txtUpdatePhone.Clear();

                await LoadAllClientsAsync();
            }
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



        try
        {
            bool deleted = await _repository.DeleteAsync(clientId);
            if (deleted)
            {
                MessageBox.Show($"Клиент с ID {clientId} удален!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtDeleteId.Clear();
                await LoadAllClientsAsync();
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

    private async Task BtnShow21_Click(object? sender, EventArgs e)
    {
        try
        {
            var client21 = await _repository.GetActiveOlderThanAgeAsync(21);
            dgvClients.DataSource = null;
            dgvClients.DataSource = client21;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка БД: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task BtnShowAll_Click(object? sender, EventArgs e)
    {
        await LoadAllClientsAsync();
    }

    private async Task LoadAllClientsAsync()
    {
        try
        {
            var clients = await _repository.GetAllActiveAsync();
            dgvClients.DataSource = null;
            dgvClients.DataSource = clients;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка БД: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        base.OnFormClosed(e);
        if (_repository is IDisposable disposableRepo)
        {
            disposableRepo.Dispose();
        }
    }
}
