using CommandLine;
using Tasync.Utils;

namespace Tasync.Commands
{
    [Verb("logout", HelpText = "Logout from the cloud")]
    public class LogoutCommand : BaseCommand, ICommand
    {
        public async Task Execute()
        {
            Config.UserToken = null;
            await Task.CompletedTask;
        }
    }
}
