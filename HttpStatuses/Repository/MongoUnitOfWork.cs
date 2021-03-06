using System;
using System.Collections.Concurrent;
using MongoDB.Driver;

namespace HttpStatuses.Repository
{
  public interface IMongoUnitOfWork
  {
    IMongoDatabase GetDatabase();
    IMongoRepository<T> GetRepository<T>() where T : class;
  }

  public class MongoUnitOfWork : IMongoUnitOfWork
  {
    private readonly IMongoDbContext _context;
    private ConcurrentDictionary<Type, object> repositories;
    private IClientSessionHandle _session;

    public MongoUnitOfWork(IMongoDbContext context)
    {
      _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public IMongoRepository<T> GetRepository<T>() where T : class
    {
      if (repositories == null)
        repositories = new ConcurrentDictionary<Type, object>();

      var type = typeof(T);
      if (!repositories.ContainsKey(type))
      {
        repositories[type] = new MongoRepository<T>(_context);
      }

      return (IMongoRepository<T>) repositories[type];
    }

    public IClientSessionHandle GetSession() => _session;

    public IMongoDatabase GetDatabase() => _context.Database;
  }
}