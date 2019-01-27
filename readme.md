# Matching App API

The purpose is to demonstrate a walking skeleton (or better) for a matching/dating application. The backend approach will be DB->ORM-API.

## More info

The construct technical detail at a high level is:

    DB  -> SQLite
    ORM -> Entity Framework
    API -> asp.net

### Settings
Some development settings remain in place

    startup.cs has commented UseHsts and UseHttpsRedirection for now.
    Properties/launchSettings.json remains in development mode for now
    and uses a single port ref point "applicationUrl": "http://localhost:5000",

The dataContext is resolved by:
```
using matcher.API.Models;
using Microsoft.EntityFrameworkCore;

namespace matcher.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Value> Values { get; set; }
    }
}
```

### Setting up a conectionString

To setup a connection string (for a trivial SQLite DB) we can add to the appsettings.json file and provide a ConnectionStrings block. This will point to a data source directly in an Sqlite setting. To enable and use the new connection string we must amend the startup.cs services to use the injected Configuration and with the GetConnectionString pass in our newly created DefaultConnection string.
```
services.AddDbContext<DataContext>(x => x.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
```