using Eventure.Models;
using Microsoft.EntityFrameworkCore;

namespace Eventure.Data;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) {}

    public DbSet<User> Users => Set<User>();
    public DbSet<EventComponent> Events => Set<EventComponent>();
    public DbSet<TaskItem> Tasks => Set<TaskItem>();
    public DbSet<TaskState> TaskStatuses { get; set; }
    public DbSet<EventInvitation> EventInvitations { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<User>().Property(p => p.Username).HasMaxLength(64).IsRequired();
        modelBuilder.Entity<User>().Property(p => p.Email).HasMaxLength(256).IsRequired();
        modelBuilder.Entity<User>().Property(p => p.PasswordHash).IsRequired();

        modelBuilder.Entity<EventComponent>().Property(p => p.Title).HasMaxLength(100).IsRequired();
        modelBuilder.Entity<EventComponent>().Property(p => p.Description).HasMaxLength(500);

        modelBuilder.Entity<EventInvitation>()
            .HasKey(x => new { x.EventId, x.UserId });

        modelBuilder.Entity<EventInvitation>()
            .HasOne(x => x.Event)
            .WithMany()
            .HasForeignKey(x => x.EventId);

        modelBuilder.Entity<EventInvitation>()
            .HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId);

    }
}
                