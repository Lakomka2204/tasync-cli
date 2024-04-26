using System.Net.Http.Json;
using CommandLine;
using Tasync.Responses;
using Tasync.Utils;

namespace Tasync.Commands
{
    [Verb("get", false, ["g"], HelpText = "Retrieves commits from cloud folder")]
    public class GetCommand : BaseCommand
    {
        [Value(0, MetaName = "location", Required = true, HelpText = "Location url", MetaValue = "http://127.0.0.1#FOLDER")]
        public string Location { get; set; } = string.Empty;

        [Value(1, MetaName = "commit", HelpText = "Commit (optional)")]
        public string? Commit { get; set; }
        public override async Task Execute()
        {
            try
            {
                var host = Location.ParseHost() ?? throw new ArgumentException("Unable to parse host");
                if (Config.Count is null)
                {
                    Console.WriteLine("You are not logged in");
                    Environment.ExitCode = 1;
                    return;
                }
                var uri = Request.ComposeUri(host.host!, $"/folder/{host.folderName}/{Commit ?? "last"}");
                var res = await Request.Make(HttpMethod.Get, uri, Config.Get(host.host!));
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
                var ignoreHeaders = res.Headers.GetValues("Ignore");
                var archiveStream = await res.Content.ReadAsStreamAsync();
                Archive.ExtractTo(archiveStream, Dir);
                var info = new InfoFile(Dir, host)
                {
                    CommitTime = commitTime,
                    IgnoredFiles = ignoreHeaders.ToList()
                };
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: {0}\n{1}", ex.Message,ex.StackTrace);
                Environment.ExitCode = 1;
            }
        }
    }
}
