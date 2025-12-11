using Microsoft.SemanticKernel;
using System.ComponentModel;
using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using Bot.Core.Models;

namespace Bot.Core.Plugins
{
    public class CreateAppointmentPlugin
    {
        private readonly HttpClient _http;

        public CreateAppointmentPlugin()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            _http = new HttpClient
            {
                BaseAddress = new Uri(config["GarageApi:Url"])
            };
        }

        [KernelFunction("create_appointment")]
        [Description("Creates a new garage appointment.")]
        public async Task<Appointment?> CreateAsync(
            int carId,
            int serviceId,
            DateTime date,
            string customerName,
            string customerEmail,
            string? notes)
        {
            var body = new { carId, serviceId, date, customerName, customerEmail, notes };

            var response = await _http.PostAsJsonAsync("appointments", body);
            return await response.Content.ReadFromJsonAsync<Appointment>();
        }
    }
}
