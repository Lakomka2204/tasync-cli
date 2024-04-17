namespace Tasync.Utils
{
    public static class Config
    {
        public static string Path {
            get {
                return System.IO.Path.Combine(UserDir, UserConfigFile);
            }
        }
        public static string? UserToken
        {
            get
            {
                if (!File.Exists(Path))
                    return null;
                using var sr = new StreamReader(Path);
                return sr.ReadLine()?.Trim();
            }
            set
            {
                if (value is not null)
                {
                    using var sw = new StreamWriter(Path);
                    sw.Write(value);
                }
                else File.Delete(Path);
            }
        }
        public const string UserConfigFile = ".gtasync";
        public static string UserDir
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            }
        }
    }
}
