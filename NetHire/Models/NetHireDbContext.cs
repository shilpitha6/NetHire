using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace NetHire.Models
{
    public class NetHireDbContext : IdentityDbContext<ApplicationUser>
    {
        public NetHireDbContext(DbContextOptions<NetHireDbContext> options)
            : base(options)
        {
        }

        //public DbSet<User> Users { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<Education> Education { get; set; }
        public DbSet<PreviousEmployment> PreviousEmployments { get; set; }
        public DbSet<EmergencyContact> EmergencyContacts { get; set; }
        public DbSet<ProfessionalReference> ProfessionalReferences { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<ApplicantContactInformation> ApplicantContactInformation { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure one-to-one relationship between User and Company
            

            // Configure one-to-many relationship between Company and Jobs
            modelBuilder.Entity<Job>()
                .HasOne(j => j.Company)
                .WithMany()
                .HasForeignKey(j => j.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure Application relationships
            

            modelBuilder.Entity<Application>()
                .HasOne(a => a.Job)
                .WithMany()
                .HasForeignKey(a => a.JobId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ApplicantContactInformation>(entity =>
            {
                entity.Property(a => a.Phone).HasMaxLength(15);
                entity.Property(a => a.AltPhone).HasMaxLength(15);
                entity.Property(a => a.Email).HasMaxLength(255);
                entity.Property(a => a.AltEmail).HasMaxLength(255);
                entity.Property(a => a.StreetAddress).HasMaxLength(255);
                entity.Property(a => a.Address2).HasMaxLength(255);
                entity.Property(a => a.City).HasMaxLength(100);
                entity.Property(a => a.State).HasMaxLength(100);
                entity.Property(a => a.ZipCode).HasMaxLength(20);
                entity.HasKey(a => a.ContactInfoId);
                entity.HasOne<ApplicationUser>()
                    .WithOne()
                    .HasForeignKey<ApplicantContactInformation>(a => a.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}