using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace Bot.Core
{
    public class KernelService
    {
        private readonly Kernel _kernel;
        private readonly ConcurrentDictionary<string, ChatHistory> _sessions = new();

        // ⚠️ DIT IS DE SYSTEM PROMPT – HEEL BELANGRIJK
        private readonly string systemPrompt = @"
You are GarageBot, a virtual assistant for an auto garage.
Your tasks:
- Help users register cars (brand, model, year, license plate, image)
- Show available services such as maintenance, repairs, airco, brakes, diagnosis
- Create appointments (car, service, date, notes)
- Modify appointments
- Cancel appointments
- Show the service history of a car

When the user tries to talk about anything unrelated (weather, food, school, random chat):
→ Politely refuse and explain you can only help with car-related tasks.

When listing cars, services, or appointments:
→ Always use JSON with the appropriate structure.
→ Do NOT add text outside the JSON.

When confirming an appointment:
→ Respond in JSON like:
{
  ""answer"": ""Appointment confirmed!"",
  ""appointment"": { ""carId"": """", ""serviceId"": """", ""date"": """", ""notes"": """", ""status"": ""Scheduled"" },
  ""question"": ""Would you like anything else?"" 
}

For all other conversation:
→ Reply in normal text.";

        public KernelService(Kernel kernel)
        {
            _kernel = kernel;
        }

        public async Task<string> GetChatResponseAsync(string sessionId, string userInput)
        {
            // Get the chat completion model
            var chat = _kernel.GetRequiredService<IChatCompletionService>();

            ChatHistory history;

            // Retrieve existing chat session OR create a new one
            if (_sessions.ContainsKey(sessionId))
            {
                history = _sessions[sessionId];
            }
            else
            {
                history = new ChatHistory();
                _sessions[sessionId] = history;

                var today = DateTime.Now.ToString("dddd, dd MMMM yyyy");

                // Add system prompt (garage version)
                history.AddSystemMessage(@$"
{systemPrompt}
- Today is {today}.");
            }

            // Add user input
            history.AddUserMessage(userInput);

            var settings = new OpenAIPromptExecutionSettings
            {
                FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
            };

            // Execute with plugins enabled
            var response = await chat.GetChatMessageContentAsync(
                history,
                executionSettings: settings,
                kernel: _kernel
            );

            // Store assistant reply in history
            history.AddAssistantMessage(response.Content);

            return response.Content ?? "No answer.";
        }
    }
}
