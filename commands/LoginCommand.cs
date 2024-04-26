using System.Diagnostics;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Web;
using CommandLine;
using Tasync.Responses;
using Tasync.Utils;

namespace Tasync.Commands
{
    [Verb("login", HelpText = "Login to the cloud")]
    public class LoginCommand : BaseCommand
    {
        public const int ServerPort = 10000;
        [Value(0, MetaName = "url", Required = true, HelpText = "Url of the website you will be redirected to")]
        public string Url { get; set; } = string.Empty;

        public override async Task Execute()
        {
            if (Config.Count is not null)
            {
                Console.WriteLine("You are already logged in");
                Environment.ExitCode = 1;
                return;
            }
            string callbackUrl = $"http://127.0.0.1:{ServerPort}/";
            var url = new Uri(Url).GetLeftPart(UriPartial.Path)+"?callbackUrl="+callbackUrl;
            Console.WriteLine("Open this url in web browser: {0}",url);
            using HttpListener listener = new();
            listener.Prefixes.Add(callbackUrl);
            listener.Start();
            string? token = null;
            do {
                var ctx = await listener.GetContextAsync();
                if (ctx.Request.Url?.Query is null) continue;
                var qs = HttpUtility.ParseQueryString(ctx.Request.Url.Query);
                token = qs.Get("token");
                await ctx.Response.OutputStream.WriteAsync(Encoding.UTF8.GetBytes(token is not null ? "Logged in!": "No token"));
                ctx.Response.OutputStream.Close();
            } while(token is null);
            listener.Stop();
            Config.Set(Url,token);
            Console.WriteLine("Got token: {0}",token);
        }
    }
}
