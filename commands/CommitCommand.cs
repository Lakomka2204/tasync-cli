using CommandLine;
using Tasync.Utils;

namespace Tasync.Commands
{
  [Verb("commit", HelpText = "Commits to the folder or creates new if doesn't exist")]
  public class CommitCommand : ICommand
  {
    [Option('f',"force",HelpText = "Force commit into cloud", Default = false)]
    public bool Force {get; set;} = false;
    public Task Execute(CLIOptions options)
    {
      throw new NotImplementedException();
    }
  }
}
