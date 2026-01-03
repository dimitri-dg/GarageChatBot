using Microsoft.SemanticKernel;
using System.ComponentModel;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Bot.Core.Models;

namespace Bot.Core.Plugins
{
    public class GetServicesPlugin
    {
        private readonly HttpClient _http;

        public GetServicesPlugin()
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

        [KernelFunction("get_services")]
        [Description("Gets all available garage services. Returns a JSON array of services with name, description, price, and duration.")]
        public async Task<string> GetServicesAsync()
        {
            try
            {
                var response = await _http.GetAsync("services");

                if (!response.IsSuccessStatusCode)
                {
                    return JsonSerializer.Serialize(new
                    {
                        error = true,
                        message = $"Failed to get services. Status: {response.StatusCode}"
                    });
                }

                var services = await response.Content.ReadFromJsonAsync<List<Service>>();

                if (services == null || !services.Any())
                {
                    return JsonSerializer.Serialize(new
                    {
                        answer = "Er zijn momenteel geen services beschikbaar.",
                        services = new List<Service>(),
                        question = "Kan ik je ergens anders mee helpen?"
                    });
                }

                return JsonSerializer.Serialize(new
                {
                    answer = "Beschikbare services:",
                    services,
                    question = "Welke service heb je nodig?"
                });
            }
            catch (HttpRequestException ex)
            {
                return JsonSerializer.Serialize(new
                {
                    error = true,
                    message = $"Cannot connect to Garage API: {ex.Message}"
                });
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new
                {
                    error = true,
                    message = $"Error getting services: {ex.Message}"
                });
            }
        }
    }
}