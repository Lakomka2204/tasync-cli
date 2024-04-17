using CommandLine;
using Tasync.Commands;
using Tasync.Utils;

var config = Parser.Default.ParseArguments<CLIOptions>(args).Value;
var commandType = typeof(ICommand);
var commandTypes = AppDomain.CurrentDomain.GetAssemblies()
    .SelectMany(x => x.GetTypes())
    .Where(x => commandType.IsAssignableFrom(x) && x != commandType).ToArray();
var parser = await Parser.Default
.ParseArguments(args,commandTypes)
.WithParsedAsync<ICommand>(async (command) => await command.Execute(config));
await parser.WithNotParsedAsync(errors => {
    if (errors.Select(x => x.StopsProcessing).Any())
    Environment.ExitCode = 1;
    return Task.CompletedTask;
});
