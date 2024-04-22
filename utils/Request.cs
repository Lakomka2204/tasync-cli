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
    public static async Task<HttpResponseMessage?> Make(HttpMethod method, Uri url, string auth, string[] files, string[] ignoredFiles)
    {
      List<Stream> streams = [];
      try
      {
        using var http = new HttpClient();
        var req = new HttpRequestMessage(method, url);
        var content = new MultipartFormDataContent();
        foreach (var file in files)
        {
          FileStream stream = new(file, FileMode.Open);
          streams.Add(stream);
          Console.WriteLine("File that is being sent {0}", file);
          var streamContent = new StreamContent(stream);
          streamContent.Headers.ContentType = new MediaTypeHeaderValue(MimeMapping.MimeUtility.GetMimeMapping(file), "UTF-8");
          content.Add(streamContent, "file", file);
        }
        req.Headers.Authorization = new AuthenticationHeaderValue(AuthScheme, auth);
        req.Content = content;
        req.Content.Headers.ContentType!.CharSet = "UTF-8";
        req.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
        req.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
        req.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));
        req.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("br"));
        Console.WriteLine("ignore {0}", ignoredFiles.Nice());
        var bbbs = req.Headers.TryAddWithoutValidation("Ignore", ignoredFiles);
        Console.WriteLine("added w/out validation {0}", bbbs);
        return await http.SendAsync(req);
      }
      catch (Exception ex)
      {
        Console.Error.WriteLine("Error: {0}", ex.Message);
        Environment.ExitCode = ex.HResult;
        return null;
      }
      finally
      {
        foreach (var s in streams)
          await s.DisposeAsync();
      }
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
