using Million.Domain.Entities;

namespace Million.Domain.Repositories
{
    public interface ITokenRepository : IRepository<Token>
    {
        Task<IEnumerable<Token>> GetTokensByUserIdAsync(string userId);
        Task<Token?> GetActiveTokenByUserIdAsync(string userId);
        Task<bool> DeactivateTokenAsync(string tokenValue);
    }
}
