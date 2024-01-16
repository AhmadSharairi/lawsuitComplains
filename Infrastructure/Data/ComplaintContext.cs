using Core.Entities;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Data
{
    public class ComplaintContext : DbContext
    {

        public ComplaintContext(DbContextOptions<ComplaintContext> options) : base(options) { }


        /*Fluent API*/
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //one to many
            modelBuilder.Entity<User>()
           .HasMany(u => u.Complaints)
           .WithOne(c => c.User)
          .HasForeignKey(c => c.UserId);


            //2-This complaint has a multiple demands (one to many relationship).
            modelBuilder.Entity<Complaint>()
                .HasMany(c => c.Demands)
                .WithOne(d => d.Complaint)
                .HasForeignKey(d => d.ComplaintId);

            //one to many User with AttachmentPdf
            modelBuilder.Entity<Attachment>()
                    .HasOne(a => a.User)
                    .WithMany(u => u.Attachments)
                    .HasForeignKey(a => a.UserId);


     



        }
        public DbSet<User> Users { get; set; }
        public DbSet<Complaint> Complaints { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<Demand> Demands { get; set; }
        public DbSet<UserAuthentication> UsersAuthentication { get; set; }


    }

}