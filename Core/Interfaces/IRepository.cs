using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IRepository<T>
    {
        Task InsertUpdate(ICollection<T> model, string userId);
        Task<ICollection<T>> GetList(long id, string userId);
        Task Delete(long id, string userId);
    }
}
