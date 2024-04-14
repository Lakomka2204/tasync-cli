using System.Collections;
using System.Text;
using System.Text.RegularExpressions;

namespace tasync
{
  public partial record Watcher : IDisposable
  {
    public record FileState(string File)
    {
      public DateTime LastWrite { get; set; }
      public override string ToString()
      {
        StringBuilder s = new();
        s.Append(Math.Floor(LastWrite.Subtract(DateTime.UnixEpoch).TotalSeconds));
        s.Append('\t');
        s.Append(File);
        return s.ToString();
      }
    }
    public const string INFOFILE = ".tasync";
    public string Remote { get; set; } = string.Empty;
    public string[] IgnoreFiles { get; set; } = [];
    public FileState[] FileStates { get; private set; } = [];
    public string InfoFile { get => CombinePath(INFOFILE); }
    public string Folder { get; private set; } = string.Empty;
    public DateTime CommitTime {get; set; }
    public string TrackingDir
    {
      get
      {
        return Directory.GetParent(InfoFile)?.FullName ?? throw new ArgumentException("dir is null");
      }
    }
    public IEnumerable<string?> Files
    {
      get
      {
        return Directory.GetFiles(TrackingDir).Select(Path.GetFileName).Where(x => x is not null && !(IgnoreFiles.Contains(x) || x.EndsWith(INFOFILE)));
      }
    }
    public IEnumerable<string?> AddedFiles
    {
      get
      {
        return Files.Where(x => !FileStates.Any(s => s.File == x));
      }
    }
    public IEnumerable<FileState> DeletedFiles
    {
      get
      {
        return FileStates.Where(x => !Files.Contains(x.File));
      }
    }
    public IEnumerable<FileState> TrackingFiles
    {
      get
      {
        return FileStates.Where(x => !DeletedFiles.Contains(x));
      }
    }
    public IEnumerable<FileState> UpdatedFiles
    {
      get
      {
        return TrackingFiles.Where(
        x =>
        {
          var lastWrite = Math.Floor(File.GetLastWriteTime(CombinePath(x.File)).Subtract(DateTime.UnixEpoch).TotalSeconds);
          var committedWrite = Math.Floor(x.LastWrite.Subtract(DateTime.UnixEpoch).TotalSeconds);
          return lastWrite > committedWrite;
        }
      );
      }
    }
    public bool HasUncommittedChanges
    {
      get
      {
        return
          AddedFiles.Any() ||
        UpdatedFiles.Any() ||
        DeletedFiles.Any()
        ;
      }
    }
    private string CombinePath(string file)
    {
      return Path.Combine(Folder, file);
    }
    public void Commit()
    {
      foreach (var updatedFile in UpdatedFiles)
      {
        updatedFile.LastWrite = File.GetLastWriteTime(CombinePath(updatedFile.File));
      }
      foreach (var addedFile in AddedFiles)
      {
        FileStates = [.. FileStates, new FileState(addedFile!) { LastWrite = File.GetLastWriteTime(CombinePath(addedFile!)) }];
      }
      FileStates = FileStates.Where(x => !DeletedFiles.Contains(x)).ToArray();
    }
    public void Save()
    {
      using var sw = new StreamWriter(InfoFile, false);
      sw.Write(this);
    }
    public override string ToString()
    {
      StringBuilder b = new();
      b.AppendLine("commit:");
      b.AppendLine(Math.Floor(DateTime.UtcNow.Subtract(DateTime.UnixEpoch).TotalSeconds).ToString());
      if (!string.IsNullOrWhiteSpace(Remote))
      {
        b.AppendLine(":remote:");
        b.AppendLine(Remote);
      }
      if (IgnoreFiles.Length > 0)
      {
        b.AppendLine("ignore:");
        b.AppendLine(string.Join(Environment.NewLine, IgnoreFiles).Trim());
        b.AppendLine(":end ignore");
      }
      if (FileStates.Length > 0)
      {
        b.AppendLine("files:");
        b.Append(string.Join(Environment.NewLine, FileStates.Select(x => x.ToString())));
      }
      return b.ToString();
    }
    public Watcher(string Folder, bool createNew = false)
    {
      this.Folder = Folder;
      if (createNew) return;
      if (!File.Exists(InfoFile))
        throw new ArgumentException("info file does not exists");
      using var sr = new StreamReader(InfoFile);
      var lines = sr.ReadToEnd().Split(Environment.NewLine).ToArray().GetEnumerator();
      while (lines.MoveNext())
      {
        var line = lines.Current as string;
        switch (line)
        {
          case "commit:":
            lines.MoveNext();
            if (lines.Current is not string commit)
              throw new ArgumentException("no commit time");
            if (!double.TryParse(commit,out double commitTime))
              throw new ArgumentException("commit is not number");
            CommitTime = DateTime.UnixEpoch.AddSeconds(commitTime);
              break;
          case "remote:":
            lines.MoveNext();
            if (lines.Current is not string remote)
              throw new ArgumentException("remote is not string");
            Remote = remote ?? throw new ArgumentException("remote is null");
            break;
          case "ignore:":
            StringBuilder ignoreFiles = new();
            while (lines.MoveNext())
            {
              if (lines.Current is not string ignoreFile)
                throw new ArgumentException("ignore is null");
              if (string.IsNullOrWhiteSpace(ignoreFile))
                throw new ArgumentException("no end ignore");
              if (ignoreFile == ":end ignore") break;
              ignoreFiles.AppendLine(ignoreFile);
            }
            IgnoreFiles = ignoreFiles.ToString().Trim().Split(Environment.NewLine);
            break;
          case "files:":
            StringBuilder files = new();
            while (lines.MoveNext())
            {
              if (lines.Current is not string file)
                throw new ArgumentException("file is null");
              files.AppendLine(file);
            }
            string[] fileArray = files.ToString().Split(Environment.NewLine).Where(x => !string.IsNullOrEmpty(x)).ToArray();
            Regex rgx = CommitRegex();
            List<FileState> states = [];
            foreach (string file in fileArray)
            {
              var match = rgx.Match(file)
                ?? throw new ArgumentException(string.Format("file parse error {0}", file));
              if (!double.TryParse(match.Groups[1].Value, out double lastWrite))
                throw new ArgumentException($"file ts is not int");
              var fs = new FileState(
                match.Groups[2].Value
              )
              { LastWrite = DateTime.UnixEpoch.AddSeconds(lastWrite) };
              states.Add(fs);
            }
            FileStates = [.. states];
            break;
        }
      }
      GC.Collect();
    }
    [GeneratedRegex(@"(\d*)\t(.*)")]
    private static partial Regex CommitRegex();

    public void Dispose()
    {
      GC.SuppressFinalize(this);
      FileStates = [];
      IgnoreFiles = [];
      Remote = string.Empty;
      Folder = string.Empty;
      GC.Collect();
    }
  }
  static class Extension
  {
    public static string Nice(this IEnumerable arr)
    {
      return $"[{string.Join(", ", arr.Cast<object>())}]";
    }
  }
}