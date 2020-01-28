using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp
{
    public class LoginModel
    {
        public static readonly LoginModel Instance = new LoginModel();

        public string MyUserId = "myUserId";

        public string ServerAddress { get; private set; } = "http://localhost:8906";
    }
}
