using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace Ideal.Core.Web
{
    /// <summary>
    /// 生命周期
    /// </summary>
    public enum LifetimesEnum
    {
        /// <summary>
        /// 域
        /// </summary>
        Scoped,
        /// <summary>
        /// 单例
        /// </summary>
        Singleton,
        /// <summary>
        /// 瞬时
        /// </summary>
        Transient
    }

    /// <summary>
    /// 服务集合扩展
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 动态注册接口
        /// </summary>
        /// <param name="services"></param>
        /// <param name="assemblyName">程序集名称</param>
        /// <param name="serviceNameSuffix">服务后缀</param>
        /// <param name="lifetimesEnum">生命周期</param>
        public static void AddRegisters(this IServiceCollection services, string assemblyName, string serviceNameSuffix, LifetimesEnum lifetimesEnum)
        {
            var types = Assembly.Load(assemblyName).GetTypes().Where(k =>
               k.FullName.EndsWith(serviceNameSuffix, StringComparison.InvariantCultureIgnoreCase)
               && k.IsClass
               && !k.IsAbstract
               && !k.IsGenericType);
            foreach (var type in types)
            {
                var interfaceList = type.GetInterfaces();
                if (interfaceList.Length > 0)
                {
                    var inter = interfaceList.FirstOrDefault(item => item.FullName.Contains(type.Name));
                    if (inter != null)
                    {
                        switch (lifetimesEnum)
                        {
                            case LifetimesEnum.Transient:
                                services.AddTransient(inter, type);
                                break;
                            case LifetimesEnum.Singleton:
                                services.AddSingleton(inter, type);
                                break;
                            case LifetimesEnum.Scoped:
                                services.AddScoped(inter, type);
                                break;
                            default:
                                services.AddTransient(inter, type);
                                break;
                        }
                    }
                }
            }
        }
    }
}
