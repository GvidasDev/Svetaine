using Eventure.Models;
using Microsoft.EntityFrameworkCore;

namespace Eventure.Data;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {

    }

    public DbSet<EventComponent> Events => Set<EventComponent>();
}