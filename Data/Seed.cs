using System.Collections.Generic;
using hookup.API.Models;
using Newtonsoft.Json;

namespace hookup.API.Data
{
  public class Seed
  {
    private readonly DataContext _context;
    public Seed(DataContext context)
    {
      _context = context;
    }

    public void SeedUsers()
    {
      var userData = System.IO.File.ReadAllText("Data/userSeed.json");

      // serialize into objects
      var users = JsonConvert.DeserializeObject<List<User>>(userData);

      // loop across
      foreach (var user in users)
      {
        byte[] passwordHash, passwordSalt;
        CreatePasswordHash("password", out passwordHash, out passwordSalt);
        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;
        user.Username = user.Username.ToLower();

        // add user to the collection
        _context.Users.Add(user);
      }

      _context.SaveChanges();
    }

    /**
     * copied from the AuthRepository.completionlist to avoid making it public static
     * on the basis of this being a development file that wont make it to live.
     */
    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
      using (var hmac = new System.Security.Cryptography.HMACSHA512())
      {
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
      }
    }
  }
}