using System.Collections.Generic;
using System.Threading.Tasks;
using hookup.API.Models;
using Microsoft.EntityFrameworkCore;

namespace hookup.API.Data
{
  public class HookupRepository : IHookupRepository
  {
    private readonly DataContext _context;
    public HookupRepository(DataContext context)
    {
      _context = context;

    }

    public void Add<T>(T entity) where T : class
    {
      _context.Add(entity);
    }

    public void Delete<T>(T entity) where T : class
    {
      _context.Remove(entity);
    }

    public async Task<User> GetUser(int id)
    {
      var user = await _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(u => u.Id == id);

      return user;
    }

    public async Task<IEnumerable<User>> GetUsers()
    {
      var users = await _context.Users.Include(p => p.Photos).ToListAsync();

      return users;
    }

    public async Task<bool> SaveAll()
    {
      /**
       * to return the bool we measure against the count
       * being returned. If greater than zero we have
       * saved some results and the result is true. if
       * the result is zero it is false and we haven't
       * saved any changes.
       */
      return await _context.SaveChangesAsync() > 0;
    }
  }
}