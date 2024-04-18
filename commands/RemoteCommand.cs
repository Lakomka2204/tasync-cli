
using CommandLine;
using Tasync.Utils;

namespace Tasync.Commands
{
    [Verb("remote",default,["r"],HelpText = "Get or change cloud name")]
    public class RemoteCommand : BaseCommand
    {
        [Value(0,MetaName = "new remote",HelpText = "Sets new cloud folder name")]
        public string? NewRemote { get; set; }
        public override async Task Execute()
        {
            try{
                var info = new InfoFile(Dir);
                if (NewRemote is not null)
                {
                    info.RemoteFolderName = NewRemote;
                    info.Save();
                }
                else
                    Console.WriteLine(info.RemoteFolderName);
            }
            catch(Exception ex)
            {
                Console.Error.WriteLine("Error: {0}",ex.Message);
                Environment.ExitCode = ex.HResult;
            }
            await Task.CompletedTask;
        }
    }
}
