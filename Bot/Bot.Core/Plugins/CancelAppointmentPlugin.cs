using Microsoft.SemanticKernel;
using System.ComponentModel;
using Microsoft.Extensions.Configuration;

namespace Bot.Core.Plugins
{
    public class CancelAppointmentPlugin
    {
        private readonly HttpClient _http;

        public CancelAppointmentPlugin()
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

        [KernelFunction("cancel_appointment")]
        [Description("Cancels an appointment by ID.")]
        public async Task<bool> CancelAsync(int id)
        {
            await _http.DeleteAsync($"appointments/{id}");
            return true;
        }
    }
}
