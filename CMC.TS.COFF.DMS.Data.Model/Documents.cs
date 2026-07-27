using System;
using System.Collections.Generic;
using System.Text;

namespace CMC.TS.COFF.DMS.Data.Model
{
    public class Documents
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string ContentType { get; set; } = string.Empty;
        public string Extension { get; set; } = string.Empty;

        public Documents()
        {

        }

        public Documents(string title, string? description, string contenttype, string extension)
        {
            Title = title;
            Description = description;
            ContentType = contenttype;
            Extension = extension;
        }
    }
}
