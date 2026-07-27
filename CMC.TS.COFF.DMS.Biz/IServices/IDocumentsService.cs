using System;
using System.Collections.Generic;
using System.Text;
using CMC.TS.COFF.DMS.Biz.Model.Documents;

namespace CMC.TS.COFF.DMS.Biz.IServices
{
    public interface IDocumentsService
    {
        Task<bool> NewDocument(New news);
    }
}
