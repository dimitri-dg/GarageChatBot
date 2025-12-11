using Microsoft.SemanticKernel;
using System.ComponentModel;
using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using Bot.Core.Models;

namespace Bot.Core.Plugins
{
    public class ModifyAppointmentPlugin
    {
        private readonly HttpClient _http;

        public ModifyAppointmentPlugin()
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

        [KernelFunction("modify_appointment")]
        [Description("Modifies fields of an existing appointment.")]
        public async Task<Appointment?> ModifyAsync(
            int id,
            DateTime? date,
            string? notes,
            string? status)
        {
            var body = new { date, notes, status };

            var response = await _http.PutAsJsonAsync($"appointments/{id}", body);
            return await response.Content.ReadFromJsonAsync<Appointment>();
        }
    }
}
