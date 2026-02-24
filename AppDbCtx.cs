using billing.Entities;
using Microsoft.EntityFrameworkCore;

namespace billing;

public class AppDbCtx(DbContextOptions<AppDbCtx> options) : DbContext(options)
{
    public DbSet<Region> Regions => Set<Region>();
    public DbSet<District> Districts => Set<District>();
    public DbSet<OrgType> OrgTypes => Set<OrgType>();
    public DbSet<Org> Orgs => Set<Org>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<ClientType> ClientTypes => Set<ClientType>();
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<User> Users => Set<User>();
    public DbSet<UserRole> UserRole => Set<UserRole>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<Charge> Charges => Set<Charge>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>()
            .Property(r => r.Perms)
            .HasColumnType("jsonb");

        modelBuilder.Entity<UserRole>()
            .HasKey(ur => new { ur.OrgId, ur.UserId, ur.RoleId });
    }
}
