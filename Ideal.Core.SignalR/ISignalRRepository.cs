using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;

namespace Ideal.Core.SignalR
{
    public interface ISignalRRepository : IHostedService
    {
        HubConnection GetHubConnection();
    }
}
