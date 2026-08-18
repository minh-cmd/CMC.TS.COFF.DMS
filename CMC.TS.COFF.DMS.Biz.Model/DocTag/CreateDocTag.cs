using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CMC.TS.COFF.DMS.Biz.Model.DocTag
{
    public class CreateDocTag
    {
        [Required]
        public Guid DocumentId { get; set; }
        [Required]
        public Guid TagId { get; set; }
    }
}
