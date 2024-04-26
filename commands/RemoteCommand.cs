
using CommandLine;
using Tasync.Utils;

namespace Tasync.Commands
{
    [Verb("remote", default, ["r"], HelpText = "Get or change cloud name")]
    public class RemoteCommand : BaseCommand
    {
        [Value(0, MetaName = "new remote", HelpText = "Sets new cloud folder name")]
        public string? NewRemote { get; set; }
        [Option('h', "host", Required = false, HelpText = "Sets new host for remote")]
        public string? NewHostName { get; set; }
        [Option('f', "folder", Required = false, HelpText = "Sets new cloud folder name")]
        public string? NewFolderName { get; set; }
        public override async Task Execute()
        {
            try
            {
                var info = new InfoFile(Dir);
                var newHost = new Host(
                    NewHostName ?? info.Host?.host,
                    NewFolderName ?? info.Host?.folderName
                    );
                if (info.Host != newHost)
                {
                    info.Host = newHost;
                    info.Save();
                }
                else
                    Console.WriteLine(info.Remote);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: {0}", ex);
                Environment.ExitCode = ex.HResult;
            }
            await Task.CompletedTask;
        }
    }
}
