using CommandLine;

namespace Tasync.Commands
{
  [Verb("commit", HelpText = "Commits to the folder or creates new if doesn't exist")]
  public class CommitCommand : BaseCommand, ICommand
  {
    [Option('f',"force",HelpText = "Force commit into cloud", Default = false)]
    public bool Force {get; set;} = false;
    public Task Execute()
    {
      throw new NotImplementedException();
    }
  }
}
