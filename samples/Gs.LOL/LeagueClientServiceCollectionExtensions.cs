using Gs.LOL;
using System.Diagnostics;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using static System.Net.WebRequestMethods;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class LeagueClientServiceCollectionExtensions
    {
        public static IServiceCollection AddLeagueClient(this IServiceCollection services)
        {
            services.AddHttpClient<LeagueClient>()
           .ConfigureHttpClient(http =>
           {
               http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
               Connnect();
               void Connnect()
               {
                   var idx = 0;
                   while (true)
                   {
                       try
                       {
                           var authenticate = GetAuthenticationToken();
                           if (!string.IsNullOrEmpty(authenticate) && authenticate.Contains("--remoting-auth-token="))
                           {
                               var tokenResults = authenticate.Split("--remoting-auth-token=");
                               var portResults = authenticate.Split("--app-port=");
                               var PidResults = authenticate.Split("--app-pid=");
                               var installLocations = authenticate.Split("--install-directory=");

                               var token = tokenResults[1].Substring(0, tokenResults[1].IndexOf("\""));
                               var port = int.TryParse(portResults[1].Substring(0, portResults[1].IndexOf("\"")), out var temp) ? temp : 0;
                               var pid = int.TryParse(PidResults[1].Substring(0, PidResults[1].IndexOf("\"")), out var temp1) ? temp1 : 0;
                               if (string.IsNullOrEmpty(token) || port == 0)
                                   throw new InvalidOperationException("invalid data when try to crack.");
                               http.BaseAddress = new Uri($"https://127.0.0.1:{port}");
                               http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"riot:{token}")));
                               break;
                           }
                           else
                           {
                               Trace.WriteLine("解析错误", $"客户端连接失败{++idx}");
                               throw new InvalidOperationException("can't read right token and port");
                           }
                       }
                       catch (Exception ex)
                       {
                           Trace.WriteLine(ex, $"客户端连接失败{++idx}");
                           Thread.Sleep(2000);
                       }
                   }
               }
               string GetAuthenticationToken()
               {
                   string _cmdPath = @"C:\Windows\System32\cmd.exe";
                   string _excuteShell = "wmic PROCESS WHERE name='LeagueClientUx.exe' GET commandline";
                   using (Process p = new Process())
                   {
                       p.StartInfo = new ProcessStartInfo
                       {
                           FileName = _cmdPath,
                           UseShellExecute = false,
                           RedirectStandardError = true,
                           RedirectStandardInput = true,
                           RedirectStandardOutput = true,
                           CreateNoWindow = true
                       };
                       p.Start();
                       p.StandardInput.WriteLine(_excuteShell.TrimEnd('&') + "&exit");
                       p.StandardInput.AutoFlush = true;
                       string output = p.StandardOutput.ReadToEnd();
                       p.WaitForExit();
                       p.Close();
                       return output;
                   }
               }
           })
           .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
           {
               ClientCertificateOptions = ClientCertificateOption.Manual,
               ServerCertificateCustomValidationCallback = (response, cert, chain, errors) => true
           });
            return services;
        }
    }
}
