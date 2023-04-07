namespace Ideal.Core.Quartz
{
    public class JobSchedule
    {
        public JobSchedule(Type jobType, TriggerWayEnum triggerWay = TriggerWayEnum.Simple, int milliseconds = 10000, string? cron = null)
        {
            JobType = jobType;
            Milliseconds = milliseconds;
            Cron = cron;
            TriggerWay = triggerWay;
        }

        public Type JobType { get; }
        public int Milliseconds { get; }
        public string? Cron { get; set; }
        public TriggerWayEnum TriggerWay { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public enum TriggerWayEnum
    {
        /// <summary>
        /// Simple
        /// </summary>
        Simple = 1,

        /// <summary>
        /// Cron
        /// </summary>
        Cron = 2,
    }
}
