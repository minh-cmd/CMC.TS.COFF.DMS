using CMC.TS.COFF.DMS.Data.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace CMC.TS.COFF.DMS.Data
{
    public class SQLServerDbContext : DbContext
    {
        public DbSet<Documents> documents {  get; set; }
        public SQLServerDbContext(DbContextOptions<SQLServerDbContext> options) : base(options) 
        {
        
        }

        public SQLServerDbContext()
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Documents>(entity =>
            {
                entity.ToTable("Documents");

                entity.HasKey(d => d.Id);

                // Title: Required with max length
                entity.Property(d => d.Title)
                      .IsRequired()
                      .HasMaxLength(255);

                // Description: Optional
                entity.Property(d => d.Description)
                      .HasMaxLength(1000);

                entity.Property(d => d.ContentType)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(d => d.Extension)
                      .IsRequired()
                      .HasMaxLength(10);

                // File Storage Details
                entity.Property(d => d.FileSize)
                      .IsRequired(); // bigint in SQL Server

                entity.Property(d => d.StoragePath)
                      .IsRequired()
                      .HasMaxLength(1000); // Prevents nvarchar(max) for better performance

                // Relational Keys
                entity.Property(d => d.CategoryId)
                      .IsRequired(false); // Nullable FK column

                // Audit & System Metadata
                entity.Property(d => d.CreatedAt)
                      .IsRequired();

                entity.Property(d => d.UpdatedAt)
                      .IsRequired(false);

                entity.Property(d => d.CreatedBy)
                      .IsRequired(false);

                entity.Property(d => d.IsDeleted)
                      .IsRequired()
                      .HasDefaultValue(false); // SQL Server defaults bit flag to 0
            });

            modelBuilder.Entity<Categories>(entity =>
            {
                entity.ToTable("Categories");

                entity.HasKey(d => d.Id);

                // Title: Required with max length
                entity.Property(d => d.Name)
                      .IsRequired()
                      .HasMaxLength(255);

                entity.Property(d => d.Code)
                      .IsRequired()
                      .HasMaxLength(10);

                entity.HasIndex(d=> d.Code)
                        .IsUnique();

                // Description: Optional
                entity.Property(d => d.Description)
                      .HasMaxLength(1000);

                // Audit & System Metadata
                entity.Property(d => d.CreatedAt)
                      .IsRequired();

                entity.Property(d => d.UpdatedAt)
                      .IsRequired(false);

                entity.Property(d => d.CreatedBy)
                      .IsRequired(false);

                entity.Property(d => d.IsDeleted)
                      .IsRequired()
                      .HasDefaultValue(false); // SQL Server defaults bit flag to 0
            });
        }
    }
}
