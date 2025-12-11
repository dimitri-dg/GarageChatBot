using Bot.API.KernelResponses;
using System.Text.Json;

namespace Bot.API.AdaptiveCardBuilders
{
    public class CarsCardBuilder
    {
        public static string Build(CarsResponse response)
        {
            var body = new List<object>();

            body.Add(new
            {
                type = "TextBlock",
                text = response.Answer,
                weight = "Bolder",
                size = "Large",
                spacing = "Medium"
            });

            // Buttons for each car
            body.Add(new
            {
                type = "ActionSet",
                actions = response.Cars.Select(car => new
                {
                    type = "Action.Submit",
                    title = $"{car.Brand} {car.Model} ({car.LicensePlate})",
                    data = new
                    {
                        msteams = new
                        {
                            type = "messageBack",
                            text = $"Select car {car.Id}"
                        }
                    }
                }).ToList()
            });

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
