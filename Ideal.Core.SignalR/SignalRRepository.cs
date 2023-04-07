using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

namespace Ideal.Core.SignalR
{
    public class SignalRRepository : ISignalRRepository
    {
        private readonly ILogger<SignalRRepository> logger;
        private readonly HubConnection hubConnection;
        private Timer _timer = null!;

        public SignalRRepository(ILogger<SignalRRepository> logger, HubConnection hubConnection)
        {
            this.logger = logger;
            this.hubConnection = hubConnection;
        }

        public HubConnection GetHubConnection()
        {
            return hubConnection;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                await hubConnection.StartAsync();
                logger.LogInformation($"SignalR服务器连接成功！");
                Console.WriteLine("SignalR服务器连接成功！");
            }
            catch (Exception exp)
            {
                logger.LogError($"SignalR服务器连接失败 Msg：{exp.Message}");
                Console.WriteLine($"SignalR服务器连接失败 Msg：{exp.Message}");
            }

            _timer = new Timer(CheckConnectionStatus, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(5));
        }

        private async void CheckConnectionStatus(object state)
        {
            try
            {
                if (hubConnection.State == HubConnectionState.Disconnected)
                {
                    await hubConnection.StartAsync();
                    logger.LogInformation($"SignalR服务器重连成功！");
                    Console.WriteLine("SignalR服务器重连成功！");
                }
            }
            catch (Exception exp)
            {
                logger.LogError($"SignalR重连服务器失败 Msg：{exp.Message}");
                Console.WriteLine($"SignalR重连服务器失败 Msg：{exp.Message}");
            }

        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                await hubConnection.StopAsync(cancellationToken);
                logger.LogInformation($"SignalR服务器停止成功！");
                Console.WriteLine($"SignalR服务器停止成功！");
            }

            await hubConnection.StopAsync();
            logger.LogInformation($"SignalR服务器停止成功！");
            Console.WriteLine($"SignalR服务器停止成功！");
        }
    }
}
