using System.Net.Http.Json;
using CommandLine;
using Tasync.Responses;
using Tasync.Utils;

namespace Tasync.Commands
{
    [Verb("register", default, ["reg"], HelpText = "Creates account in the cloud")]
    public class RegisterCommand : LoginCommand
    {
        public override async Task Execute()
        {
            if (Config.UserToken is not null)
            {
                Console.WriteLine("You are already logged in");
                Environment.ExitCode = 1;
                return;
            }
            var uri = Request.ComposeUri(Host,"/account");
            var res = await Request.Make(HttpMethod.Put,uri,null,
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
