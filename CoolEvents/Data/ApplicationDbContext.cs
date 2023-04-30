using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CoolEvents.Models;

namespace CoolEvents.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<UserEvent> UserEvents { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserEvent>().HasKey(bc => new { bc.UserId, bc.EventId });
            modelBuilder.Entity<UserEvent>()
                .HasOne(bc => bc.User)
                .WithMany(b => b.UserEvents)
                .HasForeignKey(bc => bc.UserId);
            modelBuilder.Entity<UserEvent>()
                .HasOne(bc => bc.Event)
                .WithMany(c => c.UserEvents)
                .HasForeignKey(bc => bc.EventId);

        }
        public DbSet<CoolEvents.Models.EditEventModel> EditEventModel { get; set; } = default!;
    }
}