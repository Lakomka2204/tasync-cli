using CommandLine;

namespace Tasync.Commands
{
  [Verb("commit", HelpText = "Commits to the folder or creates new if doesn't exist")]
  public class CommitCommand : ICommand
  {
    public async Task Execute()
    {
      throw new NotImplementedException();
    }
  }
}
