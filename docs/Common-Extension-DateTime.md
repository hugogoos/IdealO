# 时间相关扩展方法
主要包括时间戳与日期时间、日期、时间之间转换，以及字符串转日期时间、日期、时间。

## 1.日期时间转时间戳（秒）
```
public static long ToUnixTimestampBySeconds(this DateTime dateTime)
```
## 2.日期时间转时间戳（毫秒）
```
public static long ToUnixTimestampByMilliseconds(this DateTime dateTime)     
```
## 3.时间戳（秒）转本地日期时间
```
public static DateTime ToLocalTimeDateTimeBySeconds(this long timestamp)        
```
## 4.时间戳（秒）转UTC日期时间
```
public static DateTime ToUniversalTimeDateTimeBySeconds(this long timestamp)        
```
## 5.时间戳（毫秒）转本地日期时间
```
public static DateTime ToLocalTimeDateTimeByMilliseconds(this long timestamp)       
```
## 6.时间戳（毫秒）转UTC日期时间
```
public static DateTime ToUniversalTimeDateTimeByMilliseconds(this long timestamp)        
```
## 7.时间戳（秒）转本地日期
```
public static DateOnly ToLocalTimeDateBySeconds(this long timestamp)       
```
## 8.时间戳（秒）转UTC日期
```
public static DateOnly ToUniversalTimeDateBySeconds(this long timestamp)        
```
## 9.时间戳（毫秒）转本地日期
```
public static DateOnly ToLocalTimeDateByMilliseconds(this long timestamp)       
```
## 10.时间戳（毫秒）转UTC日期
```
public static DateOnly ToUniversalTimeDateByMilliseconds(this long timestamp)       
```
## 11.时间戳（秒）转本地时间
```
public static TimeOnly ToLocalTimeTimeBySeconds(this long timestamp)        
```
## 12.时间戳（秒）转UTC时间
```
public static TimeOnly ToUniversalTimeTimeBySeconds(this long timestamp)        
```
## 13.时间戳（毫秒）转本地时间
```
public static TimeOnly ToLocalTimeTimeByMilliseconds(this long timestamp)        
```
## 14.时间戳（毫秒）转UTC时间
```
public static TimeOnly ToUniversalTimeTimeByMilliseconds(this long timestamp)        
```
## 15.字符串转日期时间，转换失败则返回空
```
public static DateTime? ToDateTime(this string source)        
```
## 16.字符串转日期时间，转换失败则返回默认值
```
public static DateTime ToDateTime(this string source, DateTime dateTime)        
```
## 17.字符串转日期，转换失败则返回空
```
public static DateOnly? ToDateOnly(this string source)        
```
## 18.字符串转日期，转换失败则返回默认日期
```
public static DateOnly ToDateOnly(this string source,DateOnly dateOnly)        
```
## 19.字符串转时间，转换失败则返回空
```
public static TimeOnly? ToTimeOnly(this string source)        
```
## 20.字符串转时间，转换失败则返回默认时间
```
public static TimeOnly ToTimeOnly(this string source,TimeOnly timeOnly)
```