using System.Net.Http.Json;
using CommandLine;
using Tasync.Responses;
using Tasync.Utils;

namespace Tasync.Commands
{
    [Verb("commit", default, ["c"], HelpText = "Commits to the folder or creates new if doesn't exist")]
    public class CommitCommand : BaseCommand
    {
        [Option('f', "force", HelpText = "Force commit into cloud", Default = false)]
        public bool Force { get; set; } = false;
        public override async Task Execute()
        {
            if (Config.UserToken is null)
            {
                Console.WriteLine("You are not logged in");
                Environment.ExitCode = 1;
                return;
            }
            
            var uri = Request.ComposeUri(Host,$"/folder/"); //todo get folder from info file
            var res = await Request.Make(HttpMethod.Put,uri); // todo get files from info file
            if (!res.IsSuccessStatusCode)
            {
                var error = await res.Content.ReadFromJsonAsync<ErrorResponse>();
                Environment.ExitCode = Request.PrintHttpErrorAndExit(error);
                return;
            }
        }
    }
}
