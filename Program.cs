namespace tasync;
string appName = AppDomain.CurrentDomain.FriendlyName;
string host = Environment.GetEnvironmentVariable("T_HOST") ?? "192.168.31.203";
string dir = Directory.GetCurrentDirectory();
string help = $@"
usage: {appName} [parameters] [command]

parameters:
-host [host] - change default host url which is: {host} to specified
-dir [dir] - change working directory to specified

commands:
login [username] [password] - login to the cloud
logout - logs off the account from cloud
list - list all folders stored in the cloud
get [folder name] - retrieves last commit from cloud folder
get [folder name] [commit] - retrieves specified commit from cloud folder
commit - commits to the folder or if doesn't exists in cloud creates new
commits - get list of all commits of current folder
commits [folder name] - get list of all commits of specified cloud folder
";


if (args.Length == 0)
{
  Console.WriteLine(help);
  return 0;
}

#region Params secion
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
      case "-url":
      if (!commandEnum.MoveNext())
      {
        Console.WriteLine(help);
        return 1;
      }
      host = commandEnum.Current;
  }
}
commandEnum.Reset();
#endregion

var commandEnum = args.ToList().GetEnumerator() as IEnumerator<string>;
while (commandEnum.MoveNext()) {
  string command = commandEnum.Current;
  switch(command) {
    case "login":
      
    break;
  }
}
return 0;