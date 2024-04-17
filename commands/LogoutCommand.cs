using CommandLine;
using Tasync.Utils;

namespace Tasync.Commands
{
    [Verb("logout", HelpText = "Logout from the cloud")]
    public class LogoutCommand : ICommand
    {
        public async Task Execute(CLIOptions options)
        {
            Config.UserToken = null;
            await Task.CompletedTask;
        }
    }
}
