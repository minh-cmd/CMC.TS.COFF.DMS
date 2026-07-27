using CMC.TS.COFF.DMS.Data.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CMC.TS.COFF.DMS.Biz.IRepositories
{
    public interface IDocumentsRepository
    {
        Task<bool> Create(Documents documents);
    }
}
