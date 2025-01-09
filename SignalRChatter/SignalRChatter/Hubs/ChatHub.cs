using Microsoft.AspNetCore.SignalR;

namespace SignalRChatter.Hubs;

public class ChatHub(ClientRepository rep) : Hub<IChatServerToClient>, IChatClientToServer
{
    public int GetNrClients()
    {
        //Ned broadcasta till alla clients
        //Clients.All.AdminNotification("Nigasbut");
        return rep._clients.Count;
    }

    public void RegisterTopicsOfInterest(List<string> topicsOfInterest)
    {
        throw new NotImplementedException();
    }

    public void SendMessage(string name, string message, string topic = "")
    {
        Clients.All.NewMessage(name, message, DateTime.Now.ToString());
    }

    public bool SignIn(string username, string password)
    {
        rep._clients.Add(Context.ConnectionId, new Client { Username = username, RegisterTime = DateTime.Now, LastMessageTime = DateTime.Now });
        Clients.All.ClientConnected(username);
        if (username.ToLower().StartsWith("admin"))
        {
            Clients.All.NrClientsChanged(rep._clients.Count);
            return true;
        }
        return false;
    }

    public void SignOut()
    {
        Clients.All.ClientDisconnected(rep._clients[Context.ConnectionId].Username);
        rep._clients.Remove(Context.ConnectionId);
    }
}
