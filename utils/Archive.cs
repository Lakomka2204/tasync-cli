using System.IO.Compression;
using System.Text;

namespace Tasync.Utils
{
    public static class Archive
    {
        public static void ExtractTo(Stream archiveStream, string extractDir)
        {
            using ZipArchive archive = new ZipArchive(archiveStream, ZipArchiveMode.Read,false, Encoding.UTF8);
            archive.ExtractToDirectory(extractDir);
        }
    }
}
