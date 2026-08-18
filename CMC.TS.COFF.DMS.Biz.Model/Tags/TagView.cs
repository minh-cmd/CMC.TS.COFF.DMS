using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CMC.TS.COFF.DMS.Biz.Model.Tags
{
    public class TagView
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string ColorHex { get; set; }
    }
}
