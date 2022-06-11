using Microsoft.AspNetCore.SignalR;

namespace Paladin.WebApp.Pages.Environments;

public class EnvironmentHub : Hub
{
    public async Task RefreshAsync()
    {
        await Clients.All.SendAsync("Refresh");
    }
}
