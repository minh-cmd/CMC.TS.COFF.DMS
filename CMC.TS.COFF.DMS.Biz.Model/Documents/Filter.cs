using System;
using System.Collections.Generic;
using System.Text;

namespace CMC.TS.COFF.DMS.Biz.Model.Documents
{
    public class Filter
    {
        public string? Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? ContentType { get; set; } = string.Empty;
        public string? Extension { get; set; } = string.Empty;
        public long? FileSize { get; set; } = 0;
        public string? StoragePath { get; set; } = string.Empty;
        public List<Guid>? TagIds { get; set; }
    }
}
