using CommandLine;
using Tasync.Utils;

namespace Tasync.Commands
{
    [Verb("ignore", default, ["ig"], HelpText = "Adds/removes files from tracking")]
    public class IgnoreCommand : BaseCommand
    {
        public enum SubCommand { none, add, remove }
        [Value(0, MetaName = "action", HelpText = "add | remove", Required = true)]
        public SubCommand Command { get; set; }
        [Value(1, MetaName = "files", HelpText = "Files to be added/removed", Min = 1, Required = true)]
        public IEnumerable<string> Files { get; set; } = [];
        public override Task Execute()
        {
            try
            {
                var info = new InfoFile(Dir);
                switch (Command)
                {
                    case SubCommand.add:
                        info.IgnoredFiles.AddRange(Files);
                        info.IgnoredFiles = info.IgnoredFiles.Distinct().ToList();
                    break;
                    case SubCommand.remove:
                        info.IgnoredFiles.RemoveAll(x => Files.Contains(x));
                    break;
                }
                if (Command != SubCommand.none)
                    info.Save();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: {0}", ex.Message);
                Environment.ExitCode = ex.HResult;
            }
            return Task.CompletedTask;
        }
        public class AddOption
        {

        }
    }
}
