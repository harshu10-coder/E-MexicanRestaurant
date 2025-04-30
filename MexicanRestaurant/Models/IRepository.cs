using System.Linq.Expressions;
namespace MexicanRestaurant.Models
{
    public interface IRepository <T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id, QueryOptions<T> options);

        Task AddAsync(T Entity);

        Task UpdateAsync(T Entity);

        Task DeleteAsync(int id);
    }
}
