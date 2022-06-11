using Microsoft.AspNetCore.SignalR;

namespace Paladin.WebApp.Pages.Environments
{
    public class RefreshBackgroundService : BackgroundService
    {
        private readonly IHubContext<EnvironmentHub> _environmentHub;

        public RefreshBackgroundService(IHubContext<EnvironmentHub> environmentHub)
        {
            _environmentHub = environmentHub;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var timer = new PeriodicTimer(TimeSpan.FromSeconds(1));

            while (await timer.WaitForNextTickAsync(CancellationToken.None))
            {
                await _environmentHub.Clients.All.SendAsync("Refresh");
            }
        }
    }
}
