using System;
using System.Collections.Generic;
using System.Text;

namespace CMC.TS.COFF.DMS.Biz.Model.Tags
{
    public class FilterTag
    {
        public string? Name { get; set; }
        public string? ColorHex { get; set; } 
        public DateTime? CreatedAtFrom { get; set; }
        public DateTime? CreatedAtTo { get; set; }

        public DateTime? UpdatedAtFrom { get; set; }
        public DateTime? UpdatedAtTo{ get; set; }
    }
}
