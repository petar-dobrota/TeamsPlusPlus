using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Chat.Data.Domain;

namespace Chat.Data.Repository
{
    /// <summary>
    /// For testing purposes.
    /// </summary>
    public class UserInMemoryRepository : IUserRepository
    {
        private Dictionary<string, UserInfo> userInfos = new Dictionary<string, UserInfo>();

        public UserInMemoryRepository()
        {
        }

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
