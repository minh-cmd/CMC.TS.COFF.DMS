using System;
using System.Collections.Generic;
using System.Text;

namespace CMC.TS.COFF.DMS.Data.Model
{
    public class Tags
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string ColorHex { get; set; } = "#6C757D";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public Guid? UpdatedBy { get; set; }
        public Guid? CreatedBy { get; set; }
        public bool IsDeleted { get; set; } = false;

        public Tags()
        {

        }

        public Tags(string name, string color)
        {
            Name = name;
            ColorHex = color;
        }
    }
}
