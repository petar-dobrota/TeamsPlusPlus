using System.Collections.Generic;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Chat.Data.Controllers
{
    public struct ChatRoom
    {
        public string name;

        /// <summary>
        /// Oldest message is at index 0
        /// </summary>
        public List<ChatMessage> messages;
    }
}
