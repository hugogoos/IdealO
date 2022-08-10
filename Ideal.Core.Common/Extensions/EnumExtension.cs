using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace Ideal.Core.Common.Extensions
{
    /// <summary>
    /// 枚举扩展
    /// </summary>
    public static class EnumExtension
    {
        private static readonly ConcurrentDictionary<Type, Dictionary<string, string>> _concurrentDicDictionary = new();
        private static readonly ConcurrentDictionary<Enum, string> _concurrentDictionary = new();

        /// <summary>
        /// 根据枚举名称转换成枚举
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">枚举名称</param>
        /// <returns></returns>
        public static T? ToEnum<T>(this string name)
        {
            Enum.TryParse(typeof(T), name, true, out var result);
            if (result is null)
            {
                return default;

            }

            return (T)result;
        }

        /// <summary>
        /// 根据枚举值转换成枚举
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">枚举值</param>
        /// <returns></returns>
        public static T? ToEnum<T>(this int value)
        {
            Enum.TryParse(typeof(T), Enum.GetName(typeof(T), value), true, out var result);
            if (result is null)
            {
                return default;

            }

            return (T)result;
        }

        /// <summary>
        /// 根据枚举值转换成枚举名称
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">枚举值</param>
        /// <returns></returns>
        public static string ToEnumName<T>(this int value)
        {
            var result = Enum.GetName(typeof(T), value);
            if (result is null)
            {
                return value.ToString();

            }

            return result;
        }

        /// <summary>
        /// 根据枚举名称转换成枚举值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">枚举名称</param>
        /// <returns></returns>
        public static int? ToEnumValue<T>(this string name)
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
        /// <returns></returns>
        public static string ToDescription(this Enum source)
        {
            return _concurrentDictionary.GetOrAdd(source, (key) =>
            {
                var type = key.GetType();
                var field = type.GetField(key.ToString());
                //如果field为null则应该是组合位域值，
                return field == null ? key.ToDescriptions() : GetFieldDescription(field);
            });
        }

        /// <summary>
        /// 获取枚举的说明
        /// </summary>
        /// <param name="source"></param>
        /// <param name="split">位枚举的分割符号（仅对位枚举有作用）</param>
        public static string ToDescriptions(this Enum source, string split = ",")
        {
            var names = source.ToString().Split(',');
            var res = new string[names.Length];
            var type = source.GetType();
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
        }

        ///<summary>
        /// 获取枚举项+描述
        ///</summary>
        ///<param name="enumType">Type,该参数的格式为typeof(需要读的枚举类型)</param>
        ///<returns>键值对</returns>
        public static Dictionary<string, string> GetEnumItemDesc(Type enumType)
        {
            var dic = new Dictionary<string, string>();
            var fieldinfos = enumType.GetFields();
            foreach (var field in fieldinfos)
            {
                if (field.FieldType.IsEnum)
                {
                    var objs = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
                    dic.Add(field.Name, ((DescriptionAttribute)objs[0]).Description);
                }
            }
            return dic;
        }

        ///<summary>
        /// 获取枚举值+描述
        ///</summary>
        ///<param name="enumType">Type,该参数的格式为typeof(需要读的枚举类型)</param>
        ///<returns>键值对</returns>
        public static Dictionary<string, string> GetEnumItemValueDesc(Type enumType)
        {
            var dic = new Dictionary<string, string>();
            var typeDescription = typeof(DescriptionAttribute);
            var fields = enumType.GetFields();
            foreach (var field in fields)
            {
                if (field.FieldType.IsEnum)
                {
                    var strValue = ((int)enumType.InvokeMember(field.Name, BindingFlags.GetField, null, null, null)).ToString();
                    var arr = field.GetCustomAttributes(typeDescription, true);
                    string strText;
                    if (arr.Length > 0)
                    {
                        var aa = (DescriptionAttribute)arr[0];
                        strText = aa.Description;
                    }
                    else
                    {
                        strText = field.Name;
                    }
                    dic.Add(strValue, strText);
                }
            }

            return dic;
        }

        /// <summary>
        /// 将注释转换成枚举值，匹配不上返回Null
        /// </summary>
        /// <param name="type"></param>
        /// <param name="strDescription"></param>
        /// <returns></returns>
        public static int? GetEnumValByDescription(this Type type, string strDescription)
        {
            int? enumVal = null;
            foreach (var obj in Enum.GetValues(type))
            {
                var nEnum = (Enum)obj;
                if (nEnum.ToDescription() == strDescription)
                {
                    enumVal = (int)Convert.ChangeType(nEnum, typeof(int));
                }
            }
            return enumVal;
        }

        /// <summary>
        /// 获取枚举类型键值对
        /// </summary>
        /// <param name="em"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetEunItemValueAndDesc(Type em)
        {
            return _concurrentDicDictionary.GetOrAdd(em, (key) =>
            {
                var type = key.GetType();
                if (_concurrentDicDictionary.ContainsKey(key))
                {
                    return _concurrentDicDictionary[key];
                }
                else
                {
                    return GetEnumItemValueDesc(em);
                }
            });
        }

        private static string GetFieldDescription(FieldInfo field)
        {
            var att = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute), false);
            return att == null ? field.Name : ((DescriptionAttribute)att).Description;
        }
    }
}
