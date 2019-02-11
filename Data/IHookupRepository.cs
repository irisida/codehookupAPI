using System.Collections.Generic;
using System.Threading.Tasks;
using hookup.API.Models;

namespace hookup.API.Data
{
  public interface IHookupRepository
  {
    void Add<T>(T entity) where T : class;
    void Delete<T>(T entity) where T : class;
    Task<bool> SaveAll();
    Task<IEnumerable<User>> GetUsers();
    Task<User> GetUser(int id);
  }
}