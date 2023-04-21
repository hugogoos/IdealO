# 枚举相关扩展方法
主要包括枚举值、枚举名称、枚举描述之间相互转换。

## 1.根据枚举名称转换成枚举，转换失败则返回空
```
public static T? ToEnum<T>(this string name) where T : struct, Enum
```
## 2.根据枚举名称转换成枚举，转换失败则返回默认枚举值
```
public static T ToEnum<T>(this string name, T defaultValue) where T : struct, Enum 
```
## 3.根据枚举值转换成枚举，转换失败则返回空    
```
public static T? ToEnum<T>(this int value) where T : struct, Enum
```
## 4.根据枚举值转换成枚举，转换失败则返回默认枚举值
```
public static T? ToEnum<T>(this int value, T defaultValue) where T : struct, Enum
```
## 5.将枚举描述转换成枚举值，匹配失败返回空
```
public static T? ToEnumByDescription<T>(this string description) where T : struct, Enum
```
## 6.将枚举描述转换成枚举值，匹配失败返回默认枚举值
```
public static T? ToEnumByDescription<T>(this string description, T defaultValue) where T : struct, Enum  
```
## 7.根据枚举值转换成枚举名称，转换失败则返回当前枚举值
```
public static string ToEnumName<T>(this int value) where T : struct, Enum
```
## 8.根据枚举值转换成枚举名称，转换失败则返回默认枚举名称
```
public static string ToEnumName<T>(this int value, T defaultValue) where T : struct, Enum 
```
## 9.根据枚举名称转换成枚举值，转换失败则返回空
```
public static int? ToEnumValue<T>(this string name) where T : struct, Enum     
```
## 10.根据枚举名称转换成枚举值，转换失败则返回默认枚举值
```
public static int ToEnumValue<T>(this string name, T defaultValue) where T : struct, Enum
```
## 11.获取枚举描述(Descripion)。支持位域，如果是位域组合值，多个按分隔符组合。
```
public static string ToDescription(this Enum source, string split = ",")
```
## 12.获取枚举名称+描述
```
public static Dictionary<string, string> ToEnumNameDescriptions(this Type enumType)
```
## 13.获取枚举值+描述
```
public static Dictionary<int, string> ToEnumValueDescriptions(this Type enumType)   
```
## 14.获取枚举名称+枚举值
```
public static Dictionary<string, int> GetEnumNameValues(this Type enumType)
       
```
## 15.获取枚举值+枚举名称
```
public static Dictionary<int, string> GetEnumValueNames(this Type enumType)
```