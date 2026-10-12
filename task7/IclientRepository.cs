namespace task6;

public interface IClientRepository
{
    Task<List<Client>> GetAllActiveAsync(CancellationToken cancellationToken = default);
    Task<Client?> GetByIdAsync(int clientId, CancellationToken cancellationToken = default);
    Task<int> CreateAsync(Client client, CancellationToken cancellationToken = default);
    Task<bool> UpdateContactAsync(int clientId, string? phone, string? email, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int clientId, CancellationToken cancellationToken = default);
    Task<List<ClientAgeDto>> GetActiveOlderThanAgeAsync(int minAge, CancellationToken cancellationToken = default);
}