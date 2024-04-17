using Tasync.Utils;

namespace Tasync.Commands
{
  public interface ICommand
  {
    Task Execute(CLIOptions options);
  }
}
