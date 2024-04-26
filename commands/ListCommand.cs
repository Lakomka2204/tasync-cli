using System.Net.Http.Json;
using System.Text.Json;
using CommandLine;
using Tasync.Responses;
using Tasync.Utils;

namespace Tasync.Commands
{
    /*
    [Verb("list", default, ["l"], HelpText = "List all folders stored in the cloud")]
    public class ListCommand : BaseCommand
    {
        [Option('j', "json", HelpText = "Format output in json format", Required = false, Default = false)]
        public bool Json { get; set; } = false;
        public override async Task Execute()
        {
            if (Config.UserToken is null)
            {
                Console.WriteLine("You are not logged in");
                Environment.ExitCode = 1;
                return;
            }
            var uri = Request.ComposeUri(, "/folder");
            var res = await Request.Make(HttpMethod.Get, uri, Config.UserToken);
            if (!res.IsSuccessStatusCode)
            {
                var error = await res.Content.ReadFromJsonAsync<ErrorResponse>();
                Environment.ExitCode = Request.PrintHttpErrorAndExit(error);
                return;
            }
            var folders = await res.Content.ReadFromJsonAsync<FolderResponse[]>();
            if (Json)
            {
                var outputStream = Console.OpenStandardOutput();
                await JsonSerializer.SerializeAsync(outputStream, folders, _jsonSerializerOptions);
            }
            else
            Console.WriteLine(string.Join(Environment.NewLine, folders?.Select(x => x.ToString()) ?? ["None"]));
        }
    }
    */
}
