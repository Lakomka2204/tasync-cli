using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Tasync.Responses;

namespace Tasync.Utils
{
    public static class Request
    {
        public const string AuthScheme = "Bearer";
        /// <summary>
        /// wrapped request
        /// </summary>
        /// <param name="method">HttpMethod.?</param>
        /// <param name="url">https://123.456/path</param>
        /// <param name="data">new {a="3",b=true,c=3.54}</param>
        /// <param name="auth">raw header without bearer</param>
        /// <returns>http result</returns>
        public static async Task<HttpResponseMessage> Make(HttpMethod method, Uri url, string? auth = null, object? data = null)
        {

            using var http = new HttpClient();
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AuthScheme, auth);
            var req = new HttpRequestMessage(method, url);
            if (data is not null)
                req.Content = JsonContent.Create(data);
            return await http.SendAsync(req);
        }
        public static async Task<HttpResponseMessage> Make(HttpMethod method, Uri url, string auth, string[] files)
        {
            Console.WriteLine("Creating http");
            using var http = new HttpClient();
            Console.WriteLine("set authorization");
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AuthScheme, auth);
            Console.WriteLine("compose http req message");
            var req = new HttpRequestMessage(method, url);
            Console.WriteLine("creating multipart content");
            var content = new MultipartFormDataContent();
            Console.WriteLine("iter files");
            foreach (var file in files)
            {
                Console.WriteLine("file {0}", file);
                var fileStream = new FileStream(file, FileMode.Open);
                Console.WriteLine("created stream");
                var fileContent = new StreamContent(fileStream);
                Console.WriteLine("created streamcontent");
                fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = "file",
                    FileName = Path.GetFileName(file)
                };
                Console.WriteLine("streamcontent headers");
                content.Add(fileContent);
                Console.WriteLine("add to content");
            }
            req.Content = content;
            Console.WriteLine("seinding...");
            return await http.SendAsync(req);
        }
        public static Uri ComposeUri(string baseUrl, string path, string? query = null)
        {
            return new UriBuilder(baseUrl)
            {
                Path = path,
                Query = query
            }.Uri;
        }
        public static int PrintHttpErrorAndExit(ErrorResponse? error)
        {
            if (error is null) return 1;
            if (error.Message is JsonElement arrayError && arrayError.ValueKind == JsonValueKind.Array)
            {
                Console.WriteLine("HTTP Errors: {0}", Environment.NewLine + string.Join(Environment.NewLine, arrayError.EnumerateArray()));
                return 1;
            }
            else if (error.Message?.ToString() is string singleError)
            {
                Console.WriteLine("HTTP Error: {0}", singleError);
                return 1;
            }
            return -1;
        }
    }
}
