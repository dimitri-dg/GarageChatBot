using System.Text.Json;
using Bot.API.AdaptiveCardBuilders;
using Bot.API.KernelResponses;
using Bot.API.Models;
using Bot.Core;
using Microsoft.AspNetCore.Mvc;

namespace Bot.API.Controllers
{
    [ApiController]
    [Route("api/chat")]
    public class ChatController : ControllerBase
    {
        private readonly KernelService _kernelService;
        private readonly ILogger<ChatController> _logger;

        public ChatController(KernelService kernelService, ILogger<ChatController> logger)
        {
            _kernelService = kernelService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Post(ChatRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                _logger.LogInformation($"Received message: {request.Message}");

                var kernelReply = await _kernelService.GetChatResponseAsync(
                    request.SessionId, request.Message);

                _logger.LogInformation($"Kernel reply (first 200 chars): {kernelReply.Substring(0, Math.Min(200, kernelReply.Length))}");

                string reply;

                // Check if it's valid JSON first
                if (!IsValidJson(kernelReply))
                {
                    _logger.LogInformation("Reply is plain text, not JSON");
                    reply = kernelReply;
                    return Ok(new ChatResponseDto { Reply = reply });
                }

                // --- JSON direct naar cards mappen ---
                if (HasKey(kernelReply, "cars"))
                {
                    _logger.LogInformation("Building cars card");
                    var data = JsonSerializer.Deserialize<CarsResponse>(kernelReply);
                    reply = CarsCardBuilder.Build(data);
                }
                else if (HasKey(kernelReply, "car") && !HasKey(kernelReply, "appointment"))
                {
                    // Single car confirmation (when adding a car)
                    _logger.LogInformation("Building car confirmation card");
                    var data = JsonSerializer.Deserialize<CarConfirmationResponse>(kernelReply);
                    reply = CarConfirmationCardBuilder.Build(data.Answer, data.Car, data.Question);
                }
                else if (HasKey(kernelReply, "services"))
                {
                    _logger.LogInformation("Building services card");
                    var data = JsonSerializer.Deserialize<ServicesResponse>(kernelReply);
                    reply = ServicesCardBuilder.Build(data);
                }
                else if (HasKey(kernelReply, "appointment"))
                {
                    _logger.LogInformation("Building appointment card");
                    var data = JsonSerializer.Deserialize<AppointmentResponse>(kernelReply);
                    reply = AppointmentCardBuilder.Build(data);
                }
                else
                {
                    // JSON maar geen bekende structuur, stuur als tekst
                    _logger.LogInformation("JSON without known structure, sending as text");
                    reply = kernelReply;
                }

                var response = new ChatResponseDto()
                {
                    Reply = reply
                };

                return Ok(response);

            }
            catch (JsonException jsonEx)
            {
                _logger.LogError($"JSON parsing error: {jsonEx.Message}");
                return Ok(new ChatResponseDto
                {
                    Reply = "Sorry, ik kreeg een onverwacht antwoord. Probeer het opnieuw."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in ChatController: {ex.Message}");
                _logger.LogError($"Stack trace: {ex.StackTrace}");
                return StatusCode(500, new
                {
                    error = ex.Message
                });
            }
        }

        private bool IsValidJson(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return false;

            text = text.Trim();

            if (!text.StartsWith("{") && !text.StartsWith("["))
                return false;

            try
            {
                using var doc = JsonDocument.Parse(text);
                return true;
            }
            catch (JsonException)
            {
                return false;
            }
        }

        private bool HasKey(string result, string key)
        {
            try
            {
                using var doc = JsonDocument.Parse(result);
                return doc.RootElement.TryGetProperty(key, out _);
            }
            catch (JsonException ex)
            {
                _logger.LogWarning($"Failed to parse as JSON in HasKey: {ex.Message}");
                return false;
            }
        }
    }
}