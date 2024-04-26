using System.Buffers.Text;
using System.Text;

namespace Tasync.Utils
{
    public static class Config
    {
        private static Dictionary<string, string>? tokens = null;
        public static void Set(string host, string token)
        {
            tokens ??= [];
            tokens[host] = token;
            Save();
        }
        public static string Get(string host)
        {
            if (tokens is null) Read();
            return tokens![host];
        }
        public static void Clear()
        {
            File.Delete(Path);
            tokens = [];
        }
        public static void Read()
        {
            tokens = [];
            using var sr = new StreamReader(Path);
            while (!sr.EndOfStream)
            {
                var line = sr.ReadLine();
                if (line is null) continue;
                var kvp = line.Split('\t');
                if (kvp.Length < 2) continue;
                tokens.Add(kvp[0], Encoding.UTF8.GetString(Convert.FromBase64String(kvp[1])));
            }
        }
        public static void Save()
        {
            using var sw = new StreamWriter(Path);
            foreach (var kvp in tokens ?? [])
                sw.WriteLine(kvp.Key + '\t' + Convert.ToBase64String(Encoding.UTF8.GetBytes(kvp.Value)));

        }
        public static string Path
        {
            get
            {
                return System.IO.Path.Combine(UserDir, UserConfigFile);
            }
        }
        public static string UserDir
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            }
        }
        public static int? Count { get => tokens?.Count; }
        public static string[]? Keys {get => [.. tokens?.Keys];}
        public static string[]? Values {get => [.. tokens?.Values];}
        public const string UserConfigFile = ".gtasync";
        
    }
}
