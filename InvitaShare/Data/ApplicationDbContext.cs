using InvitaShare.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using InvitaShare.ViewModels;

namespace InvitaShare.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
         : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<WeddingEvent> WeddingEvents { get; set; }
        public DbSet<BaptismEvent> BaptismEvents { get; set; }
        public DbSet<EventUser> EventUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<ApplicationUser>()
            //    .HasMany(c => c.Events)
            //    .WithOne(e => e.ApplicationUser)
            //    .HasForeignKey(e => e.ApplicationUserId)
            //    .IsRequired();

            modelBuilder.Entity<EventUser>()
                .HasKey(bc => new { bc.EventId, bc.ApplicationUserId });
            modelBuilder.Entity<EventUser>()
                .HasOne(bc => bc.Event)
                .WithMany(b => b.EventUsers)
                .OnDelete(DeleteBehavior.NoAction)
                .HasForeignKey(bc => bc.EventId);

            modelBuilder.Entity<EventUser>()
                .HasOne(bc => bc.ApplicationUser)
                .WithMany(c => c.EventUsers)
                .OnDelete(DeleteBehavior.NoAction)
                .HasForeignKey(bc => bc.ApplicationUserId);
        }
        public DbSet<InvitaShare.ViewModels.EventUserDTO> GuestRegisteredDTO { get; set; } = default!;
        //public DbSet<InvitaShare.ViewModels.WeddingEventDTO> WeddingEventDTO { get; set; } = default!;
    }

}