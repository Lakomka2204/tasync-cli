using CommandLine;
namespace Tasync.Commands
{
    [Verb("login", HelpText = "Login to the cloud")]
    public class LoginCommand : ICommand
    {
        [Value(0, MetaName = "email", Required = true, HelpText = "Email address")]
        public string Email { get; set; } = string.Empty;

        [Value(1, MetaName = "password", Required = true, HelpText = "Password")]
        public string Password { get; set; } = string.Empty;

        public async Task Execute()
        {
            throw new NotImplementedException();
        }
    }
}
