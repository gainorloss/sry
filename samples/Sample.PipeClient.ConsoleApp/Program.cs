// See https://aka.ms/new-console-template for more information

using System.IO.Pipes;
using System.Text;

using (var client = new AnonymousPipeClientStream(PipeDirection.In, args[0]))
{
    using (var sw = new StreamWriter("c://pipe.txt"))
    {
        using (var sr = new StreamReader(client))
        {
            var line = await sr.ReadLineAsync();
            if (!string.IsNullOrEmpty(line))
            {
                sw.WriteLine(await sr.ReadLineAsync());
            }
        }
    }
}

Console.Read();