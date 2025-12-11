using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Core.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        public int CarId { get; set; }
        public int ServiceId { get; set; }
        public DateTime Date { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string? Notes { get; set; } = string.Empty;

        // bv. "Scheduled" – "Completed" – "Cancelled"
        public string Status { get; set; } = string.Empty;

        // Optioneel: extra data die de bot kan tonen
        public Car? Car { get; set; }
        public Service? Service { get; set; }
    }
}
