using Microsoft.SemanticKernel;
using System.ComponentModel;
using System.Net.Http.Json;
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
                BaseAddress = new Uri(config["GarageApi:Url"])
            };
        }

        [KernelFunction("update_car")]
        [Description("Updates fields of a car by its ID. Only provided fields will be updated.")]
        public async Task<Car?> UpdateCarAsync(
            int id,
            string? brand,
            string? model,
            int? year,
            string? licensePlate)
        {
            var body = new { brand, model, year, licensePlate };

            var response = await _http.PutAsJsonAsync($"cars/{id}", body);
            return await response.Content.ReadFromJsonAsync<Car>();
        }
    }
}
