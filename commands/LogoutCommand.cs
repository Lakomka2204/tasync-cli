using CommandLine;

namespace Tasync.Commands
{
    [Verb("logout", HelpText = "Logout from the cloud")]
    public class LogoutCommand : ICommand
    {
        public async Task Execute()
        {
            Config.UserToken = null;
            await Task.CompletedTask;
        }
    }
}
