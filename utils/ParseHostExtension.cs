namespace Tasync.Utils
{
    public record Host(string? host, string? folderName)
    {
        public override string ToString()
        {
            if (host is null) return string.Empty;
            return new UriBuilder(host) { Path = folderName }.Uri.ToString();
        }
    }
    public static class HostExtension
    {
        public static Host? ParseHost(this string host)
        {
            try
            {
                var uri = new Uri(host);
                return new Host(uri.GetLeftPart(UriPartial.Path), uri.Fragment.Trim('#'));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to parse host: {0}", ex.Message);
                return null;
            }
        }
    }
}
