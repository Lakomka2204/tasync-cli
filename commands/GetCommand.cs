using System.Net.Http.Json;
using CommandLine;
using Tasync.Responses;
using Tasync.Utils;

namespace Tasync.Commands
{
    [Verb("get",false,["g"], HelpText = "Retrieves commits from cloud folder")]
    public class GetCommand : BaseCommand
    {
        [Value(0, MetaName = "folder", Required = true, HelpText = "Folder name")]
        public string Folder { get; set; } = string.Empty;

        [Value(1, MetaName = "commit", HelpText = "Commit (optional)")]
        public string? Commit { get; set; }

        public override async Task Execute()
        {
            if (Config.UserToken is null)
            {
                Console.WriteLine("You are not logged in");
                Environment.ExitCode = 1;
                return;
            }
            var uri = Request.ComposeUri(Host,$"/folder/{Folder}/{Commit ?? "last"}");
            var res = await Request.Make(HttpMethod.Get,uri,Config.UserToken);
            if (!res.IsSuccessStatusCode)
            {
                var error = await res.Content.ReadFromJsonAsync<ErrorResponse>();
                Environment.ExitCode = Request.PrintHttpErrorAndExit(error);
                return;
            }
            var commitHeader = res.Headers.GetValues("Commit").ElementAt(0);
            if (!double.TryParse(commitHeader, out var commitTime))
            {
                Console.Error.WriteLine("Received non-int header");
                Environment.ExitCode = 1;
                return;
            }
            var archiveStream = await res.Content.ReadAsStreamAsync();
            Archive.ExtractTo(archiveStream,Dir);
            var info = new InfoFile(Dir,Folder,true) {CommitTime = commitTime};
            info.Save();
        }
    }
}
