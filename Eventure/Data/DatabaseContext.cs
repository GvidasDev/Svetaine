using Eventure.Models;
using Microsoft.EntityFrameworkCore;

namespace Eventure.Data;

public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) {}

        public DbSet<User> Users => Set<User>();
        public DbSet<EventComponent> Events => Set<EventComponent>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Jei nori: galima pridėti apribojimus laukų ilgiui
            modelBuilder.Entity<User>().Property(p => p.Username).HasMaxLength(64).IsRequired();
            modelBuilder.Entity<User>().Property(p => p.Email).HasMaxLength(256).IsRequired();
            modelBuilder.Entity<User>().Property(p => p.PasswordHash).IsRequired();

            modelBuilder.Entity<EventComponent>().Property(p => p.Title).HasMaxLength(100).IsRequired();
            modelBuilder.Entity<EventComponent>().Property(p => p.Description).HasMaxLength(500);
        }
    }