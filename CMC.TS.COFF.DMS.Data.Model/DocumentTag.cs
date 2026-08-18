using System;
using System.Collections.Generic;
using System.Text;

namespace CMC.TS.COFF.DMS.Data.Model
{
    public class DocumentTag
    {
        public Guid DocumentId { get; set; }
        public Guid TagId { get; set; }

        // Tracking Metadata
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Guid? CreatedBy { get; set; }

        public DocumentTag(Guid documentId, Guid tagId, DateTime createdAt, Guid? createdBy)
        {
            DocumentId = documentId;
            TagId = tagId;
            CreatedBy = createdBy;
        }

        public DocumentTag()
        {

        }
    }
}
