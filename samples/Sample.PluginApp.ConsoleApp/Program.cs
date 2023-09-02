using CommunityToolkit.Common;
using FluentValidation;
using FluentValidation.Results;
//using Gs.LOL;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using Newtonsoft.Json;
using Sample.PluginApp.ConsoleApp;
using System.Diagnostics;
using System.IO.Pipes;
using System.Reflection;

internal class Program
{
    private static IServiceProvider sp;
    static Program()
    {
        var services = new ServiceCollection();
        services.AddDynamicProxy<SampleService>()
            .AddValidation();
        //services.AddLeagueClient();
        sp = services.BuildServiceProvider();
    }
    private static async Task Main(string[] args)
    {
        // DynamicProxyTest();
        //CommunityToolkitTest();
        //await LeagueClientTestAsync();
        //PipesTest();
        //AmqpTest();

        //QueueAttributeTest();
        //SerializeTest();
        await ValidationTest();
        Console.ReadLine();
    }

    private static async Task ValidationTest()
    {
        var validtor = sp.GetRequiredService<IValidator<UserLoginInput>>();
        var rt = await validtor.ValidateAsync(new UserLoginInput { Uid = "galoSoft", Pwd = "" });
        if (!rt.IsValid)
        {
            var msg = rt.ErrorMessage();
            Trace.WriteLine($"\t{msg}", "“validation”");
        }
    }

    private static void SerializeTest()
    {
        var obj = Enumerable.Range(0, 100000).Select(i => new HiSayInput { Who = $"gs_{i}" });

        //PerformanceCount(() => MemoryPackSerializer.Serialize(obj), title: "MemoryPack");
        //PerformanceCount(() => System.Text.Json.JsonSerializer.Serialize(obj), title: "Microsoft");
        PerformanceCount(() => JsonConvert.SerializeObject(obj), title: "Newtonsoft");
    }

    private static void PerformanceCount(Action action, int times = 10000, string title = "default")
    {
        var sw = Stopwatch.StartNew();
        sw.Start();

        for (int i = 0; i < times; i++)
        {
            action.Invoke();
        }

        sw.Stop();
        var ms = sw.ElapsedMilliseconds;
        Trace.WriteLine($"\t耗时{ms}", $"“{title}”");
    }

    private static void QueueAttributeTest()
    {
        var services = new ServiceCollection()
            .AddTransient<HelloWorldHandler>();
        var sp = services.BuildServiceProvider();
        HandlersRegister(sp);

        var idx = 10;
        while (idx-- >= 0)
        {
            object? obj = null;
            var q = string.Empty;
            var i = idx % 3;
            switch (i)
            {
                case 0:
                    obj = new HiSayInput { Who = $"galoS_{idx}" };
                    q = "csharp.amqp.test";
                    break;
                case 1:
                    obj = idx;
                    q = "csharp.amqp.test.int";
                    break;
                case 2:
                    obj = $"string_{idx}";
                    q = "csharp.amqp.test.string";
                    break;
                default:
                    break;
            }
            if (obj == null || string.IsNullOrEmpty(q))
                continue;

            AmqpHelper.Publish(obj, q);
        }
    }

    private static void HandlersRegister(IServiceProvider sp)
    {
        var libs = DependencyContext.Default.RuntimeLibraries.Where(i => i.Type.Equals("project"))
            .Select(i => Assembly.LoadFrom(Path.Combine(AppContext.BaseDirectory, $"{i.Name}.dll")));
        var classTypes = libs.SelectMany(i => i.GetTypes()).Where(t => t.IsClass && !t.IsGenericType && !t.IsAbstract);

        var mis = classTypes.SelectMany(i => i.GetMethods())
              .Where(m => m.GetCustomAttribute<QueueAttribute>(false) != null);
        if (mis is null || !mis.Any())
            return;

        foreach (var mi in mis)
        {
            var queue = mi.GetCustomAttribute<QueueAttribute>();
            AmqpHelper.Consume(msg =>
            {
                using (var scope = sp.CreateScope())
                {
                    var sp = scope.ServiceProvider;
                    var instance = sp.GetRequiredService(mi.DeclaringType);

                    var parameters = new List<object>();
                    var paras = mi.GetParameters();
                    if (paras.Any())
                    {
                        var first = paras[0];
                        var firstType = first.ParameterType;
                        if (firstType.IsPrimitive || firstType == typeof(string))
                        {
                            parameters.Add(Convert.ChangeType(msg, firstType));
                        }
                        else
                        {
                            var parameter = JsonConvert.DeserializeObject(msg, paras[0].ParameterType);
                            if (parameter is not null)
                            {
                                parameters.Add(parameter);
                            }
                        }
                    }
                    var rt = mi.Invoke(instance, parameters.ToArray());
                    if (mi.ReturnType.IsAssignableTo(typeof(Task)))
                    {
                        var task = rt as Task;
                        task.ConfigureAwait(false);
                        rt = task.GetType().GetProperty("Result").GetValue(rt);
                    }
                    if (rt is Boolean b)
                        return b;

                    return true;
                }
            }, queue.Name);
        }
    }

    private static void AmqpTest()
    {
        AmqpHelper.Consume(msg =>
        {
            Trace.WriteLine($"\t{msg}", "“Consumer”>");
            return true;
        });
        AmqpHelper.Publish("Hello World!!!");
    }

    private static async void PipesTest()
    {
        using (var server = new AnonymousPipeServerStream(PipeDirection.Out, HandleInheritability.Inheritable))
        {
            var pipeName = server.GetClientHandleAsString();

            var startInfo = new ProcessStartInfo(@"E:\code\samples\Sample.PipeClient.ConsoleApp\bin\Debug\net6.0\Sample.PipeClient.ConsoleApp.exe", pipeName);
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            using (var process = Process.Start(startInfo))
            {
                server.DisposeLocalCopyOfClientHandle();
                using (var sw = new StreamWriter(server))
                {
                    var lines = 0;
                    while (lines++ <= 13)
                    {
                        var j = lines;
                        await sw.WriteLineAsync($"{j}.\tprocess starting...");
                        //server.WaitForPipeDrain();
                    }
                }
                process.WaitForExit();
                process.Dispose();
                process.Close();
            }
        }

    }

    //private static async Task LeagueClientTestAsync()
    //{
    //    var league = sp.GetRequiredService<LeagueClient>();

    //    //await league.ConnnectAsync();

    //    var session = await league.LoginSessionGetAsync();
    //    Trace.WriteLine($"{session.accountId}", "Login");

    //    var summoner = await league.SummonerGetAsync();
    //    Trace.WriteLine($"{summoner.accountId},{summoner.displayName},{summoner.puuid}", "Summoner");

    //    await league.UxMinimizeAsync();
    //    await league.UxShowAsync();
    //    //await league.UxLaunchAsync();
    //    //await league.UxKillAsync();

    //    //var perk = await league.CurrentPerkGetAsync();
    //    //Trace.WriteLine($"{perk["id"]}.{perk["name"]}", "Perk");

    //    //var perks = await league.PerkGetAsync();
    //    //Trace.WriteLine($"\r{string.Join("\r", perks.Select(i => $"{i["id"]}.{i["name"]}"))}", "Perk");

    //    //perks = await league.PerkDeletableGetAsync();
    //    //Trace.WriteLine($"\r{string.Join("\r", perks.Select(i => $"{i["id"]}.{i["name"]}"))}", "Perk");

    //    //await league.PerkAddAsync(new PerkAddRequest
    //    //{
    //    //    name = "峡谷1",
    //    //    current = true,

    //    //});
    //    //await league.PerkDeleteAsync(perks.First()["id"]);

    //    //var stats = await league.RankStatsGetAsync();
    //    //stats = await league.RankStatsGetAsync(summoner["puuid"]);

    //    var summonerProps = await league.SummonerQueryAsync("吊锤台湾完成统一");

    //    //var conversations = await league.ChatConversationsGetAsync();
    //    //var names = conversations.Select(i => i["name"]);
    //    //var id = conversations.First(i => i["name"].ToString().Equals("吊锤台湾完成统一"))["id"];

    //    await league.ChatConversationMessageSendAsync(summonerProps["accountId"], "shl si foolish");

    //    //var champSessions = await league.ChampSelectSessionGetAsync();//localPlayerCellId,myTeam,actions["id"]
    //    //league.HeroLockAsync(new HeroLockRequest { ActionId="", ChampionId="",Type="pick" });//TODO type :pick or ban

    //    //var game = await league.GameflowSessionGetAsync();
    //    //perks = league.PerksRecommendedGetAsync();
    //}

    private static void CommunityToolkitTest()
    {
        var name = "gainorloss";
        name.IsEmail();
        var arr = new[] { 1, 2 };
        var strs = arr.ToArrayString();
        foreach (var str in strs)
        {
            Console.WriteLine(str);
        }
        var htmlStr = @"<style></style><html><head>title</head><body>body</body></html>";
        htmlStr = htmlStr.FixHtml();
        Trace.WriteLine(htmlStr, "CommunityToolkit");
        var mobile = "175xxxx1463";
        Trace.WriteLine(mobile.IsPhoneNumber(), "CommunityToolkit");
        mobile = "175 5103 1463";
        Trace.WriteLine(mobile.IsPhoneNumber(), "CommunityToolkit");
        mobile = mobile.Truncate(10);
        Trace.WriteLine(mobile, "CommunityToolkit");
    }

    private static void DynamicProxyTest()
    {
        var proxy = sp.GetRequiredService<ISampleService>();
        proxy.SayHello();
        // await proxy.SayHelloAsync();
    }
}

public class UserLoginInput
{
    public string Uid { get; set; }
    public string Pwd { get; set; }
}

internal class UserLoginInputValidator : AbstractValidator<UserLoginInput>
{
    public UserLoginInputValidator()
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleFor(r => r.Uid).NotNull().NotEmpty()
            //.WithMessage("请输入'用户名'")
            .MinimumLength(6)
            //.WithMessage("'用户名'不得少于6个字符")
            .WithName("用户名");
        RuleFor(r => r.Pwd)
            .NotNull().NotEmpty()
            .MinimumLength(6)
            //.Must(i=>i.Equals("p@ssw0rd"))
            .WithName("密码");
    }
}