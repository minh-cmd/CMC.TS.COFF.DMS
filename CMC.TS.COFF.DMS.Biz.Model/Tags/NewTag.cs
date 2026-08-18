using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CMC.TS.COFF.DMS.Biz.Model.Tags
{
    public class NewTag
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(80)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Color is required")]
        [StringLength(15)]
        public string ColorHex { get; set; } = "#6C757D";
    }
}
