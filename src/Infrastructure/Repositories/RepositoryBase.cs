using ClinicSystem.Application.Interfaces;
using ClinicSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace ClinicSystem.Infrastructure.Repositories;

public class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public RepositoryBase(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);
    public void Update(T entity) => _dbSet.Update(entity);
    public void Remove(T entity) => _dbSet.Remove(entity);
    public void RemoveRange(IEnumerable<T> entities) => _dbSet.RemoveRange(entities);

    public async Task<int> SaveAsync(CancellationToken ct = default)
        => await _context.SaveChangesAsync(ct);

    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        => await _dbSet.AnyAsync(predicate);

    public async Task<T?> GetAsync(
        Expression<Func<T, bool>> filter,
        string? includeProperties = null,
        bool tracking = true)
    {
        IQueryable<T> query = _dbSet.Where(filter);
        if (!tracking) query = query.AsNoTracking();
        query = ApplyIncludes(query, includeProperties);
        return await query.FirstOrDefaultAsync();
    }

    public async Task<(List<T> Items, int TotalCount)> GetAllAsync(
        Expression<Func<T, bool>>? filter = null,
        int? pageNumber = null,
        int? pageSize = null,
        string? orderBy = null,
        bool ascending = true,
        string? includeProperties = null,
        bool tracking = true)
    {
        IQueryable<T> query = _dbSet;
        if (!tracking) query = query.AsNoTracking();
        if (filter != null) query = query.Where(filter);
        query = ApplyIncludes(query, includeProperties);
        int total = await query.CountAsync();
        if (!string.IsNullOrWhiteSpace(orderBy))
            query = ApplyOrdering(query, orderBy, ascending);
        if (pageNumber.HasValue && pageSize.HasValue)
            query = query.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value);
        return (await query.ToListAsync(), total);
    }

    private static IQueryable<T> ApplyIncludes(IQueryable<T> query, string? props)
    {
        if (string.IsNullOrWhiteSpace(props)) return query;
        foreach (var p in props.Split(',', StringSplitOptions.RemoveEmptyEntries))
            query = query.Include(p.Trim());
        return query;
    }

    private static IQueryable<T> ApplyOrdering(IQueryable<T> query, string orderBy, bool ascending)
    {
        var prop = typeof(T).GetProperty(orderBy,
            BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        if (prop == null) return query;
        var param = Expression.Parameter(typeof(T), "x");
        var body = Expression.MakeMemberAccess(param, prop);
        var lambda = Expression.Lambda(body, param);
        var method = ascending ? "OrderBy" : "OrderByDescending";
        var expr = Expression.Call(
            typeof(Queryable), method,
            new[] { typeof(T), prop.PropertyType },
            query.Expression, Expression.Quote(lambda));
        return query.Provider.CreateQuery<T>(expr);
    }
}
