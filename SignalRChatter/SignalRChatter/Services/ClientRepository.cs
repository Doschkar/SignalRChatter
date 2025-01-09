namespace SignalRChatter.Services;

public class ClientRepository
{
    public readonly Dictionary<string, Client> _clients = new();
}

public class Client
{
    public string Username { get; set; } = null!;
    public DateTime RegisterTime { get; set; }
    public DateTime LastMessageTime { get; set; }
}