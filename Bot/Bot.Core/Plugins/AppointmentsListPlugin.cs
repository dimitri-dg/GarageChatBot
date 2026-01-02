using System.ComponentModel;
using System.Net.Http.Json;
using System.Text.Json;
using Bot.Core.Models;
using Microsoft.SemanticKernel;

namespace Bot.Core.Plugins
{
    public class AppointmentsListPlugin
    {
        private readonly HttpClient _http;

        public AppointmentsListPlugin(HttpClient http)
        {
            _http = http;
        }

        [KernelFunction]
        [Description("Shows all appointments of the garage. Return JSON only.")]
        public async Task<string> GetAllAppointments()
        {
            var appointments = await _http.GetFromJsonAsync<List<Appointment>>(
                "appointments");

            var result = new
            {
                answer = "Here is an overview of all appointments:",
                appointments = appointments,
                question = "Would you like to modify or cancel an appointment?"
            };

            return JsonSerializer.Serialize(result);
        }
    }
}
