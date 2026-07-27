using CMC.TS.COFF.DMS.Biz.IRepositories;
using CMC.TS.COFF.DMS.Data;
using CMC.TS.COFF.DMS.Data.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CMC.TS.COFF.DMS.Biz.Repositories
{
    public class DocumentRepository : IDocumentsRepository
    {
        private readonly SQLServerDbContext _context;

        public DocumentRepository(SQLServerDbContext context)
        {
            _context = context;
        }
        public async Task<bool> Create(Documents document)
        {
            _context.documents.Add(document);
            return await _context.SaveChangesAsync() > 0;
        }

    }
}
