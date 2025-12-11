using System.Text.Json.Serialization;
using Bot.Core.Models;

namespace Bot.API.KernelResponses
{
    public class AppointmentResponse
    {
        [JsonPropertyName("answer")]
        public string Answer { get; set; }

        [JsonPropertyName("appointment")]
        public Appointment Appointment { get; set; }

        [JsonPropertyName("question")]
        public string Question { get; set; }
    }
}
