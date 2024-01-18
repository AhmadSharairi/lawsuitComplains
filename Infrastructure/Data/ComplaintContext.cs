using Core.Entities;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Data
{
    public class ComplaintContext : DbContext
    {

        public ComplaintContext(DbContextOptions<ComplaintContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Complaint> Complaints { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<Demand> Demands { get; set; }
        public DbSet<UserAuthentication> UsersAuthentication { get; set; }

        public DbSet<LocalizedText> LocalizedTexts { get; set; }



        // Entity framework as the ORM (must be code first).
        /*Fluent API*/
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // one-to-many relationship between User and Complaint
            modelBuilder.Entity<User>()
                .HasMany(u => u.Complaints)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            //  one-to-many relationship between Complaint and Demand
            modelBuilder.Entity<Complaint>()
                .HasMany(c => c.Demands)
                .WithOne(d => d.Complaint)
                .HasForeignKey(d => d.ComplaintId)
                .OnDelete(DeleteBehavior.Restrict);

            //  one-to-many relationship between Attachment and User
            modelBuilder.Entity<Attachment>()
                .HasOne(a => a.User)
                .WithMany(u => u.Attachments)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            //  one-to-many relationship between Complaint and LocalizedText
            modelBuilder.Entity<Complaint>()
                .HasMany(c => c.ComplaintTexts)
                .WithOne(lt => lt.Complaint)
                .HasForeignKey(lt => lt.ComplaintId)
                .OnDelete(DeleteBehavior.Restrict);

            //  one-to-many relationship between Demand and LocalizedText
            modelBuilder.Entity<Demand>()
                .HasMany(d => d.DemandDescriptions)
                .WithOne(lt => lt.Demand)
                .HasForeignKey(lt => lt.DemandId)
                .OnDelete(DeleteBehavior.Restrict);



     modelBuilder.Entity<Complaint>()
        .HasMany(c => c.ComplaintTexts)
        .WithOne(lt => lt.Complaint)
        .HasForeignKey(lt => lt.ComplaintId)
        .OnDelete(DeleteBehavior.Restrict);

    // Configure one-to-many Demand and LocalizedText
    modelBuilder.Entity<Demand>()
        .HasMany(d => d.DemandDescriptions)
        .WithOne(lt => lt.Demand)
        .HasForeignKey(lt => lt.DemandId)
        .OnDelete(DeleteBehavior.Restrict);

    // Configure one-to-many  LocalizedText and Complaint
    modelBuilder.Entity<LocalizedText>()
        .HasOne(lt => lt.Complaint)
        .WithMany(c => c.ComplaintTexts)
        .HasForeignKey(lt => lt.ComplaintId)
        .OnDelete(DeleteBehavior.Restrict);

    // Configure one-to-many LocalizedText and Demand
    modelBuilder.Entity<LocalizedText>()
        .HasOne(lt => lt.Demand)
        .WithMany(d => d.DemandDescriptions)
        .HasForeignKey(lt => lt.DemandId)
        .OnDelete(DeleteBehavior.Restrict);
        }


    }

}