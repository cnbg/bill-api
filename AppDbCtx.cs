using billing.Entities;
using Microsoft.EntityFrameworkCore;

namespace billing;

public class AppDbCtx(DbContextOptions<AppDbCtx> options) : DbContext(options)
{
    public DbSet<Region> Regions => Set<Region>();
    public DbSet<Org> Orgs => Set<Org>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Perm> Perms => Set<Perm>();
    public DbSet<RolePerm> RolePerm => Set<RolePerm>();
    public DbSet<User> Users => Set<User>();
    public DbSet<UserRole> UserRole => Set<UserRole>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RolePerm>()
            .HasKey(rp => new { rp.RoleId, rp.PermId });

        modelBuilder.Entity<UserRole>()
            .HasKey(ur => new { ur.OrgId, ur.UserId, ur.RoleId });
    }
}
