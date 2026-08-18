using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CMC.TS.COFF.DMS.Biz.Model.Documents
{
    public class New
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(255)]
        public string Title { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        [Required]
        [StringLength(100)]
        public string ContentType { get; set; } = string.Empty;

        [Required]
        [StringLength(10)]
        public string Extension { get; set; } = string.Empty;

        public List<Guid>? TagIds { get; set; }
    }
}
