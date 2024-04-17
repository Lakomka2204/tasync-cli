using System.Text.Json.Serialization;

namespace Tasync.Responses
{
  public class AuthResponse
  {
    [JsonPropertyName("access_token")]
    public string? AccessToken { get; set; }
  }
}
