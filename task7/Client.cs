namespace task6;

public class Client
{
    public int ClientId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    public DateTime BirthDate { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public DateTime RegistrationDate { get; set; }
    public string Status { get; set; } = "active";
}