using System.Net.Http.Json;
using CommandLine;
using Tasync.Responses;
using Tasync.Utils;

namespace Tasync.Commands
{
    [Verb("login", HelpText = "Login to the cloud")]
    public class LoginCommand : BaseCommand, ICommand
    {
        [Value(0, MetaName = "email", Required = true, HelpText = "Email address")]
        public string Email { get; set; } = string.Empty;

        [Value(1, MetaName = "password", Required = true, HelpText = "Password")]
        public string Password { get; set; } = string.Empty;

        public async Task Execute()
        {
            if (Config.UserToken is not null)
            {
                Console.WriteLine("You are already logged in");
                Environment.ExitCode = 1;
                return;
            }
            var uri = Request.ComposeUri(Host,"/account");
            var res = await Request.Make(HttpMethod.Post,uri,
            new {
                email = Email,
                password = Password
            });
            if (!res.IsSuccessStatusCode)
            {
                var error = await res.Content.ReadFromJsonAsync<ErrorResponse>();
                Environment.ExitCode = Request.PrintHttpErrorAndExit(error);
                return;
            }
            var authResult = await res.Content.ReadFromJsonAsync<AuthResponse>();
            Config.UserToken = authResult?.AccessToken;
        }
    }
}
