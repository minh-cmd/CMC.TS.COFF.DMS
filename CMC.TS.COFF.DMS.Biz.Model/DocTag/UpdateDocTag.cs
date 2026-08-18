using System;
using System.Collections.Generic;
using System.Text;

namespace CMC.TS.COFF.DMS.Biz.Model.DocTag
{
    public class UpdateDocTag
    {
        public Guid DocId { get; set; }
        public Guid TagId { get; set; }
        public DateTime? CreateAt { get; set; }

    }
}
