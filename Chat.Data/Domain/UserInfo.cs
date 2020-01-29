using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.Data.Domain
{
    public class UserInfo
    {
        public UserInfo(string userId) { this.userId = userId; }
        public readonly string userId;
        public readonly List<string> joinedRooms = new List<string>();
    }
}
