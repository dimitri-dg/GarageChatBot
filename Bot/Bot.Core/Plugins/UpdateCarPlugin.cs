using Microsoft.SemanticKernel;
using System.ComponentModel;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Bot.Core.Models;

namespace Bot.Core.Plugins
{
    public class UpdateCarPlugin
    {
        private readonly HttpClient _http;

        public UpdateCarPlugin()
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

        [KernelFunction("update_car")]
        [Description("Updates information of an existing car. All fields are required. Returns the updated car as JSON.")]
        public async Task<string> UpdateCarAsync(
            [Description("ID of the car to update")] int carId,
            [Description("Brand of the car")] string brand,
            [Description("Model of the car")] string model,
            [Description("Year of build")] int year,
            [Description("License plate")] string licensePlate)
        {
            try
            {
                // PUT requires all fields (niet partial update)
                var body = new
                {
                    brand,
                    model,
                    year,
                    licensePlate
                };

                var response = await _http.PutAsJsonAsync($"cars/{carId}", body);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Serialize(new
                    {
                        error = true,
                        message = $"Failed to update car. Status: {response.StatusCode}. Details: {errorContent}"
                    });
                }

                var car = await response.Content.ReadFromJsonAsync<Car>();
                return JsonSerializer.Serialize(new { car });
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
                    message = $"Error updating car: {ex.Message}"
                });
            }
        }
    }
}