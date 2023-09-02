using FluentValidation;
using GaloS.Wpf.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMvvm(this IServiceCollection services)
        {
            var libs = DependencyContext.Default.RuntimeLibraries.Where(i => i.Type.Equals("project"))
                .Select(i => Assembly.LoadFrom(Path.Combine(AppContext.BaseDirectory, $"{i.Name}.dll")));
            var classTypes = libs.SelectMany(i => i.GetTypes()).Where(t => t.IsClass && !t.IsGenericType && !t.IsAbstract);
            var vmTypes = classTypes.Where(i => i.Name.EndsWith("ViewModel"));
            foreach (var vmType in vmTypes)
                services.TryAddTransient(vmType);

            var vTypes = classTypes.Where(i => i.Name.EndsWith("Page") || i.Name.EndsWith("View") || i.Name.EndsWith("Window"));
            foreach (var vType in vTypes)
            {
                var vmType = vmTypes.FirstOrDefault(i => i.Assembly.FullName.Equals(vType.Assembly.FullName) && i.Name.Contains(vType.Name));
                services.TryAddTransient(vType, sp =>
                {
                    var instance = Activator.CreateInstance(vType);
                    var dc = vType.GetProperty("DataContext");
                    if (dc != null && vmType != null)
                    {
                        var viewModel = sp.GetRequiredService(vmType);
                        dc.SetValue(instance, viewModel);
                    }
                    return instance;
                });
            }

            Trace.WriteLine($"ViewModels:{vmTypes.Count()};Views:{vTypes.Count()},", "加载组件");
            return services;
        }

        public static IServiceCollection AddValidation(this IServiceCollection services)
        {
            var libs = DependencyContext.Default.RuntimeLibraries.Where(i => i.Type.Equals("project"))
                    .Select(i => Assembly.LoadFrom(Path.Combine(AppContext.BaseDirectory, $"{i.Name}.dll")));
            var classTypes = libs.SelectMany(i => i.GetTypes()).Where(t => t.IsClass && !t.IsGenericType && !t.IsAbstract);
            classTypes = classTypes.Where(i => i.IsAssignableTo(typeof(IValidator)));
            foreach (var item in classTypes)
            {
                var serviceType = item.GetInterfaces().FirstOrDefault(i => i.Name.Equals("IValidator`1"));
                services.AddTransient(serviceType, item);
                //services.AddTransient<>
            }
            //services.AddValidatorsFromAssemblies(libs);

            return services;
        }
    }
}

namespace CommunityToolkit.Mvvm.DependencyInjection
{
    public static class IocDefaultExtensions
    {
        static IocDefaultExtensions()
        {
            ExceptionHandlerBinder.Bind();
        }

        public static Ioc AddCore(this Ioc ioc, Func<IServiceCollection,IServiceCollection>? register = null)
        {
            var services = new ServiceCollection()
                .AddMvvm();
            register?.Invoke(services);
            ioc.ConfigureServices(services.BuildServiceProvider());
            return ioc;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static object GetView(this Ioc ioc, string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException($"“{nameof(name)}”不能为 null 或空。", nameof(name));
            name = name.Replace(".xaml", string.Empty);

            string assemblyName = string.Empty;
            string vTypeName = name;
            if (name.Contains(";"))
            {
                var names = name.Split(";");
                assemblyName = names[0];
                vTypeName = names[1];
            }

            var libs = DependencyContext.Default.RuntimeLibraries.Where(i => i.Type.Equals("project"))
                .Select(i => Assembly.LoadFrom(Path.Combine(AppContext.BaseDirectory, $"{i.Name}.dll")));
            var classTypes = libs.SelectMany(i => i.GetTypes()).Where(t => t.IsClass && !t.IsGenericType && !t.IsAbstract);

            Type viewType = null;
            if (string.IsNullOrEmpty(assemblyName))
            {
                if (classTypes.Any(i => i.Name.Equals(vTypeName)))
                {
                    viewType = classTypes.First(i => i.Name.Equals(vTypeName));
                }
            }
            else
            {
                if (classTypes.Any(i => i.Assembly.FullName.Contains(assemblyName) && i.Name.Equals(vTypeName)))
                {
                    viewType = classTypes.First(i => i.Assembly.FullName.Contains(assemblyName) && i.Name.Equals(vTypeName));
                }
            }
            if (viewType is null)
                return null;

            return Ioc.Default.GetService(viewType);
        }
    }
}
