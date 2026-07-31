using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CMC.TS.COFF.DMS.Biz.Model.Categories
{
    public class NewCategory
    {
        [Required(ErrorMessage = "Categories name can't be empty")]
        public string Name { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Categories code can't be empty")]
        public string Code { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
