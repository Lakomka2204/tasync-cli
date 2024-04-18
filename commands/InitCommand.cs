
using CommandLine;
using Tasync.Utils;

namespace Tasync.Commands
{
    [Verb("init",default,["i"],HelpText = "Initialize new tracking folder")]
    public class InitCommand : BaseCommand
    {
        public override Task Execute()
        {
            _ = new InfoFile(Dir, Path.GetFileName(Dir)!, true);
            return Task.CompletedTask;
        }
    }
}
