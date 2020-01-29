using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamsPlusPlus
{
    public class LoginModel
    {
        public static readonly LoginModel Instance = new LoginModel();

        private ChatRoomsModel chatRoomsModel = ChatRoomsModel.Instance;

        public string MyUserId { get; private set; } = "myUserId";

        public string ServerAddress { get; private set; } = "http://localhost:8906";

        public void LoginAs(string userId)
        {
            MyUserId = userId;
            chatRoomsModel.Reset();
        }
    }
}
