using System.Text.Json.Serialization;

namespace Tasync.Responses
{
  public class ErrorResponse
  {
    [JsonPropertyName("message")]
    public object? Message { get; set; }
    [JsonPropertyName("error")]
    public string? Error { get; set;}
    [JsonPropertyName("statusCode")]
    public int ErrorReason { get; set;}
  }
}
