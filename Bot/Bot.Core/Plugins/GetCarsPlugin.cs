using Microsoft.SemanticKernel;
using System.ComponentModel;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Bot.Core.Models;

namespace Bot.Core.Plugins
{
    public class GetCarsPlugin
    {
        private readonly HttpClient _http;

        public GetCarsPlugin()
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

        [KernelFunction("get_cars")]
        [Description("Gets all registered cars from the system. Returns a JSON array of cars.")]
        public async Task<string> GetCarsAsync()
        {
            try
            {
                var response = await _http.GetAsync("cars");

                if (!response.IsSuccessStatusCode)
                {
                    return JsonSerializer.Serialize(new
                    {
                        error = true,
                        message = $"Failed to get cars. Status: {response.StatusCode}"
                    });
                }

                var cars = await response.Content.ReadFromJsonAsync<List<Car>>();

                // Return als JSON string
                return JsonSerializer.Serialize(new
                {
                    answer = "Hier zijn alle geregistreerde auto's:",
                    cars,
                    question = "Welke auto wil je selecteren?"
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
                    message = $"Error getting cars: {ex.Message}"
                });
            }
        }
    }
}