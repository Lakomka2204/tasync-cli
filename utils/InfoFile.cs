using System.Text;
using System.Text.Json.Serialization;

namespace Tasync.Utils
{
    public class InfoFile
    {
        [JsonIgnore]
        public const string InfoFileName = ".tasync";
        [JsonIgnore]
        private readonly string[] _allFiles = [];
        [JsonPropertyName("remote")]
        public string? Remote { get => Host?.ToString(); }
        [JsonPropertyName("commit")]
        public double CommitTime { get; set; } = Math.Floor(DateTime.Now.Subtract(DateTime.UnixEpoch).TotalSeconds);
        [JsonPropertyName("ignore")]
        public List<string> IgnoredFiles { get; set; } = [];
        [JsonPropertyName("files")]
        public string[] Files
        {
            get => _allFiles.Where(x => !IgnoredFiles.Contains(x)).ToArray();
        }
        [JsonIgnore]
        public string InfoFileLocation
        {
            get => Path.Combine(LocalFolder, InfoFileName);
        }
        [JsonIgnore]
        public string LocalFolder { get; set; }
        [JsonIgnore]
        public Host? Host { get; set; }
        /// <summary>
        /// Creates new infofile in localFolderName
        /// </summary>
        /// <param name="localFolderName">directory where info file will be placed</param>
        /// <param name="host">on get command should not be null, on init - null</param>
        public InfoFile(string localFolderName, Host? host = null)
        {
            LocalFolder = localFolderName;
            _allFiles = Directory.GetFiles(localFolderName).Select(Path.GetFileName)!.ToArray()!;
            Host = host;
            Save();
        }
        /// <summary>
        /// reads info file from localFolderName
        /// </summary>
        /// <param name="localFolderName">folder where program will search for info file</param>
        /// <exception cref="FileNotFoundException">when info file is not found</exception>
        public InfoFile(string localFolderName)
        {
            LocalFolder = localFolderName;
            if (!File.Exists(InfoFileLocation))
                throw new FileNotFoundException("Info file is not found");
            _allFiles = Directory.GetFiles(localFolderName).Select(Path.GetFileName)!.ToArray()!;
            using var sr = new StreamReader(InfoFileLocation);
            var content = sr.ReadToEnd();
            ReadContent(content);
        }
        private void ReadContent(string content)
        {
            string[] lines = content.Split(Environment.NewLine);
            foreach (var line in lines)
            {
                var text = line.Split('\t');
                if (text.Length == 0) continue;
                switch (text[0])
                {
                    case "remote":
                        if (text.Length < 2)
                            throw new ArgumentException("Remote: insufficient args");
                        Host = text[1].ParseHost();
                        break;
                    case "ignore":
                        if (text.Length < 2)
                            break;
                        IgnoredFiles.AddRange(text.Skip(1));
                        break;
                    case "commit":
                        if (text.Length < 2)
                            throw new ArgumentException("Commit: insufficient args");
                        if (!double.TryParse(text[1], out double commitTime))
                            throw new ArgumentException("Commit: time is not int " + text.Nice());
                        CommitTime = commitTime;
                        break;
                }
            }
        }
        public void Save()
        {
            using var sw = new StreamWriter(InfoFileLocation);
            sw.Write(this);
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("commit\t");
            sb.AppendLine(CommitTime.ToString());
            sb.Append("remote\t");
            sb.AppendLine(Remote);
            sb.Append("ignore\t");
            sb.AppendLine(string.Join('\t', IgnoredFiles));
            return sb.ToString().Trim();
        }
    }
}
