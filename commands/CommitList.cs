using CommandLine;

namespace Tasync.Commands
{
    [Verb("commitlist", HelpText = "Get list of commits of current or specified cloud folder")]
public class CommitListCommand : ICommand
{
    [Value(0, MetaName = "folder", HelpText = "Folder name (optional)")]
    public string Folder { get; set; } = Directory.GetCurrentDirectory();

    public async Task Execute()
    {
      throw new NotImplementedException();
    }
  }
}
