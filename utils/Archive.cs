using System.IO.Compression;

namespace Tasync.Utils
{
    public static class Archive
    {
        public static void ExtractTo(Stream archiveStream, string extractDir)
        {
            using var zipArchive = new ZipArchive(archiveStream, ZipArchiveMode.Read,false);
            zipArchive.ExtractToDirectory(extractDir, true);
            // todo create .tasync file in /utils/InfoFile.cs...
        }
    }
}
