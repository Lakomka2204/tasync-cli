string appName = AppDomain.CurrentDomain.FriendlyName;
string help = $@"
usage: {appName} [command]

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

string host = Environment.GetEnvironmentVariable("T_HOST") ?? "192.168.31.203";

if (args.Length == 0)
{
  Console.WriteLine(help);
  return 0;
}

var commandEnum = args.ToList().GetEnumerator() as IEnumerator<string>;
while (commandEnum.MoveNext()) {
  string command = commandEnum.Current;
  switch(command) {
    case "login":
      using (var h = new HttpClient()) {
        
      }
    break;
  }
}
return 0;