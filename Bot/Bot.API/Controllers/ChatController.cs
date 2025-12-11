using System.Text.Json;
using Bot.Core;
using Microsoft.AspNetCore.Mvc;

namespace Bot.API.Controllers
{
    [ApiController]
    [Route("api/chat")]
    public class ChatController : ControllerBase
    {
        private readonly KernelService _kernelService;

        public ChatController(KernelService kernelService)
        {
            _kernelService = kernelService;
        }

        [HttpPost]
        public async Task<IActionResult> Post(ChatRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var kernelReply = await _kernelService.GetChatResponseAsync(
                request.SessionId, request.Message);

            string reply;

            // --- JSON direct naar cards mappen ---
            if (HasKey(kernelReply, "cars"))
            {
                var data = JsonSerializer.Deserialize<CarsResponse>(kernelReply);
                reply = CarsCardBuilder.Build(data);
            }
            else if (HasKey(kernelReply, "services"))
            {
                var data = JsonSerializer.Deserialize<ServicesResponse>(kernelReply);
                reply = ServicesCardBuilder.Build(data);
            }
            else if (HasKey(kernelReply, "appointment"))
            {
                var data = JsonSerializer.Deserialize<AppointmentResponse>(kernelReply);
                reply = AppointmentCardBuilder.Build(data);
            }
            else
            {
                // gewone tekst
                reply = kernelReply;
            }

            return Ok(new ChatResponseDto { Reply = reply });
        }

        private bool HasKey(string result, string key)
        {
            try
            {
                using var doc = JsonDocument.Parse(result);
                return doc.RootElement.TryGetProperty(key, out _);
            }
            catch
            {
                return false;
            }
        }
    }
}
