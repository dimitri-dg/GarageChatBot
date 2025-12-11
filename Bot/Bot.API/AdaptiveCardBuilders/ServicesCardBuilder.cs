using Bot.API.KernelResponses;
using System.Text.Json;

namespace Bot.API.AdaptiveCardBuilders
{
    public class ServicesCardBuilder
    {
        public static string Build(ServicesResponse response)
        {
            var body = new List<object>();

            // Titel / antwoord
            body.Add(new
            {
                type = "TextBlock",
                text = response.Answer,
                wrap = true,
                weight = "Bolder",
                size = "Medium"
            });

            bool first = true;

            foreach (var service in response.Services)
            {
                body.Add(new
                {
                    type = "Container",
                    separator = !first,
                    spacing = "Medium",
                    items = new object[]
                    {
                        new {
                            type = "ColumnSet",
                            columns = new object[]
                            {
                                // GEEN IMAGE-KOLOM → WEGLATEN
                                
                                new {
                                    type = "Column",
                                    width = "stretch",
                                    items = new object[]
                                    {
                                        new {
                                            type = "TextBlock",
                                            text = service.Name,
                                            weight = "Bolder",
                                            wrap = true
                                        },
                                        new {
                                            type = "TextBlock",
                                            text = service.Description,
                                            wrap = true
                                        },
                                        new {
                                            type = "TextBlock",
                                            text = $"{service.Price} euro",
                                            wrap = true
                                        },
                                        new {
                                            type = "TextBlock",
                                            text = $"Duration: {service.DurationMinutes} min",
                                            wrap = true,
                                            spacing = "Small",
                                            color = "Accent"
                                        }
                                    }
                                }
                            }
                        }
                    }
                });

                first = false;
            }

            // Follow-up Question
            body.Add(new
            {
                type = "TextBlock",
                text = response.Question,
                wrap = true,
                spacing = "Medium"
            });

            var adaptiveCard = new Dictionary<string, object>
            {
                ["$schema"] = "http://adaptivecards.io/schemas/adaptive-card.json",
                ["type"] = "AdaptiveCard",
                ["version"] = "1.0",
                ["body"] = body
            };

            return JsonSerializer.Serialize(adaptiveCard);
        }
    }
}
