using System.Text.Json.Serialization;

namespace Tasync.Responses
{
  public class Folder
  {
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    [JsonPropertyName("ownerId")]
    public int OwnerId { get; set; }
    [JsonPropertyName("lastCommit")]
    public long? LastCommit { get; set; }
    public override string ToString()
    {
      return $"[{Id}] {Name} -> {(LastCommit is null ? "No commit" : LastCommit)}";
    }
  }
}
