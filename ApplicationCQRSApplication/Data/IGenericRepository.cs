using ApplicationCQRSApplication.DTOs;
using System.Linq.Expressions;

namespace ApplicationCQRSApplication.Data
{
    public interface IGenericRepository<T> where T : class
    {
        Task<PagedResult<T>> GetPagedAsync(
        GridRequest request);
    }
}
