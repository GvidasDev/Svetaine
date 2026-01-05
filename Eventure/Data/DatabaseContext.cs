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
        
            modelBuilder.Entity<TaskState>().HasData(
            new TaskState { Id = 1, Name = "TODO" },
            new TaskState { Id = 2, Name = "IN PROGRESS" },
            new TaskState { Id = 3, Name = "DONE" }
            );
        }
    }