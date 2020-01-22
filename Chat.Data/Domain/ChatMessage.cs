// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Chat.Data.Controllers
{
    public struct ChatMessage
    {
        public ChatMessage(string _senderUser, string _messageBody) { //: this() => senderUsed = _senderUser, messageBody=null;
            senderUsed = _senderUser;
            messageBody = _messageBody;
        }

        public readonly string senderUsed;
        public readonly string messageBody;
    }
}
