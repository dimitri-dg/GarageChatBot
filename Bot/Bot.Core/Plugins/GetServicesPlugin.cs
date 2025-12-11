using Microsoft.SemanticKernel;
using System.ComponentModel;
using System.Net.Http.Json;
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
                BaseAddress = new Uri(config["GarageApi:Url"])
            };
        }

        [KernelFunction("get_services")]
        [Description("Gets all available garage services.")]
        public async Task<List<Service>> GetServicesAsync()
        {
            return await _http.GetFromJsonAsync<List<Service>>("services")
                   ?? new List<Service>();
        }
    }
}
