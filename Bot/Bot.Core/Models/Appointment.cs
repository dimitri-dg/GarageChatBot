using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Bot.Core.Models
{
    public class Appointment
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("carId")]
        public int CarId { get; set; }

        [JsonPropertyName("serviceId")]
        public int ServiceId { get; set; }

        // BELANGRIJK: Gebruik "date" zoals de werkende bot!
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        [JsonPropertyName("customerName")]
        public string CustomerName { get; set; } = string.Empty;

        [JsonPropertyName("customerEmail")]
        public string CustomerEmail { get; set; } = string.Empty;

        // BELANGRIJK: Gebruik "notes" zoals de werkende bot!
        [JsonPropertyName("notes")]
        public string? Notes { get; set; } = string.Empty;

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("car")]
        public Car? Car { get; set; }

        [JsonPropertyName("service")]
        public Service? Service { get; set; }
    }
}
