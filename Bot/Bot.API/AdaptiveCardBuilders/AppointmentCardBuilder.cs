using Bot.API.KernelResponses;
using System.Text.Json;

namespace Bot.API.AdaptiveCardBuilders
{
    public class AppointmentCardBuilder
    {
        public static string Build(AppointmentResponse response)
        {
            var body = new List<object>();

            // Header block
            body.Add(new
            {
                type = "Container",
                style = "emphasis",
                spacing = "Medium",
                items = new object[]
                {
                    new {
                        type = "ColumnSet",
                        columns = new object[]
                        {
                            new {
                                type = "Column",
                                width = "auto",
                                items = new object[]
                                {
                                    new {
                                        type = "Image",
                                        url = "https://cdn-icons-png.flaticon.com/512/2966/2966327.png",
                                        size = "Small",
                                        altText = "Appointment confirmed",
                                        pixelWidth = 40,
                                        pixelHeight = 40
                                    }
                                }
                            },
                            new {
                                type = "Column",
                                width = "stretch",
                                items = new object[]
                                {
                                    new {
                                        type = "TextBlock",
                                        text = "Appointment Confirmed",
                                        weight = "Bolder",
                                        size = "Large",
                                        wrap = true
                                    },
                                    new {
                                        type = "TextBlock",
                                        text = response.Answer,
                                        wrap = true
                                    }
                                }
                            }
                        }
                    }
                }
            });

            // Appointment details
            if (response.Appointment != null)
            {
                var appt = response.Appointment;

                body.Add(new
                {
                    type = "Container",
                    spacing = "Medium",
                    items = new object[]
                    {
                        new {
                            type = "TextBlock",
                            text = "Appointment Details",
                            weight = "Bolder",
                            size = "Medium",
                            wrap = true
                        },
                        new {
                            type = "FactSet",
                            facts = new object[]
                            {
                                new { title = "Car:", value = appt.Car != null ? $"{appt.Car.Brand} {appt.Car.Model}" : "" },
                                new { title = "Service:", value = appt.Service?.Name ?? "" },
                                new { title = "Date:", value = appt.Date.ToString("dd/MM/yyyy HH:mm") },
                                new { title = "Status:", value = appt.Status },
                                new { title = "Notes:", value = appt.Notes }
                            }
                        }
                    }
                });
            }

            // Follow-up question
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
                ["version"] = "1.0",
                ["body"] = body
            };

            return JsonSerializer.Serialize(card);
        }
    }
}
