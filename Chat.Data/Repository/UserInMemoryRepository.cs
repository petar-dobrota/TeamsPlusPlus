using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Chat.Data.Domain;

namespace Chat.Data.Repository
{
    public class UserInMemoryRepository : IUserRepository
    {
        public UserInMemoryRepository()
        {
            var dummyUser = new UserInfo("myUserId");
            dummyUser.joinedRooms.Add("public");
            dummyUser.joinedRooms.Add("myroom");
            userInfos[dummyUser.userId] = dummyUser;
        }

        private Dictionary<string, UserInfo> userInfos = new Dictionary<string, UserInfo>();

        public Task<UserInfo> GetUserInfoAsync(string userId, CancellationToken cancellationToken)
        {
            userInfos.TryGetValue(userId, out var retVal);
            return Task.FromResult(retVal);
        }

        public Task PersistAsync(UserInfo newInfo, CancellationToken cancellationToken)
        {
            userInfos[newInfo.userId] = newInfo;
            return Task.CompletedTask;
        }
    }
}
