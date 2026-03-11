using System.Collections.Generic;
using Newtonsoft.Json;

namespace ZxInfoBot.Models
{
    public class SimpleGameData
    {
        [JsonProperty("id")] public string Id { get; set; }
        [JsonProperty("title")] public string Title { get; set; }
    }
}