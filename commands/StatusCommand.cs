using System.Text.Json;
using CommandLine;
using Tasync.Utils;

namespace Tasync.Commands
{
    [Verb("status", default, ["s"], HelpText = "Check the status of folder")]
    public class StatusCommand : BaseCommand
    {
        [Option('j', "json", HelpText = "Format output in json format", Required = false, Default = false)]
        public bool Json { get; set; } = false;
        private static JsonSerializerOptions _jsonSerializerOptions = new(JsonSerializerDefaults.Web) { WriteIndented = true };
        public override async Task Execute()
        {
            try
            {
                var info = new InfoFile(Dir);
                if (Json)
                {
                    var outputStream = Console.OpenStandardOutput();
                    await JsonSerializer.SerializeAsync(outputStream, info, _jsonSerializerOptions);
                }
                else
                {
                    Console.WriteLine("Tracking files:\t\t{0}", info.Files.Nice());
                    Console.WriteLine("Ignored files:\t\t{0}", info.IgnoredFiles.Nice());
                    Console.WriteLine("Last commit:\t\t{0}", DateTime.UnixEpoch.AddSeconds(info.CommitTime).ToString("U"));
                    Console.WriteLine("Remote folder name:\t{0}", info.RemoteFolderName);
                }
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: {0}", ex.Message);
                Environment.ExitCode = ex.HResult;
            }
        }
    }
}
