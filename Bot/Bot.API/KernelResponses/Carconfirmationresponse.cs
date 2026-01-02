using System.Text.Json.Serialization;
using Bot.Core.Models;

namespace Bot.API.KernelResponses
{
    public class CarConfirmationResponse
    {
        [JsonPropertyName("answer")]
        public string Answer { get; set; }

        [JsonPropertyName("car")]
        public Car Car { get; set; }

        [JsonPropertyName("question")]
        public string Question { get; set; }
    }
}