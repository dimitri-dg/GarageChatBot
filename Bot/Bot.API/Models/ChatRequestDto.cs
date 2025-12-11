using System.ComponentModel.DataAnnotations;

namespace Bot.API.Models
{
    public class ChatRequestDto
    {
        [Required(ErrorMessage = "SessionId is required")]
        public string SessionId { get; set; }

        [Required(ErrorMessage = "Message is required")]
        public string Message { get; set; }
    }
}
