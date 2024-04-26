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
            if (Config.Count is null)
            {
                Console.WriteLine("You are not logged in");
                Environment.ExitCode = 1;
                return;
            }
            try
            {
                var info = new InfoFile(Dir);
                if (info.Host is null)
                    throw new ArgumentException("Could not find remote url inside of info file");
                var uri = Request.ComposeUri(info.Host.host!,
                    $"/folder/{info.Host.folderName}/{info.CommitTime}",
                    $"force={Force.ToString().ToLower()}");
                if (Force)
                    Console.WriteLine("Force commit");
                var resolvedFiles = info.Files
                .Where(f => !f.Equals(InfoFile.InfoFileName))
                .Select(f => Path.GetFullPath(Path.Combine(Dir, f)))
                .ToArray();
                var res = await Request.Make(HttpMethod.Put, uri, Config.Get(info.Host.host!), resolvedFiles, [.. info.IgnoredFiles]);
                if (!res!.IsSuccessStatusCode)
                {
                    var error = await res.Content.ReadFromJsonAsync<ErrorResponse>();
                    Environment.ExitCode = Request.PrintHttpErrorAndExit(error);
                    return;
                }
                var newCommit = await res.Content.ReadAsStringAsync();
                if (!double.TryParse(newCommit, out var newCommitTime))
                {
                    Console.Error.WriteLine("Received response is not int!");
                    Environment.ExitCode = 1;
                    return;
                }
                info.CommitTime = newCommitTime;
                info.Save();
                Console.WriteLine(newCommitTime);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: {0}", ex.Message);
                Environment.ExitCode = ex.HResult;
            }
        }
    }
}
