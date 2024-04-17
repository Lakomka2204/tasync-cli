using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Tasync.Responses;

namespace Tasync
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
    public static async Task<HttpResponseMessage> Make(HttpMethod method, Uri url, object? data, string? auth = null)
    {

      using var http = new HttpClient();
      http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AuthScheme, auth);
      var req = new HttpRequestMessage(method, url);
      if (data is not null)
        req.Content = JsonContent.Create(data);
      // req.Content.ReadAsStream().CopyTo(Console.OpenStandardError());
      return await http.SendAsync(req);
    }
    public static Uri ComposeUri(string scheme,string baseUrl, string path, string? query = null)
    {
      return new UriBuilder()
      {
        Scheme = scheme,
        Host = baseUrl,
        Path = path,
        Query = query
      }.Uri;
    }
    public static int PrintHttpErrorAndExit(ErrorResponse? error)
    {
      if (error is null) return 1;
      if (error.Message?.ToString() is string singleError)
      {
        Console.WriteLine(ErrorPrefix, singleError);
        return 1;
      }
      if (error.Message is JsonElement arrayError && arrayError.ValueKind == JsonValueKind.Array)
      {
        Console.WriteLine(ErrorPrefix,string.Join('\n',arrayError.EnumerateArray()));
        return 1;
      }
      return -1;
    }
  }
}
