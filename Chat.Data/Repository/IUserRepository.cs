using System.Threading;
using System.Threading.Tasks;
using Chat.Data.Domain;

namespace Chat.Data.Repository
{
    public interface IUserRepository
    {
        Task<UserInfo> GetUserInfoAsync(string userId, CancellationToken cancellationToken);
        Task PersistAsync(UserInfo newInfo, CancellationToken cancellationToken);
    }
}