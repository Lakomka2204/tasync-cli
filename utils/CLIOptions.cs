using CommandLine;
namespace Tasync.Utils
{
  public class CLIOptions
  {
    [Option('d',"dir", HelpText = "Set working directory", Required = false, Separator = ' ')]
    public string Dir { get; set;} = Directory.GetCurrentDirectory();
    [Option('h',"host", HelpText ="Set current host", Required = false, Separator = ' ')]
    public string Host { get; set; } = "http://192.168.31.202"; // lol
  }
}
