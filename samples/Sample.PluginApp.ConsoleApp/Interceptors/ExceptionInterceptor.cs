namespace Sample.PluginApp.ConsoleApp;

using System.Diagnostics;
using Castle.DynamicProxy;

/// <summary>
/// 
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true)]
public class ExceptionInterceptor : Attribute, IInterceptor
{
    public void Intercept(IInvocation invocation)
    {
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

        try
        {
            proceedInfo.Invoke();
        }
        catch (System.Exception e)
        {

        }
        finally
        {
            Trace.WriteLine("异常处理", "castle>\t");
        }
    }
}

