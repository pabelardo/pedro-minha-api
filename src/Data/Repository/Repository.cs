using System.Linq.Expressions;
using Business.Interfaces;
using Business.Models;
using Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository;

public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity, new()
{
    protected readonly MeuDbContext Db;
    protected readonly DbSet<TEntity> DbSet;
    protected Repository(MeuDbContext db)
    {
        Db = db;
        DbSet = db.Set<TEntity>();
    }

    public async Task<IEnumerable<TEntity>> BuscarAsync(Expression<Func<TEntity, bool>> predicate) =>
        await DbSet
            .AsNoTracking()
            .Where(predicate)
            .ToListAsync();

    public virtual async Task<TEntity> ObterPorIdAsync(Guid id) => await DbSet.FindAsync(id);

    public virtual Task<List<TEntity>> ObterTodosAsync() => DbSet.ToListAsync();

    public virtual async Task AdicionarAsync(TEntity entity)
    {
        DbSet.Add(entity);
        await SaveChangesAsync();
    }

    public virtual async Task AtualizarAsync(TEntity entity)
    {
        DbSet.Update(entity);
        await SaveChangesAsync();
    }

    public virtual async Task RemoverAsync(Guid id)
    {
        DbSet.Remove(new TEntity { Id = id });
        await SaveChangesAsync();
    }

    public Task<int> SaveChangesAsync() =>
        Db.SaveChangesAsync();

    public void Dispose() => GC.SuppressFinalize(this);
}