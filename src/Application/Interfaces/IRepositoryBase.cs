using System.Linq.Expressions;

namespace ClinicSystem.Application.Interfaces;

public interface IRepositoryBase<T> where T : class
{
    Task AddAsync(T entity);
    void Update(T entity);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);
    Task<int> SaveAsync(CancellationToken ct = default);
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
    Task<T?> GetAsync(
        Expression<Func<T, bool>> filter,
        string? includeProperties = null,
        bool tracking = true);
    Task<(List<T> Items, int TotalCount)> GetAllAsync(
        Expression<Func<T, bool>>? filter = null,
        int? pageNumber = null,
        int? pageSize = null,
        string? orderBy = null,
        bool ascending = true,
        string? includeProperties = null,
        bool tracking = true);
}
