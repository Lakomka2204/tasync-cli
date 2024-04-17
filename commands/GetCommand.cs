using CommandLine;

namespace Tasync.Commands
{
    [Verb("get", HelpText = "Retrieves commits from cloud folder")]
    public class GetCommand : BaseCommand, ICommand
    {
        [Value(0, MetaName = "folder", Required = true, HelpText = "Folder name")]
        public string Folder { get; set; } = string.Empty;

        [Value(1, MetaName = "commit", HelpText = "Commit (optional)")]
        public string Commit { get; set; } = string.Empty;

        public Task Execute()
        {
            throw new NotImplementedException();
        }
    }
}
