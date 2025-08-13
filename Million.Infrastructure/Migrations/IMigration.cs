using System.Threading;
using System.Threading.Tasks;

namespace Million.Infrastructure.Migrations
{
    public interface IMigration
    {
        int Version { get; }
        string Name { get; }
        string Description { get; }
        
        Task<bool> UpAsync(CancellationToken cancellationToken = default);
        Task<bool> DownAsync(CancellationToken cancellationToken = default);
    }
}
