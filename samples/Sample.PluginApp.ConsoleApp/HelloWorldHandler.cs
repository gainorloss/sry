using MemoryPack;
using System.Diagnostics;

namespace Sample.PluginApp.ConsoleApp
{
    internal class HelloWorldHandler
    {
        [Queue("csharp.amqp.test.string")]
        public bool SayHi(string msg)
        { 
            Trace.WriteLine($"\t{msg}", "“QueueAttribute”>");
            return true;
        }

        [Queue("csharp.amqp.test.int")]
        public bool SayHi(int msg)
        {
            Trace.WriteLine($"\t{msg}", "“QueueAttribute”>");
            return true;
        }

        [Queue("csharp.amqp.test")]
        public async Task<bool> SayHiAsync(HiSayInput msg)
        {
            Trace.WriteLine($"\t{msg}", "“QueueAttribute”>");
            await Task.Delay(10);
            return true;
        }
    }

    [MemoryPackable]
    public partial class HiSayInput
    {
        public string? Who { get; set; }

        public override string ToString()
        {
            return $"Hi,{Who}.";
        }
    }
}
