using Bot.Core;

namespace Bot.API.Controllers
{
    public class ChatController
    {
        private readonly KernelService _kernelService;

        public ChatController(KernelService kernelService)
        {
            _kernelService = kernelService;
        }

    }
}
