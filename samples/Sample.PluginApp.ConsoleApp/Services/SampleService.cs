using System.Diagnostics;

namespace Sample.PluginApp.ConsoleApp;

public class SampleService : ISampleService
{
    /// <summary>
    /// 
    /// </summary>
    public void SayHello()
    {
        Trace.WriteLine("Hello");
    }

    /// <summary>
    /// 
    /// </summary>
    public async Task SayHelloAsync()
    {
        Trace.WriteLine("Hello");
    }
}

// [ExceptionInterceptor]
// [LoggerInterceptor]
public interface ISampleService
{
    void SayHello(); 
    Task SayHelloAsync();
}
