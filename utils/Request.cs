using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Tasync.Responses;

namespace Tasync.Utils
{
  public static class Request
  {
    public const string AuthScheme = "Bearer";
    public const string ErrorPrefix = "HTTP Error {0}";
    /// <summary>
    /// wrapped request
    /// </summary>
    /// <param name="method">HttpMethod.?</param>
    /// <param name="url">https://123.456/path</param>
    /// <param name="data">new {a="3",b=true,c=3.54}</param>
    /// <param name="auth">raw header without bearer</param>
    /// <returns>http result</returns>
    public static async Task<HttpResponseMessage> Make(HttpMethod method, Uri url, object? data = null, string? auth = null)
    {

      using var http = new HttpClient();
      http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AuthScheme, auth);
      var req = new HttpRequestMessage(method, url);
      if (data is not null)
        req.Content = JsonContent.Create(data);
      return await http.SendAsync(req);
    }
    public static async Task<HttpResponseMessage> Make(HttpMethod method, Uri url, string[] files, string? auth = null)
    {

      using var http = new HttpClient();
      http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AuthScheme, auth);
      var req = new HttpRequestMessage(method, url);
      var content = new MultipartFormDataContent();
      foreach (var file in files)
      {
        using var fileStream = new FileStream(file,FileMode.Open);
        var fileContent = new StreamContent(fileStream);
        fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
        {
            Name = "files",
            FileName = Path.GetFileName(file)
        };
        content.Add(fileContent);
      }
      req.Content = content;
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
        Console.WriteLine(ErrorPrefix,Environment.NewLine+string.Join(Environment.NewLine,arrayError.EnumerateArray()));
        return 1;
      }
      else if (error.Message?.ToString() is string singleError)
      {
        Console.WriteLine(ErrorPrefix, singleError);
        return 1;
      }
      return -1;
    }
  }
}
