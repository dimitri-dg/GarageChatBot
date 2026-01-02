using Bot.Core.Models;
using System.Text.Json;

namespace Bot.API.AdaptiveCardBuilders
{
    public class CarConfirmationCardBuilder
    {
        public static string Build(string answer, Car car, string question)
        {
            var body = new List<object>();

            // Success header
            body.Add(new
            {
                type = "Container",
                style = "good",
                items = new object[]
                {
                    new
                    {
                        type = "ColumnSet",
                        columns = new object[]
                        {
                            new
                            {
                                type = "Column",
                                width = "auto",
                                items = new object[]
                                {
                                    new
                                    {
                                        type = "Image",
                                        url = "https://cdn-icons-png.flaticon.com/512/3547/3547287.png",
                                        size = "Small",
                                        altText = "Success",
                                        pixelWidth = 40,
                                        pixelHeight = 40
                                    }
                                }
                            },
                            new
                            {
                                type = "Column",
                                width = "stretch",
                                items = new object[]
                                {
                                    new
                                    {
                                        type = "TextBlock",
                                        text = "Auto Toegevoegd!",
                                        weight = "Bolder",
                                        size = "Large",
                                        wrap = true
                                    },
                                    new
                                    {
                                        type = "TextBlock",
                                        text = answer,
                                        wrap = true,
                                        isSubtle = true
                                    }
                                }
                            }
                        }
                    }
                }
            });

            // Car details
            body.Add(new
            {
                type = "Container",
                spacing = "Medium",
                separator = true,
                items = new object[]
                {
                    new
                    {
                        type = "TextBlock",
                        text = "Auto Details",
                        weight = "Bolder",
                        size = "Medium"
                    },
                    new
                    {
                        type = "FactSet",
                        facts = new object[]
                        {
                            new { title = "Merk:", value = car.Brand },
                            new { title = "Model:", value = car.Model },
                            new { title = "Jaar:", value = car.Year.ToString() },
                            new { title = "Kenteken:", value = car.LicensePlate },
                            new { title = "ID:", value = car.Id.ToString() }
                        }
                    }
                }
            });

            // Follow-up question
            body.Add(new
            {
                type = "TextBlock",
                text = question,
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