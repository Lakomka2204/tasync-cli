using System.Collections;
using tasync;
string appName = AppDomain.CurrentDomain.FriendlyName;
string help = $@"
usage: {appName} [parameters] [command]

parameters:

-dir [directory] - changes working directory to specified

commands:
help - show help
init - initialize tracking
status - check status
commit - commit changes
upload - upload files to cloud (not yet)
sync - sync files (not yet)
ignore add|remove ...files - adds or removes files from ignore list
";
if (args.Length == 0)
{
  Console.WriteLine(help);
  return 0;
}
string dir = Directory.GetCurrentDirectory();

var commandEnum = args.ToList().GetEnumerator() as IEnumerator<string>;
while (commandEnum.MoveNext())
{
  var current = commandEnum.Current;
  switch (current)
  {
    case "-dir":
      if (!commandEnum.MoveNext())
      {
        Console.WriteLine(help);
        return 1;
      }
      var newDir = commandEnum.Current;
      if (!Directory.Exists(Path.Combine(dir, newDir)))
      {
        Console.WriteLine("dir {0} does not exists", newDir);
        return 1;
      }
      dir = newDir;
      break;
  }
}
commandEnum.Reset();

while (commandEnum.MoveNext())
{
  if (commandEnum.Current is null)
    continue;
  var current = commandEnum.Current;
  try
  {
    switch (current)
    {
      case "help":
        Console.WriteLine(help);
        return 0;
      case "status":
        using (var w = new Watcher(dir))
        {
          Console.WriteLine("Current\t{0}", (from f in w.TrackingFiles select f.File).Nice());
          Console.WriteLine("Added\t{0}", w.AddedFiles.Nice());
          Console.WriteLine("Updated\t{0}", (from f in w.UpdatedFiles select f.File).Nice());
          Console.WriteLine("Deleted\t{0}", (from f in w.DeletedFiles select f.File).Nice());
        }
        return 0;
      case "commit":
        using (var w = new Watcher(dir))
        {
          if (!w.HasUncommittedChanges)
          {
            Console.WriteLine("Nothing to commit.");
            return 0;
          }
          Console.WriteLine("Commit A:{0} U:{1} D:{2}", w.AddedFiles.Count(), w.UpdatedFiles.Count(), w.DeletedFiles.Count());
          w.Commit();
          w.Save();
          return 0;
        }
      case "init":
        using (var w = new Watcher(dir, true))
        {
          w.Commit();
          w.Save();
        }
        return 0;
      case "ignore":
        if (!commandEnum.MoveNext())
        {
          Console.WriteLine(help);
          return 1;
        }
        var command = commandEnum.Current;
        switch (command)
        {
          case "add":
            using (var w = new Watcher(dir))
            {
              List<string> files = [];
              while (commandEnum.MoveNext())
                files.Add(commandEnum.Current);
              w.IgnoreFiles = [.. w.IgnoreFiles, .. files];
              w.IgnoreFiles = w.IgnoreFiles.Distinct().ToArray();
              w.Save();
            }
            return 0;
          case "remove":
            using (var w = new Watcher(dir))
            {
              List<string> files = [];
              while (commandEnum.MoveNext())
                files.Add(commandEnum.Current);
              w.IgnoreFiles = w.IgnoreFiles.Where(x => !files.Contains(x)).ToArray();
              w.Save();
            }
            return 0;
          default:
            Console.WriteLine(help);
            return 1;
        }
    }
  }
  catch (ArgumentException ae)
  {
    Console.WriteLine("error {0}", ae.Message);
    return ae.HResult;
  }
}
return 0;