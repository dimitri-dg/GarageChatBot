using Microsoft.SemanticKernel;
using System.ComponentModel;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Bot.Core.Models;

namespace Bot.Core.Plugins
{
    public class ModifyAppointmentPlugin
    {
        private readonly HttpClient _http;

        public ModifyAppointmentPlugin()
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

        [KernelFunction("modify_appointment")]
        [Description("Modifies an existing appointment by changing the date or special notes. Returns the updated appointment as JSON.")]
        public async Task<string> ModifyAsync(
            [Description("ID of the appointment to modify")] int appointmentId,
            [Description("New appointment date and time (format: yyyy-MM-ddTHH:mm:ss)")] DateTime appointmentDate,
            [Description("New special notes (optional)")] string? specialNotes = null)
        {
            try
            {
                // Match met je API DTO: AppointmentForUpdateDto
                var body = new
                {
                    appointmentDate,
                    specialNotes
                };

                var response = await _http.PutAsJsonAsync($"appointments/{appointmentId}", body);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Serialize(new
                    {
                        error = true,
                        message = $"Failed to modify appointment. Status: {response.StatusCode}. Details: {errorContent}"
                    });
                }

                var appointment = await response.Content.ReadFromJsonAsync<Appointment>();
                return JsonSerializer.Serialize(new { appointment });
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
                    message = $"Error modifying appointment: {ex.Message}"
                });
            }
        }
    }
}