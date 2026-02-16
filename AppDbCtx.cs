using billing.Entities;
using Microsoft.EntityFrameworkCore;

namespace billing;

public class AppDbCtx(DbContextOptions<AppDbCtx> options) : DbContext(options)
{
    public DbSet<Region> Regions => Set<Region>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Org> Orgs => Set<Org>();
}
