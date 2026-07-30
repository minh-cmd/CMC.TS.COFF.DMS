using System;
using System.Collections.Generic;
using System.Text;

namespace CMC.TS.COFF.DMS.Data.Model
{
    public class Categories
    {
        public Guid Id { get; init; } = Guid.NewGuid();

        // Core Classification
        public string Name { get; set; } = string.Empty;          // e.g., "Financial Invoices"
        public string Code { get; set; } = string.Empty;          // e.g., "FIN-INV" (helps in file numbering & indexing)
        public string? Description { get; set; }

        // Audit Metadata
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Guid? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid? UpdatedBy { get; set; }
        public bool IsDeleted { get; set; } = false;

        public Categories() { }

        public Categories(string name, string code, string? description)
        {
            Name = name;
            Code = code;
            Description = description;
        }
    }
}
