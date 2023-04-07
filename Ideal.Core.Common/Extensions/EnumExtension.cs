using System.Collections.Concurrent;
using System.ComponentModel;
using System.Reflection;

namespace Ideal.Core.Common.Extensions
{
    /// <summary>
    /// 枚举扩展
    /// </summary>
    public static class EnumExtension
    {
        private static readonly ConcurrentDictionary<Type, Dictionary<string, string>> _concurrentDicNameDescriptionDictionary = new();
        private static readonly ConcurrentDictionary<Type, Dictionary<int, string>> _concurrentValueDescriptionDictionary = new();
        private static readonly ConcurrentDictionary<Type, Dictionary<string, int>> _concurrentNameValueDictionary = new();
        private static readonly ConcurrentDictionary<Type, Dictionary<int, string>> _concurrentValueNameDictionary = new();
        private static readonly ConcurrentDictionary<Enum, string> _concurrentDescriptionDictionary = new();
        private static readonly ConcurrentDictionary<string, string> _concurrentEnumByDescriptionDictionary = new();

        /// <summary>
        /// 根据枚举名称转换成枚举
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">枚举名称</param>
        /// <returns></returns>
        public static T? ToEnum<T>(this string name) where T : struct, Enum
        {
            Enum.TryParse(typeof(T), name, true, out var result);
            if (result is null)
            {
                return default;
            }

            return (T?)result!;

        }

        /// <summary>
        /// 根据枚举值转换成枚举
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">枚举值</param>
        /// <returns></returns>
        public static T? ToEnum<T>(this int value) where T : struct, Enum
        {
            return value.ToString().ToEnum<T>();
        }

        /// <summary>
        /// 将注释转换成枚举值，匹配不上返回Null
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static T? ToEnumByDescription<T>(this string description) where T : struct, Enum
        {
            var type = typeof(T);
            foreach (var obj in Enum.GetValues(type))
            {
                var nEnum = (Enum)obj;
                if (nEnum.ToDescription() == description)
                {
                    return (T)obj;
                }
            }

            return default;
        }

        /// <summary>
        /// 根据枚举值转换成枚举名称
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">枚举值</param>
        /// <returns></returns>
        public static string ToEnumName<T>(this int value) where T : struct, Enum
        {
            var result = value.ToEnum<T>();
            if (result is null)
            {
                return value.ToString();
            }

            return result.ToString()!;

        }

        /// <summary>
        /// 根据枚举名称转换成枚举值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">枚举名称</param>
        /// <returns></returns>
        public static int? ToEnumValue<T>(this string name) where T : struct, Enum
        {
            var result = name.ToEnum<T>();
            if (result is null)
            {
                return null;

            }

            return result.GetHashCode();
        }

        /// <summary>
        /// 获取枚举的描述信息(Descripion)。
        /// 支持位域，如果是位域组合值，多个按分隔符组合。
        /// </summary>
        /// <param name="source"></param>
        /// <param name="split">位枚举的分割符号（仅对位枚举有作用）</param>
        /// <returns></returns>
        public static string ToDescription(this Enum source, string split = ",")
        {
            return _concurrentDescriptionDictionary.GetOrAdd(source, (key) =>
            {
                var names = key.ToString().Split(',');
                var res = new string[names.Length];
                var type = key.GetType();
                for (var i = 0; i < names.Length; i++)
                {
                    var field = type.GetField(names[i].Trim());
                    if (field == null)
                    {
                        continue;
                    }

                    res[i] = GetFieldDescription(field);
                }

                return string.Join(split, res);
            });
        }

        ///<summary>
        /// 获取枚举名+描述
        ///</summary>
        ///<param name="enumType">枚举的类型</param>
        ///<returns>键值对</returns>
        public static Dictionary<string, string> ToEnumNameDescriptions(this Type enumType)
        {
            var dic = new Dictionary<string, string>();
            if (!enumType.IsEnum)
            {
                return dic;
            }

            return _concurrentDicNameDescriptionDictionary.GetOrAdd(enumType, (key) =>
            {
                var fieldinfos = enumType.GetFields();
                foreach (var field in fieldinfos)
                {
                    if (field.FieldType.IsEnum)
                    {
                        var desc = GetFieldDescription(field);
                        dic.Add(field.Name, desc);
                    }
                }

                return dic;
            });
        }

        ///<summary>
        /// 获取枚举值+描述
        ///</summary>
        ///<param name="enumType">枚举的类型</param>
        ///<returns>键值对</returns>
        public static Dictionary<int, string> ToEnumValueDescriptions(this Type enumType)
        {
            var dic = new Dictionary<int, string>();
            if (!enumType.IsEnum)
            {
                return dic;
            }

            return _concurrentValueDescriptionDictionary.GetOrAdd(enumType, (key) =>
            {
                var fields = enumType.GetFields();
                foreach (var field in fields)
                {
                    if (field.FieldType.IsEnum)
                    {
                        var value = Enum.Parse(enumType, field.Name) as Enum;
                        var desc = GetFieldDescription(field);
                        dic.Add(value!.GetHashCode(), desc);
                    }
                }

                return dic;
            });
        }

        /// <summary>
        /// 获取枚举名+值
        /// </summary>
        ///<param name="enumType">枚举的类型</param>
        /// <returns></returns>
        public static Dictionary<string, int> GetEnumNameValues(this Type enumType)
        {
            var dic = new Dictionary<string, int>();
            if (!enumType.IsEnum)
            {
                return dic;
            }

            return _concurrentNameValueDictionary.GetOrAdd(enumType, (key) =>
            {
                var fields = enumType.GetFields();
                foreach (var field in fields)
                {
                    if (field.FieldType.IsEnum)
                    {
                        var value = Enum.Parse(enumType, field.Name) as Enum;
                        dic.Add(field.Name, value!.GetHashCode());
                    }
                }

                return dic;
            });
        }

        /// <summary>
        /// 获取枚举值+名
        /// </summary>
        ///<param name="enumType">枚举的类型</param>
        /// <returns></returns>
        public static Dictionary<int, string> GetEnumValueNames(this Type enumType)
        {
            var dic = new Dictionary<int, string>();
            if (!enumType.IsEnum)
            {
                return dic;
            }


            return _concurrentValueNameDictionary.GetOrAdd(enumType, (key) =>
            {
                var fields = enumType.GetFields();
                foreach (var field in fields)
                {
                    if (field.FieldType.IsEnum)
                    {
                        var value = Enum.Parse(enumType, field.Name) as Enum;
                        dic.Add(value!.GetHashCode(), field.Name);
                    }
                }

                return dic;
            });
        }

        private static string GetFieldDescription(FieldInfo field)
        {
            var att = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute), false);
            return att == null ? field.Name : ((DescriptionAttribute)att).Description;
        }
    }
}
