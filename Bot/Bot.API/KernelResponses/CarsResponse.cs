using System.Text.Json.Serialization;
using Bot.Core.Models;

namespace Bot.API.KernelResponses
{
    public class CarsResponse
    {
        [JsonPropertyName("answer")]
        public string Answer { get; set; }

        [JsonPropertyName("cars")]
        public List<Car> Cars { get; set; }

        [JsonPropertyName("question")]
        public string Question { get; set; }
    }
}
