using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace tasync
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
    public static async Task<HttpResponseMessage> Make(HttpMethod method, string url, object? data,string? auth)
    {
      using var http = new HttpClient();
      var content = JsonContent.Create(data);
      http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AuthScheme, auth);
      var req = new HttpRequestMessage(method, url) { Content = content };
      return await http.SendAsync(req);
    }
  }
}