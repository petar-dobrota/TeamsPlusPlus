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
        private UserRepository userRepo;

        public UserService(UserRepository userRepo)
        {
            this.userRepo = userRepo;
        }

        public async Task<UserInfo> GetUserInfoAsync(string userId, CancellationToken cancellationToken)
        {
            var userInfo = await userRepo.GetUserInfoAsync(userId, cancellationToken);
            if (userInfo == null)
            {
                userInfo = new UserInfo(userId);
                await userRepo.PersistAsync(userInfo, cancellationToken);
            }
            return userInfo;
        }

        public async Task JoinRoomAsync(string userId, string roomName, CancellationToken cancellationToken)
        {
            var user = await userRepo.GetUserInfoAsync(userId, cancellationToken);

            HashSet<string> usersRooms = new HashSet<string>(user.joinedRooms);
            usersRooms.Add(roomName);

            UserInfo newInfo = new UserInfo(userId);
            newInfo.joinedRooms.AddRange(usersRooms);

            await userRepo.PersistAsync(newInfo, cancellationToken);
        }
    }
}
