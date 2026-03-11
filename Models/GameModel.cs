using Newtonsoft.Json;

namespace ZxInfoBot.Models
{
    public class GameModel
    {
        [JsonProperty("_seq_no")] public long SeqNo { get; set; }
        [JsonProperty("found")] public bool Found { get; set; }
        [JsonProperty("_index")] public string Index { get; set; }
        [JsonProperty("_source")] public Source Source { get; set; }
        [JsonProperty("_id")] public string Id { get; set; }
        [JsonProperty("_version")] public long Version { get; set; }
        [JsonProperty("_primary_term")] public long PrimaryTerm { get; set; }
    }
}