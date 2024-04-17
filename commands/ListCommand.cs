
using CommandLine;

namespace Tasync.Commands
{
    [Verb("list", HelpText = "List all folders stored in the cloud")]
    public class ListCommand : ICommand
    {
        public async Task Execute()
        {
            throw new NotImplementedException();
        }
    }
}
