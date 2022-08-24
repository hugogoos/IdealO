using System;

namespace Ideal.Core.Common.Helpers
{
    /// <summary>
    /// 时间处理帮助类
    /// </summary>
    public class DateTimeHelper
    {
        /// <summary>
        ///  时间戳转本地时间-时间戳精确到秒
        /// </summary> 
        [Obsolete("该方法已弃用，请直接使用long类型相对应的同名扩展方法", true)]
        public static DateTime ToLocalTimeDateBySeconds(long unix)
        {
            var dto = DateTimeOffset.FromUnixTimeSeconds(unix);
            return dto.ToLocalTime().DateTime;
        }

        /// <summary>
        ///  时间转时间戳Unix-时间戳精确到秒
        /// </summary> 
        [Obsolete("该方法已弃用，请直接使用DateTime类型相对应的同名扩展方法", true)]
        public static long ToUnixTimestampBySeconds(DateTime dt)
        {
            var dto = new DateTimeOffset(dt);
            return dto.ToUnixTimeSeconds();
        }

        /// <summary>
        ///  时间戳转本地时间-时间戳精确到毫秒
        /// </summary> 
        [Obsolete("该方法已弃用，请直接使用long类型相对应的同名扩展方法", true)]
        public static DateTime ToLocalTimeDateByMilliseconds(long unix)
        {
            var dto = DateTimeOffset.FromUnixTimeMilliseconds(unix);
            return dto.ToLocalTime().DateTime;
        }

        /// <summary>
        ///  时间转时间戳Unix-时间戳精确到毫秒
        /// </summary> 
        [Obsolete("该方法已弃用，请直接使用DateTime类型相对应的同名扩展方法", true)]
        public static long ToUnixTimestampByMilliseconds(DateTime dt)
        {
            var dto = new DateTimeOffset(dt);
            return dto.ToUnixTimeMilliseconds();
        }
    }
}
