using System.Threading;
using System.Threading.Tasks;
using Chat.Data.Domain;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;

namespace Chat.Data.Repository
{
    public class UserReliableRepository : IUserRepository
    {
        private readonly IReliableStateManager stateManager;

        public struct UserInfoEntity
        {
            public string userId;
            public string[] joinedRooms;
        }

        private UserInfoEntity ToEntity(UserInfo userInfo)
        {
            var userStruct = new UserInfoEntity();
            userStruct.userId = userInfo.userId;
            userStruct.joinedRooms = userInfo.joinedRooms.ToArray();

            return userStruct;
        }

        private UserInfo FromEntity(UserInfoEntity userEntity)
        {
            var userInfo = new UserInfo(userEntity.userId);
            userInfo.joinedRooms.AddRange(userEntity.joinedRooms);
            return userInfo;
        }

        public UserReliableRepository(IReliableStateManager stateManager)
        {
            this.stateManager = stateManager;
        }

        private async Task<IReliableDictionary<string, UserInfoEntity>> GetUserStatesByIdMapAsync()
        {
            return await stateManager.GetOrAddAsync<IReliableDictionary<string, UserInfoEntity>>("userInfoMap");
        }

        public async Task<UserInfo> GetUserInfoAsync(string userId, CancellationToken cancellationToken)
        {
            var userMap = await GetUserStatesByIdMapAsync();
            using (var tx = stateManager.CreateTransaction())
            {
                var userConditional = await userMap.TryGetValueAsync(tx, userId);
                return userConditional.HasValue ? FromEntity(userConditional.Value) : null;
            }
        }

        public async Task PersistAsync(UserInfo newInfo, CancellationToken cancellationToken)
        {
            var userEntity = ToEntity(newInfo);
            var userMap = await GetUserStatesByIdMapAsync();
            using (var tx = stateManager.CreateTransaction())
            {
                await userMap.AddOrUpdateAsync(tx, newInfo.userId, userEntity, (key, originalEntity) => userEntity);
                await tx.CommitAsync();
            }
        }
    }
}
