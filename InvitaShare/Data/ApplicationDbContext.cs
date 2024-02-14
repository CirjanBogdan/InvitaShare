using InvitaShare.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InvitaShare.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<WeddingEvent> WeddingEvents { get; set;}
        public DbSet<BaptismEvent> BaptismEvents { get; set;}
        public DbSet<Invitation> Invitations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<Event>()
            //    .HasMany(e => e.Guests)
            //    .WithOne(e => e.Event)
            //    .HasForeignKey(e => e.EventId)
            //    .IsRequired();

            
        }
    }
    
}