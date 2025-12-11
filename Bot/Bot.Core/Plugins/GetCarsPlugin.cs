using Microsoft.SemanticKernel;
using System.ComponentModel;
using System.Net.Http.Json;
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
                BaseAddress = new Uri(config["GarageApi:Url"])
            };
        }

        [KernelFunction("get_cars")]
        [Description("Gets all cars registered in the garage.")]
        public async Task<List<Car>> GetCarsAsync()
        {
            return await _http.GetFromJsonAsync<List<Car>>("cars")
                   ?? new List<Car>();
        }
    }
}
