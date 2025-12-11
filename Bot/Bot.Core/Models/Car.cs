using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Core.Models
{
    public class Car
    {
        public int Id { get; set; }
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public string LicensePlate { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
    }
}
