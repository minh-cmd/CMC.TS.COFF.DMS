using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using CMC.TS.COFF.DMS.Biz.Model;
using CMC.TS.COFF.DMS.Biz.Model.Tags;

namespace CMC.TS.COFF.DMS.Biz.Model.Documents
{
    public class DocumentDetailView
    {
        [Required]
        public Guid Id { get; init; }
        [Required]
        public string Title { get; set; }

        public string? Description { get; set; }
        [Required]
        public string ContentType { get; set; }
        [Required]
        public string Extension { get; set; }

        public List<TagView>? Tags { get; set; }
    }
}
