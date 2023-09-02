using System.Diagnostics;
using Castle.DynamicProxy;

namespace Sample.PluginApp.ConsoleApp;

/// <summary>
/// 
/// </summary>
[AttributeUsage( AttributeTargets.Class|AttributeTargets.Interface,AllowMultiple =true)]
public class LoggerInterceptorAttribute :Attribute, IInterceptor
{
    public void Intercept(IInvocation invocation)
    {
        // if (!invocation.ReturnValue.GetType().IsAssignableTo(typeof(Task)))
        // {
        //     Trace.WriteLine("同步", "castle>\t");
        //     invocation.Proceed();
        //     return;
        // }
        invocation.ReturnValue = InterceptAsync(invocation);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="invocation"></param>
    /// <returns></returns>
    public async Task InterceptAsync(IInvocation invocation)
    {
        var proceedInfo = invocation.CaptureProceedInfo();

        await Task.Run(() =>
        {
            Trace.WriteLine("执行开始", "castle>\t");
        });
        proceedInfo.Invoke();

        await Task.Run(() =>
        {
            Trace.WriteLine("执行结束", "castle>\t");
        });
    }
}
