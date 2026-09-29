using ApplicationCQRSApplication.DTOs;
using ApplicationCQRSApplication.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace ApplicationCQRSApplication.Data
{
    public class GenericRepository<T> : IGenericRepository<T>
    where T : class
    {
        private readonly AppDbContext _context;  
        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<T>> GetPagedAsync(
            GridRequest request)
        {
            IQueryable<T> query = _context.Set<T>();

            // =========================
            // DYNAMIC SEARCH
            // =========================

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                query = ApplyDynamicSearch(
                    query,
                    request.Search.Trim());
            }

            // =========================
            // TOTAL RECORDS
            // =========================

            int totalRecords = await query.CountAsync();

            // =========================
            // DYNAMIC SORTING
            // =========================

            if (!string.IsNullOrWhiteSpace(request.SortColumn))
            {
                query = ApplySorting(
                    query,
                    request.SortColumn,
                    request.SortDirection);
            }

            // =========================
            // PAGINATION
            // =========================

            var items = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            return new PagedResult<T>
            {
                Items = items,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalRecords = totalRecords
            };
        }


        private static IQueryable<T> ApplyDynamicSearch(
            IQueryable<T> query,
            string search)
        {
            // x
            var parameter =
                Expression.Parameter(typeof(T), "x");

            Expression? finalExpression = null;

            // Get all public instance string properties
            var stringProperties = typeof(T)
                .GetProperties(BindingFlags.Public |
                               BindingFlags.Instance)
                .Where(p =>
                    p.PropertyType == typeof(string));

            foreach (var property in stringProperties)
            {
                // x.Name
                var propertyExpression =
                    Expression.Property(
                        parameter,
                        property);

                // Constant search value
                var searchExpression =
                    Expression.Constant(search);

                // x.Name.Contains(search)
                var containsExpression =
                    Expression.Call(
                        propertyExpression,
                        typeof(string).GetMethod(
                            nameof(string.Contains),
                            new[] { typeof(string) })!,
                        searchExpression);

                // Combine with OR
                if (finalExpression == null)
                {
                    finalExpression = containsExpression;
                }
                else
                {
                    finalExpression =
                        Expression.OrElse(
                            finalExpression,
                            containsExpression);
                }
            }

            // No string properties
            if (finalExpression == null)
            {
                return query;
            }

            // x => x.Name.Contains(search)
            //    || x.Email.Contains(search)
            //    || x.Department.Contains(search)

            var lambda =
                Expression.Lambda<Func<T, bool>>(
                    finalExpression,
                    parameter);

            return query.Where(lambda);
        }


        private static IQueryable<T> ApplySorting(
            IQueryable<T> query,
            string propertyName,
            string direction)
        {
            var property =
                typeof(T).GetProperty(
                    propertyName,
                    BindingFlags.Public |
                    BindingFlags.Instance |
                    BindingFlags.IgnoreCase);

            if (property == null)
            {
                return query;
            }

            var parameter =
                Expression.Parameter(typeof(T), "x");

            var propertyExpression =
                Expression.Property(
                    parameter,
                    property);

            var lambda =
                Expression.Lambda(
                    propertyExpression,
                    parameter);

            string methodName =
                direction.Equals(
                    "desc",
                    StringComparison.OrdinalIgnoreCase)
                    ? "OrderByDescending"
                    : "OrderBy";

            var expression =
                Expression.Call(
                    typeof(Queryable),
                    methodName,
                    new[]
                    {
                    typeof(T),
                    property.PropertyType
                    },
                    query.Expression,
                    Expression.Quote(lambda));

            return query.Provider
                .CreateQuery<T>(expression);
        }
    }
}
