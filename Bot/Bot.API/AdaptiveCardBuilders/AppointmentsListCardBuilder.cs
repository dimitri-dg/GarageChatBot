using Bot.API.KernelResponses;
using System.Text.Json;

namespace Bot.API.AdaptiveCardBuilders
{
    public class AppointmentsListCardBuilder
    {
        public static string Build(AppointmentsListResponse response)
        {
            var body = new List<object>();

            // Titel
            body.Add(new
            {
                type = "TextBlock",
                text = response.Answer,
                weight = "Bolder",
                size = "Large",
                spacing = "Medium"
            });

            if (response.Appointments == null || !response.Appointments.Any())
            {
                body.Add(new
                {
                    type = "TextBlock",
                    text = "There are no appointments.",
                    wrap = true,
                    spacing = "Medium"
                });
            }
            else
            {
                foreach (var appt in response.Appointments)
                {
                    body.Add(new
                    {
                        type = "Container",
                        separator = true,
                        spacing = "Medium",
                        items = new object[]
                        {
                            new {
                                type = "FactSet",
                                facts = new object[]
                                {
                                    new {
                                        title = "Car:",
                                        value = appt.Car != null
                                            ? $"{appt.Car.Brand} {appt.Car.Model}"
                                            : ""
                                    },
                                    new {
                                        title = "Service:",
                                        value = appt.Service?.Name ?? ""
                                    },
                                    new {
                                        title = "Date:",
                                        value = appt.Date.ToString("dd/MM/yyyy HH:mm")
                                    },
                                    new {
                                        title = "Status:",
                                        value = appt.Status
                                    }
                                }
                            }
                        }
                    });
                }
            }

            // Follow-up
            body.Add(new
            {
                type = "TextBlock",
                text = response.Question,
                wrap = true,
                spacing = "Medium"
            });

            var card = new Dictionary<string, object>
            {
                ["$schema"] = "http://adaptivecards.io/schemas/adaptive-card.json",
                ["type"] = "AdaptiveCard",
                ["version"] = "1.4",
                ["body"] = body
            };

            return JsonSerializer.Serialize(card);
        }
    }
}
