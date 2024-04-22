using System.IO.Compression;
using System.Text;

namespace Tasync.Utils
{
    public static class Archive
    {
        public static void ExtractTo(Stream archiveStream, string extractDir)
        {
            using GZipStream s = new GZipStream(archiveStream, CompressionMode.Decompress);
            using ZipArchive archive = new ZipArchive(s, ZipArchiveMode.Read,false, Encoding.UTF8);
            archive.ExtractToDirectory(extractDir);
        }
    }
}
