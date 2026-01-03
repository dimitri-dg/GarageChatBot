using System.ComponentModel;
using System.Net.Http.Json;
using System.Text.Json;
using Bot.Core.Models;
using Microsoft.SemanticKernel;

public class AppointmentsPlugin
{
    private readonly HttpClient _http;

    public AppointmentsPlugin(HttpClient http)
    {
        _http = http;
    }

    [KernelFunction]
    [Description("Shows all appointments of the garage. Return JSON only.")]
    public async Task<string> ShowAllAppointments()
    {
        var appointments = await _http.GetFromJsonAsync<List<Appointment>>(
            "appointments");

        var result = new
        {
            answer = "Here is an overview of all appointments:",
            appointments = appointments.Select(a => new
            {
                car = $"{a.Car.Brand} {a.Car.Model}",
                service = a.Service.Name,
                date = a.AppointmentDate.ToString("dd/MM/yyyy HH:mm"),
                status = a.Status
            }),
            question = "Would you like to modify or cancel an appointment?"
        };

        return JsonSerializer.Serialize(result);
    }
}