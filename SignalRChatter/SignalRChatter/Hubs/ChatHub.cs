using Microsoft.AspNetCore.SignalR;

namespace SignalRChatter.Hubs;

public class ChatHub(ClientRepository rep) : Hub<IChatServerToClient>, IChatClientToServer
{
    public override Task OnConnectedAsync()
    {
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        return base.OnDisconnectedAsync(exception);
    }

    public int GetNrClients()
    {
        foreach (var client in rep._clients)
        {
            if (client.Value.Username.ToLower().StartsWith("admin"))
            {
                Clients.Client(client.Key).AdminNotification($"Der Admin {client.Value.Username} hat die Anzahl der Clients angefragt.");
            }
        }

        return rep._clients.Count;
    }

    public void RegisterTopicsOfInterest(List<string> topicsOfInterest)
    {
        throw new NotImplementedException();
    }

    public void SendMessage(string name, string message, string topic = "")
    {
        this.Log(name, message);
        rep._clients.Where(c => c.Key == Context.ConnectionId).FirstOrDefault().Value.LastMessageTime = DateTime.Now;
        Clients.All.NewMessage(name, message, DateTime.Now.ToString());
    }

    public bool SignIn(string username, string password)
    {
        this.Log(username, password);
        rep._clients.Add(Context.ConnectionId, new Client { Username = username, RegisterTime = DateTime.Now, LastMessageTime = DateTime.Now });
        Clients.All.ClientConnected(username);

        foreach (var client in rep._clients)
        {
            if (client.Value.Username.ToLower().StartsWith("admin"))
            {
                Clients.Client(client.Key).NrClientsChanged(rep._clients.Count);
            }
        }

        if (username.ToLower().StartsWith("admin"))
        {
            return true;
        }

        return false;
    }

    public void SignOut()
    {
        this.Log("SignOut");

        Clients.All.ClientDisconnected(rep._clients[Context.ConnectionId].Username);
        rep._clients.Remove(Context.ConnectionId);

        foreach (var client in rep._clients)
        {
            if (client.Value.Username.ToLower().StartsWith("admin"))
            {
                Clients.Client(client.Key).NrClientsChanged(rep._clients.Count);
            }
        }
    }
}
