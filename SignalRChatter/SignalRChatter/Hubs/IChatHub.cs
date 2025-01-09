namespace SignalRChatter.Hubs;

public interface IChatServerToClient
{
    Task NewMessage(string name, string message, string timestamp);
    Task ClientConnected(string name);
    Task ClientDisconnected(string name);
    Task AdminNotification(string message);
    Task NrClientsChanged(int nr);
}

public interface IChatClientToServer
{
    int GetNrClients();
    void SendMessage(string name, string message, string topic = "");
    void RegisterTopicsOfInterest(List<string> topicsOfInterest);
    bool SignIn(string username, string password);
    void SignOut();
}