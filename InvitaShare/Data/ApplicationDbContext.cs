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
        public DbSet<WeddingEvent> WeddingEvents { get; set;}
        public DbSet<BaptismEvent> BaptismEvents { get; set;}
        public DbSet<Invitation> Invitations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ////modelBuilder.Entity<ApplicationUser>()
            ////    .HasMany(c => c.Events)
            ////    .WithOne(e => e.ApplicationUser)
            ////    .HasForeignKey(e => e.ApplicationUserId)
            ////    .IsRequired();

            //modelBuilder.Entity<GuestEvent>()
            //    .HasKey(bc => new { bc.EventId, bc.GuestId });
            //modelBuilder.Entity<GuestEvent>()
            //    .HasOne(bc => bc.Book)
            //    .WithMany(b => b.BookCategories)
            //    .HasForeignKey(bc => bc.BookId);
            //modelBuilder.Entity<BookCategory>()
            //    .HasOne(bc => bc.Category)
            //    .WithMany(c => c.BookCategories)
            //    .HasForeignKey(bc => bc.CategoryId);
        }
        public DbSet<InvitaShare.ViewModels.WeddingEventDTO> WeddingEventDTO { get; set; } = default!;
    }
    
}