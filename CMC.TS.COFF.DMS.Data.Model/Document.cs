using System;
using System.Collections.Generic;
using System.Text;

namespace CMC.TS.COFF.DMS.Data.Model
{
    internal class Document
    {
        internal Guid Id = Guid.Empty;
        internal string Title { get; set; } = string.Empty;
        internal string? Description { get; set; }
        public string ContentType { get; set; } = string.Empty;
        public string Extension { get; set; } = string.Empty;

        internal Document()
        {

        }

        internal Document(string title, string description, string contenttype, string extension)
        {
            Id = Guid.NewGuid();
            Title = title;
            Description = description;
            ContentType = contenttype;
            Extension = extension;
        }
    }
}
