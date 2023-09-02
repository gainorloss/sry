using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using Microsoft.Extensions.DependencyModel;
using System.Reflection;

namespace Sample.PluginApp.ConsoleApp;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDynamicProxy<TService>(this IServiceCollection services)
    {
        services.AddSingleton<ProxyGenerator>();

        var type = typeof(TService);
        var serviceType = type.GetInterfaces().First();
        var interceptors = serviceType.GetCustomAttributes(false).Where(i => i.GetType().IsAssignableTo(typeof(IInterceptor))).Select(i => (IInterceptor)i);
        if (!interceptors.Any())
        {
            services.AddTransient(serviceType, type);
            return services;
        }

        services.AddTransient(type);
        services.AddTransient(serviceType, sp =>
        {
            var generator = sp.GetRequiredService<ProxyGenerator>();
            var proxy = generator.CreateInterfaceProxyWithTargetInterface(serviceType, sp.GetRequiredService(type), interceptors.ToArray());
            return proxy;
        });

        return services;
    }

}