using Million.Domain.Entities;

namespace Million.Domain.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<IEnumerable<User>> GetUsersByRoleAsync(string role);
        Task<bool> ValidateCredentialsAsync(string email, string password);
    }
}
