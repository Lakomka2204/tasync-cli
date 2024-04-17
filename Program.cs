using CommandLine;
using Tasync.Commands;

var commandType = typeof(ICommand);
var commandTypes = AppDomain.CurrentDomain.GetAssemblies()
    .SelectMany(x => x.GetTypes())
    .Where(x => commandType.IsAssignableFrom(x) && x != commandType).ToArray();
await Parser.Default.ParseArguments(args,commandTypes).WithParsedAsync<ICommand>(async (command) => await command.Execute());
