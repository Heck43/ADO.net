using Npgsql;
namespace task6;

public class ClientRepository : IClientRepository, IDisposable, IAsyncDisposable
{
    private readonly NpgsqDataSourse _dataSourse;
    private readonly bool _ownsDataSourse;
    public ClientRepository(string connectionString)
    {
        _dataSourse = NpgsqlDataSource.Create(connectionString);
        _ownsDataSourse = true;
    }

    public ClientRepository(NpgsqDataSourse dataSourse)
    {
        _dataSourse = dataSourse;
        _ownsDataSourse = false;
    }
    public async Task<List<Client>> GetAllActiveAsync(CancellationToken cancellationToken = default)
    {
        var clients = new List<Client>();
        await using var command = _dataSourse.CreateCommand("""
     Select client_id, first_name, last_name, birth_date, phone, email, registration_date, status 
     FROM clients 
     WHERE status = 'active'
     ORDER BY client_id;
     """);
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            clients.Add(MapClient(reader));
        }
        return clients;
    }
    public async Task<List<Client>> GetActiveOlderThanAgeAsync(int age, CancellationToken cancellationToken = default)
    {
        var clients = new List<Client>();
        await using var command = _dataSourse.CreateCommand("""
        Select client_id, first_name, last_name, birth_date, phone, email, registration_date, status 
        FROM clients 
        WHERE client_id = $1
        """);
        command.Parameters.AddWithValue(age);
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        if (!await reader.ReadAsync(cancellationToken))
        {
            return null;
        }
        return MapClient(reader);
    }

    public async Task<int> CreateAsync(Client client, CancellationToken cancellationToken = default)
    {
        await using var command = _dataSourse.CreateCommand("""
        INSERT INTO clients (first_name, last_name, birth_date, phone, email, registration_date, status)
        VALUES ($1, $2, $3, $4, $5, $6, $7)
        RETURNING client_id;
        """);

        command.Parameters.AddWithValue(client.FirstName.Trim());
        command.Parameters.AddWithValue(client.LastName.Trim());
        command.Parameters.AddWithValue(client.BirthDate);
        command.Parameters.AddWithValue(client.Phone ?? DBNull.Value);
        command.Parameters.AddWithValue(client.Email ?? DBNull.Value);
        command.Parameters.AddWithValue(DateTime.UtcNow);
        command.Parameters.AddWithValue(string.IsNullOrEmpty(client.Status) ? "active" : client.Status);

        object? result = await command.ExecuteScalarAsync(cancellationToken);
        return Convert.ToInt32(result);
    }



}