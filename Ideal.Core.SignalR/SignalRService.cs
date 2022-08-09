using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

namespace Ideal.Core.SignalR
{
    public class SignalRService : ISignalRService
    {
        private readonly ILogger<SignalRService> logger;
        private readonly HubConnection hubConnection;

        public SignalRService(ILogger<SignalRService> logger, SignalRRepositoryProvider signalRRepositoryProvider)
        {
            this.logger = logger;
            hubConnection = signalRRepositoryProvider.SignalRRepository.GetHubConnection();
        }

        public HubConnection GetHubConnection()
        {
            return hubConnection;
        }
    }
}
