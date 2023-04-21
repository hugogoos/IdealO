namespace Ideal.Core.Common.Extensions
{
    /// <summary>
    /// 时间相关扩展方法
    /// </summary>
    public static class DateTimeExtension
    {
        /// <summary>
        /// 日期时间转时间戳（秒）
        /// </summary>
        /// <param name="dateTime">日期时间</param>
        /// <returns>时间戳（秒）</returns>
        public static long ToUnixTimestampBySeconds(this DateTime dateTime)
        {
            var dto = new DateTimeOffset(dateTime);
            return dto.ToUnixTimeSeconds();
        }

        /// <summary>
        ///  日期时间转时间戳（毫秒）
        /// </summary> 
        /// <param name="dateTime">日期时间</param>
        /// <returns>时间戳（毫秒）</returns>
        public static long ToUnixTimestampByMilliseconds(this DateTime dateTime)
        {
            var dto = new DateTimeOffset(dateTime);
            return dto.ToUnixTimeMilliseconds();
        }

        /// <summary>
        /// 时间戳（秒）转本地日期时间
        /// </summary>
        /// <param name="timestamp">时间戳（秒）</param>
        /// <returns>本地日期时间</returns>
        public static DateTime ToLocalTimeDateTimeBySeconds(this long timestamp)
        {
            var dto = DateTimeOffset.FromUnixTimeSeconds(timestamp);
            return dto.ToLocalTime().DateTime;
        }

        /// <summary>
        ///  时间戳（秒）转UTC日期时间
        /// </summary> 
        /// <param name="timestamp">时间戳（秒）</param>
        /// <returns>UTC日期时间</returns>
        public static DateTime ToUniversalTimeDateTimeBySeconds(this long timestamp)
        {
            var dto = DateTimeOffset.FromUnixTimeSeconds(timestamp);
            return dto.ToUniversalTime().DateTime;
        }

        /// <summary>
        ///  时间戳（毫秒）转本地日期时间
        /// </summary> 
        /// <param name="timestamp">时间戳（毫秒）</param>
        /// <returns>本地日期时间</returns>
        public static DateTime ToLocalTimeDateTimeByMilliseconds(this long timestamp)
        {
            var dto = DateTimeOffset.FromUnixTimeMilliseconds(timestamp);
            return dto.ToLocalTime().DateTime;
        }

        /// <summary>
        ///  时间戳（毫秒）转UTC日期时间
        /// </summary> 
        /// <param name="timestamp">时间戳（毫秒）</param>
        /// <returns>UTC日期时间</returns>
        public static DateTime ToUniversalTimeDateTimeByMilliseconds(this long timestamp)
        {
            var dto = DateTimeOffset.FromUnixTimeMilliseconds(timestamp);
            return dto.ToUniversalTime().DateTime;
        }

        /// <summary>
        ///  时间戳（秒）转本地日期
        /// </summary> 
        /// <param name="timestamp">时间戳（秒）</param>
        /// <returns>本地日期</returns>
        public static DateOnly ToLocalTimeDateBySeconds(this long timestamp)
        {
            var dt = timestamp.ToLocalTimeDateTimeBySeconds();
            return DateOnly.FromDateTime(dt);
        }

        /// <summary>
        ///  时间戳（秒）转UTC日期
        /// </summary> 
        /// <param name="timestamp">时间戳（秒）</param>
        /// <returns>UTC日期</returns>
        public static DateOnly ToUniversalTimeDateBySeconds(this long timestamp)
        {
            var dt = timestamp.ToUniversalTimeDateTimeBySeconds();
            return DateOnly.FromDateTime(dt);
        }

        /// <summary>
        ///  时间戳（毫秒）转本地日期
        /// </summary> 
        /// <param name="timestamp">时间戳（毫秒）</param>
        /// <returns>本地日期</returns>
        public static DateOnly ToLocalTimeDateByMilliseconds(this long timestamp)
        {
            var dt = timestamp.ToLocalTimeDateTimeByMilliseconds();
            return DateOnly.FromDateTime(dt);
        }

        /// <summary>
        ///  时间戳（毫秒）转UTC日期
        /// </summary> 
        /// <param name="timestamp">时间戳（毫秒）</param>
        /// <returns>UTC日期</returns>
        public static DateOnly ToUniversalTimeDateByMilliseconds(this long timestamp)
        {
            var dt = timestamp.ToUniversalTimeDateTimeByMilliseconds();
            return DateOnly.FromDateTime(dt);
        }

        /// <summary>
        ///  时间戳（秒）转本地时间
        /// </summary> 
        /// <param name="timestamp">时间戳（秒）</param>
        /// <returns>本地时间</returns>
        public static TimeOnly ToLocalTimeTimeBySeconds(this long timestamp)
        {
            var dt = timestamp.ToLocalTimeDateTimeBySeconds();
            return TimeOnly.FromDateTime(dt);
        }

        /// <summary>
        ///  时间戳（秒）转UTC时间
        /// </summary> 
        /// <param name="timestamp">时间戳（秒）</param>
        /// <returns>UTC时间</returns>
        public static TimeOnly ToUniversalTimeTimeBySeconds(this long timestamp)
        {
            var dt = timestamp.ToUniversalTimeDateTimeBySeconds();
            return TimeOnly.FromDateTime(dt);
        }

        /// <summary>
        ///  时间戳（毫秒）转本地时间
        /// </summary> 
        /// <param name="timestamp">时间戳（毫秒）</param>
        /// <returns>本地时间</returns>
        public static TimeOnly ToLocalTimeTimeByMilliseconds(this long timestamp)
        {
            var dt = timestamp.ToLocalTimeDateTimeByMilliseconds();
            return TimeOnly.FromDateTime(dt);
        }

        /// <summary>
        ///  时间戳（毫秒）转UTC时间
        /// </summary> 
        /// <param name="timestamp">时间戳（毫秒）</param>
        /// <returns>UTC时间</returns>
        public static TimeOnly ToUniversalTimeTimeByMilliseconds(this long timestamp)
        {
            var dt = timestamp.ToUniversalTimeDateTimeByMilliseconds();
            return TimeOnly.FromDateTime(dt);
        }

        /// <summary>
        /// 字符串转日期时间，转换失败则返回空
        /// </summary>
        /// <param name="source">需转换的字符串</param>
        /// <returns>日期时间</returns>
        public static DateTime? ToDateTime(this string source)
        {
            if (DateTime.TryParse(source, out var date))
            {
                return date;
            }

            return default;
        }

        /// <summary>
        /// 字符串转日期时间，转换失败则返回默认值
        /// </summary>
        /// <param name="source">需转换的字符串</param>
        /// <param name="dateTime">默认日期时间</param>
        /// <returns>日期时间</returns>
        public static DateTime ToDateTime(this string source, DateTime dateTime)
        {
            if (DateTime.TryParse(source, out var date))
            {
                return date;
            }

            return dateTime;
        }

        /// <summary>
        /// 字符串转日期，转换失败则返回空
        /// </summary>
        /// <param name="source">需转换的字符串</param>
        /// <returns>日期</returns>
        public static DateOnly? ToDateOnly(this string source)
        {
            if (DateOnly.TryParse(source, out var date))
            {
                return date;
            }

            return default;
        }

        /// <summary>
        /// 字符串转日期，转换失败则返回默认日期
        /// </summary>
        /// <param name="source">需转换的字符串</param>
        /// <param name="dateOnly">默认日期</param>
        /// <returns>日期</returns>
        public static DateOnly ToDateOnly(this string source,DateOnly dateOnly)
        {
            if (DateOnly.TryParse(source, out var date))
            {
                return date;
            }

            return dateOnly;
        }

        /// <summary>
        /// 字符串转时间，转换失败则返回空
        /// </summary>
        /// <param name="source">需转换的字符串</param>
        /// <returns>时间</returns>
        public static TimeOnly? ToTimeOnly(this string source)
        {
            if (TimeOnly.TryParse(source, out var date))
            {
                return date;
            }

            return default;
        }

        /// <summary>
        /// 字符串转时间，转换失败则返回默认时间
        /// </summary>
        /// <param name="source">需转换的字符串</param>
        /// <param name="timeOnly">默认时间</param>
        /// <returns>时间</returns>
        public static TimeOnly ToTimeOnly(this string source,TimeOnly timeOnly)
        {
            if (TimeOnly.TryParse(source, out var date))
            {
                return date;
            }

            return timeOnly;
        }
    }
}
