using Chat.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Chat.Data.Domain
{
    public class UserService
    {
        private IUserRepository userRepo;

        public UserService(IUserRepository userRepo)
        {
            this.userRepo = userRepo;
        }

        private async Task<UserInfo> CrateInitialUserInfoAsync(string userId, CancellationToken cancellationToken)
        {
            var userInfo = new UserInfo(userId);
            userInfo.joinedRooms.Add("public");
            await userRepo.PersistAsync(userInfo, cancellationToken);
            return userInfo;
        }

        public async Task<UserInfo> GetUserInfoAsync(string userId, CancellationToken cancellationToken)
            {
                var userInfo = await userRepo.GetUserInfoAsync(userId, cancellationToken);
                if (userInfo == null)
                {
                    userInfo = await CrateInitialUserInfoAsync(userId, cancellationToken);
                }
                return userInfo;
        }

        public async Task JoinRoomAsync(string userId, string roomName, CancellationToken cancellationToken)
        {
            var user = await userRepo.GetUserInfoAsync(userId, cancellationToken);
            if (user == null)
            {
                user = await CrateInitialUserInfoAsync(userId, cancellationToken);
            }

            HashSet<string> usersRooms = new HashSet<string>(user.joinedRooms);
            usersRooms.Add(roomName);

            UserInfo newInfo = new UserInfo(userId);
            newInfo.joinedRooms.AddRange(usersRooms);

            await userRepo.PersistAsync(newInfo, cancellationToken);
        }
    }
}
