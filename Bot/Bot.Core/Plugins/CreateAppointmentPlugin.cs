using Microsoft.SemanticKernel;
using System.ComponentModel;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Bot.Core.Models;

namespace Bot.Core.Plugins
{
    public class CreateAppointmentPlugin
    {
        private readonly HttpClient _http;

        public CreateAppointmentPlugin()
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

        [KernelFunction("create_appointment")]
        [Description("Creates a new garage appointment. Requires carId, serviceId, appointmentDate, customerName, customerEmail, and optional notes. Returns the created appointment as JSON.")]
        public async Task<string> CreateAsync(
            [Description("ID of the car")] int carId,
            [Description("ID of the service")] int serviceId,
            [Description("Appointment date and time (format: yyyy-MM-ddTHH:mm:ss)")] DateTime appointmentDate,
            [Description("Customer name")] string customerName,
            [Description("Customer email")] string customerEmail,
            [Description("Special notes or requests (optional)")] string? specialNotes = null)
        {
            try
            {
                // First, get the car details
                var carResponse = await _http.GetAsync($"cars/{carId}");
                Car? car = null;
                if (carResponse.IsSuccessStatusCode)
                {
                    car = await carResponse.Content.ReadFromJsonAsync<Car>();
                }

                // Then, get the service details
                var serviceResponse = await _http.GetAsync($"services/{serviceId}");
                Service? service = null;
                if (serviceResponse.IsSuccessStatusCode)
                {
                    service = await serviceResponse.Content.ReadFromJsonAsync<Service>();
                }

                // Create the appointment
                var body = new
                {
                    carId,
                    serviceId,
                    appointmentDate,
                    customerName,
                    customerEmail,
                    specialNotes
                };

                var response = await _http.PostAsJsonAsync("appointments", body);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Serialize(new
                    {
                        error = true,
                        message = $"Failed to create appointment. Status: {response.StatusCode}. Details: {errorContent}"
                    });
                }

                var appointment = await response.Content.ReadFromJsonAsync<Appointment>();

                // Manually set the car and service objects if they were fetched successfully
                if (appointment != null)
                {
                    if (car != null)
                        appointment.Car = car;
                    if (service != null)
                        appointment.Service = service;
                }

                // Return complete JSON structure for card with all necessary data
                return JsonSerializer.Serialize(new
                {
                    answer = "Afspraak bevestigd!",
                    appointment,
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
                    message = $"Error creating appointment: {ex.Message}"
                });
            }
        }
    }
}