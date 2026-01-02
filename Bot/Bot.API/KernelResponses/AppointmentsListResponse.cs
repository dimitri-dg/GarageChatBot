using System.Text.Json.Serialization;
using Bot.Core.Models;

namespace Bot.API.KernelResponses
{
    public class AppointmentsListResponse
    {
        [JsonPropertyName("answer")]
        public string Answer { get; set; }

        [JsonPropertyName("appointments")]
        public List<Appointment> Appointments { get; set; }

        [JsonPropertyName("question")]
        public string Question { get; set; }
    }
}
