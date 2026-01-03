using Microsoft.SemanticKernel;
using System.ComponentModel;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Bot.Core.Models;

namespace Bot.Core.Plugins
{
    public class AddCarPlugin
    {
        private readonly HttpClient _http;

        public AddCarPlugin()
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

        [KernelFunction("add_car")]
        [Description("Adds a new car to the system. Returns the created car as JSON.")]
        public async Task<string> AddCarAsync(
            [Description("Brand of the car")] string brand,
            [Description("Model of the car")] string model,
            [Description("Year of build")] int year,
            [Description("License plate")] string licensePlate)
        {
            try
            {
                var body = new { brand, model, year, licensePlate };
                var response = await _http.PostAsJsonAsync("cars", body);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Serialize(new
                    {
                        error = true,
                        message = $"Failed to add car. Status: {response.StatusCode}. Details: {errorContent}"
                    });
                }

                var car = await response.Content.ReadFromJsonAsync<Car>();
                return JsonSerializer.Serialize(new
                {
                    answer = "De nieuwe auto is succesvol aangemaakt!",
                    car,
                    question = "Kan ik nog iets anders voor je doen?"
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
                    message = $"Error adding car: {ex.Message}"
                });
            }
        }
    }
}