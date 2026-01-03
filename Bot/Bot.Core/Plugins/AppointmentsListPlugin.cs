using System.ComponentModel;
using System.Net.Http.Json;
using System.Text.Json;
using Bot.Core.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;

namespace Bot.Core.Plugins
{
    public class AppointmentsListPlugin
    {
        private readonly HttpClient _http;

        public AppointmentsListPlugin()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            _http = new HttpClient
            {
                BaseAddress = new Uri(config["GarageApi:Url"]
                    ?? throw new ArgumentNullException("GarageApi:Url not configured"))
            };
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