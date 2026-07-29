using System;
using System.Collections.Generic;
using System.Text;

namespace CMC.TS.COFF.DMS.Data.Model
{
    public class Documents
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string ContentType { get; set; } = string.Empty;
        public string Extension { get; set; } = string.Empty;

        // File Storage Details
        public long FileSize { get; set; } = 0;
        public string StoragePath { get; set; } = string.Empty; 

        // Relational Keys (FKs based on your design choice)
        public Guid? CategoryId { get; set; }                   // Category Foreign Key

        // Audit & System Metadata
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public Guid? UpdatedBy { get; set; }                    // User ID who uploaded
        public Guid? CreatedBy { get; set; }                    // User ID who uploaded
        public bool IsDeleted { get; set; } = false;            // Soft-delete flag
        public Documents()
        {

        }

        public Documents(string title, string? description, string contenttype, string extension, long fileSize,string storagePath)
        {
            Title = title;
            Description = description;
            ContentType = contenttype;
            Extension = extension;
            FileSize = fileSize;
            StoragePath = storagePath;
        }
    }
}
