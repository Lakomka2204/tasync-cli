using CommandLine;
using Tasync.Commands;

var commandType = typeof(BaseCommand);
var commandTypes = AppDomain.CurrentDomain.GetAssemblies()
    .SelectMany(x => x.GetTypes())
    .Where(x => commandType.IsAssignableFrom(x) && x != commandType).ToArray();
    
var parser = await Parser.Default
.ParseArguments(args,commandTypes)
.WithParsedAsync<ICommand>(async (command) => await command.Execute());
await parser.WithNotParsedAsync(errors => {
    if (errors.Select(x => x.StopsProcessing).Any())
    Environment.ExitCode = 1;
    return Task.CompletedTask;
});
