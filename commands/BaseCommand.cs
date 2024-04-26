using System.Text.Json;
using CommandLine;

namespace Tasync.Commands
{
    public abstract class BaseCommand : ICommand
    {
        [Option('d', "dir", HelpText = "Set working directory", Required = false, Separator = ' ')]
        public string Dir { get; set; } = Directory.GetCurrentDirectory();
        public static readonly JsonSerializerOptions _jsonSerializerOptions = new(JsonSerializerDefaults.Web) { WriteIndented = true };

        public abstract Task Execute();
    }
}
