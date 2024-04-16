namespace tasync
{
  public static class Config
  {
    public static string? UserToken { get {
      using var sr = new StreamReader(Path.Combine(UserDir,UserConfigFile));
      return sr.ReadLine()?.Trim();
    } set {
      using var sw = new StreamWriter(Path.Combine(UserDir,UserConfigFile));
      sw.Write(value);
    } }
    public const string UserConfigFile = ".gtasync";
    public static string UserDir {
      get {
        return Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
      }
    }
  }
}