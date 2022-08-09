using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ideal.Core.Mqtt.Services
{
    public class MqttClientRepository : IMqttClientRepository
    {
        private readonly ILogger<MqttClientRepository> logger;
        private readonly IMqttClient mqttClient;
        private readonly IMqttClientOptions options;
        private Timer _timer = null!;

        public MqttClientRepository(ILogger<MqttClientRepository> logger, IMqttClientOptions options)
        {
            this.logger = logger;
            this.options = options;
            mqttClient = new MqttFactory().CreateMqttClient();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                await mqttClient.ConnectAsync(options);
                if (!mqttClient.IsConnected)
                {
                    await mqttClient.ReconnectAsync();
                }

                logger.LogInformation($"MQTT服务器连接成功！");
                Console.WriteLine($"MQTT服务器连接成功！");
            }
            catch (Exception exp)
            {
                logger.LogError($"MQTT连接服务器失败 Msg：{exp.Message}");
                Console.WriteLine($"MQTT连接服务器失败 Msg：{exp.Message}");
            }

            _timer = new Timer(CheckConnectionStatus, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(5));
        }

        private async void CheckConnectionStatus(object state)
        {
            try
            {
                if (!mqttClient.IsConnected)
                {
                    await mqttClient.ReconnectAsync();
                    logger.LogInformation($"MQTT服务器重连成功！");
                    Console.WriteLine($"MQTT服务器重连成功！");
                }
            }
            catch (Exception exp)
            {
                logger.LogError($"MQTT重连服务器失败 Msg：{exp.Message}");
                Console.WriteLine($"MQTT重连服务器失败 Msg：{exp.Message}");
            }

        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                var disconnectOption = new MqttClientDisconnectOptions
                {
                    ReasonCode = MqttClientDisconnectReason.NormalDisconnection,
                    ReasonString = "NormalDiconnection"
                };
                await mqttClient.DisconnectAsync(disconnectOption, cancellationToken);
                logger.LogInformation($"MQTT服务器停止成功！");
                Console.WriteLine($"MQTT服务器停止成功！");
            }
            await mqttClient.DisconnectAsync();
            logger.LogInformation($"MQTT服务器停止成功！");
            Console.WriteLine($"MQTT服务器停止成功！");
        }

        public IMqttClient GetMqttClient()
        {
            return mqttClient;
        }
    }
}
