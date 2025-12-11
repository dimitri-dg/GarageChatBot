using System.Text.Json.Serialization;
using Bot.Core.Models;

namespace Bot.API.KernelResponses
{
    public class ServicesResponse
    {
        [JsonPropertyName("answer")]
        public string Answer { get; set; }

        [JsonPropertyName("services")]
        public List<Service> Services { get; set; }

        [JsonPropertyName("question")]
        public string Question { get; set; }
    }
}
