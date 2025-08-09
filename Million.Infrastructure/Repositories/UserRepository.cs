using Million.Domain.Entities;
using Million.Domain.Repositories;
using Million.Infrastructure.DbContexts;

namespace Million.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(IMillionDbContext context) 
            : base(context, "Users")
        {
        }

        public Task<User?> GetUserByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> GetUsersByRoleAsync(string role)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ValidateCredentialsAsync(string email, string password)
        {
            throw new NotImplementedException();
        }
    }
}
