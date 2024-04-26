
using CommandLine;
using Tasync.Utils;

namespace Tasync.Commands
{
    [Verb("init", default, ["i"], HelpText = "Initialize new tracking folder")]
    public class InitCommand : BaseCommand
    {
        public override Task Execute()
        {
            // get current name of the folder Path.GetFileName(Path.GetFullPath(Dir))!
            _ = new InfoFile(Dir,null);
            return Task.CompletedTask;
        }
    }
}
