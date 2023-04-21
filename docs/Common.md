# 公共组件
公共组件，用于提供一些常用的帮助方法。

## 扩展方法
### 1.[时间相关扩展方法](Common-Extension-DateTime.md)
### 2.[枚举相关扩展方法](Common-Extension-Enum.md)
### 3.编码解码相关扩展方法
### 4.Linq相关扩展方法

## SnowFlake相关功能
### 获取雪花Id
```
public static string GetNewId()
```
﻿

## AreaHelper相关功能
### 判断点是否在区域内
```
/// <summary>
/// 点是否在区域内
/// </summary>
/// <param name="point">需要判断的点</param>
/// <param name="pts">区域坐标集合</param>
/// <returns></returns>
public static bool IsPtInPoly(LocationModel point, List<LocationModel> pts)
```

## ResultResponse相关功能
### 后台返回前端统一类型
```
/// <summary>
/// 统一返回类型
/// </summary>
/// <typeparam name="T"></typeparam>
public class ResultResponse<T> : ResultResponse
{
	/// <summary>
	/// 获取 返回数据
	/// </summary>
	public T Data { get; set; }
}
​
/// <summary>
/// 统一返回类型
/// </summary>
public class ResultResponse
{
	/// <summary>
	/// 是否成功
	/// </summary>
	public bool IsSucceed { get; set; }
​
	/// <summary>
	/// 操作结果类型
	/// </summary>
	public int Code { get; set; }
	​
	/// <summary>
	/// 获取 消息内容
	/// </summary>
	public string Msg { get; set; }
}
```

## DateTimeHelper相关功能
### 1.时间戳转本地时间（秒﻿）
```
public static DateTime ToLocalTimeDateBySeconds(long timestamp)
```

### 2.时间转时间戳Unix（秒﻿）
```
public static long ToUnixTimestampBySeconds(DateTime dt)
```
### 3.时间戳转本地时间（毫秒）
```
public static DateTime ToLocalTimeDateByMilliseconds(long timestamp)
```
### 4.时间转时间戳Unix（毫秒）
```
public static long ToUnixTimestampByMilliseconds(DateTime dt)
```