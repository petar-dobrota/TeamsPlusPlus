using System.Threading;
using System.Threading.Tasks;
using Chat.Data.Domain;

namespace Chat.Data.Repository
{
    /// <summary>
    /// Persistance layer for storing User infos.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Get user info for provided userId. Or null if it's not found.
        /// </summary>
        Task<UserInfo> GetUserInfoAsync(string userId, CancellationToken cancellationToken);

        Task PersistAsync(UserInfo newInfo, CancellationToken cancellationToken);
    }
}