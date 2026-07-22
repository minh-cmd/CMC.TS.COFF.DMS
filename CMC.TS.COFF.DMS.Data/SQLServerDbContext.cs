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
        DbSet<Documents> documents;
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
            });
        }
    }
}
