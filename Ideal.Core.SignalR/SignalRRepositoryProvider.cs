namespace Ideal.Core.SignalR
{
    public class SignalRRepositoryProvider
    {
        public readonly ISignalRRepository SignalRRepository;

        public SignalRRepositoryProvider(ISignalRRepository signalRRepository)
        {
            SignalRRepository = signalRRepository;
        }
    }
}
