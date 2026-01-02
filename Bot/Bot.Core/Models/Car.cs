using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Bot.Core.Models
{
    public class Car
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("brand")]
        public string Brand { get; set; } = string.Empty;

        [JsonPropertyName("model")]
        public string Model { get; set; } = string.Empty;

        [JsonPropertyName("year")]
        public int Year { get; set; }

        [JsonPropertyName("licensePlate")]
        public string LicensePlate { get; set; } = string.Empty;

        [JsonPropertyName("imageUrl")]
        public string ImageUrl { get; set; } = string.Empty;
    }
}
