using Microsoft.AspNetCore.SignalR.Client;

namespace Ideal.Core.SignalR
{
    public interface ISignalRService
    {
        HubConnection GetHubConnection();
    }
}
