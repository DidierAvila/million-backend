using System.Linq.Expressions;
using Million.Domain.Repositories;
using Million.Infrastructure.DbContexts;
using MongoDB.Driver;

namespace Million.Infrastructure.Repositories
{
    public abstract class BaseRepository<T> : IRepository<T> where T : class
    {
        protected readonly IMongoCollection<T> _collection;

        protected BaseRepository(IMillionDbContext context, string collectionName)
        {
            _collection = context.GetType()
                               .GetProperty(collectionName)?
                               .GetValue(context) as IMongoCollection<T> 
                ?? throw new ArgumentException($"Collection {collectionName} not found");
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _collection.Find(_ => true).ToListAsync(cancellationToken);
        }

        public virtual async Task<T?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var filter = Builders<T>.Filter.Eq("Id", id);
            return await _collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
        }

        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _collection.Find(predicate).ToListAsync(cancellationToken);
        }

        public virtual async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _collection.InsertOneAsync(entity, null, cancellationToken);
            return entity;
        }

        public virtual async Task<bool> UpdateAsync(string id, T entity, CancellationToken cancellationToken = default)
        {
            var filter = Builders<T>.Filter.Eq("Id", id);
            var result = await _collection.ReplaceOneAsync(filter, entity, cancellationToken: cancellationToken);
            return result.ModifiedCount > 0;
        }

        public virtual async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            var filter = Builders<T>.Filter.Eq("Id", id);
            var result = await _collection.DeleteOneAsync(filter, cancellationToken);
            return result.DeletedCount > 0;
        }

        public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _collection.Find(predicate).AnyAsync(cancellationToken);
        }
    }
}
