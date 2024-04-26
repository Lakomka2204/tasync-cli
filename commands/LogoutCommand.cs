using CommandLine;
using Tasync.Utils;

namespace Tasync.Commands
{
    [Verb("logout", HelpText = "Logout from the cloud")]
    public class LogoutCommand : BaseCommand
    {
        public override async Task Execute()
        {
            Config.Clear();
            await Task.CompletedTask;
        }
    }
}
