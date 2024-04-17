
using System.Net.Http.Json;
using CommandLine;
using Tasync.Responses;
using Tasync.Utils;

namespace Tasync.Commands
{
    [Verb("list", HelpText = "List all folders stored in the cloud")]
    public class ListCommand : ICommand
    {
        public async Task Execute(CLIOptions options)
        {
            if (Config.UserToken is null)
            {
                Console.WriteLine("You are not logged in");
                Environment.ExitCode = 1;
                return;
            }
            var uri = Request.ComposeUri(options.Host,"/folder");
            var res = await Request.Make(HttpMethod.Get,uri);
            if (!res.IsSuccessStatusCode)
            {
                var error = await res.Content.ReadFromJsonAsync<ErrorResponse>();
                Environment.ExitCode = Request.PrintHttpErrorAndExit(error);
                return;
            }
            var folders = await res.Content.ReadFromJsonAsync<FolderResponse[]>();
            Console.WriteLine(string.Join(Environment.NewLine,folders?.Select(x => x.ToString()) ?? ["None"]));
        }
    }
}
