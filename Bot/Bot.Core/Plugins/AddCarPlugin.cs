using Microsoft.SemanticKernel;
using System.ComponentModel;
using System.Net.Http.Json;
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
                BaseAddress = new Uri(config["GarageApi:Url"])
            };
        }

        [KernelFunction("add_car")]
        [Description("Adds a new car to the system. Returns the created car.")]
        public async Task<Car?> AddCarAsync(
            [Description("Brand of the car")] string brand,
            [Description("Model of the car")] string model,
            [Description("Year of build")] int year,
            [Description("License plate")] string licensePlate)
        {
            var body = new { brand, model, year, licensePlate };
            var response = await _http.PostAsJsonAsync("cars", body);

            return await response.Content.ReadFromJsonAsync<Car>();
        }
    }
}
