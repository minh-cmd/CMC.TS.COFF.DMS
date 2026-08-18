using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CMC.TS.COFF.DMS.Biz.Model.DocTag
{
    public class AddManyTagsToDocument
    {
        [Required]
        public Guid DocumentId { get; set; }
        public List<Guid>? TagIdList { get; set; }
    }
}
