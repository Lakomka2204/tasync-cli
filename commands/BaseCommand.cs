using CommandLine;

namespace Tasync.Commands
{
    public abstract class BaseCommand : ICommand
    {
        [Option('d', "dir", HelpText = "Set working directory", Required = false, Separator = ' ')]
        public string Dir { get; set; } = Directory.GetCurrentDirectory();
        [Option('h', "host", HelpText = "Set current host", Required = false, Separator = ' ')]
        public string Host { get; set; } = "http://192.168.31.202"; // lol

        public abstract Task Execute();
    }
}
