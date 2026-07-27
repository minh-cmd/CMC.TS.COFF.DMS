using System;
using System.Collections.Generic;
using System.Text;

namespace CMC.TS.COFF.DMS.Biz.IServices
{
    public interface IDocumentsService
    {
        Task<bool> NewDocument(CMC.TS.COFF.DMS.Biz.Model.Documents.New news);
    }
}
