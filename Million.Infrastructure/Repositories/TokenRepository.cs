using Million.Domain.Entities;
using Million.Domain.Repositories;
using Million.Infrastructure.DbContexts;

namespace Million.Infrastructure.Repositories
{
    public class TokenRepository : BaseRepository<Token>, ITokenRepository
    {
        public TokenRepository(IMillionDbContext context, string collectionName) : base(context, collectionName)
        {
        }

        public Task<bool> DeactivateTokenAsync(string tokenValue)
        {
            throw new NotImplementedException();
        }

        public Task<Token?> GetActiveTokenByUserIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Token>> GetTokensByUserIdAsync(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
