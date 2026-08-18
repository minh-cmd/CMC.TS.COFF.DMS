using CMC.TS.COFF.DMS.Biz.Model.Documents;
using CMC.TS.COFF.DMS.Data.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CMC.TS.COFF.DMS.Biz.IRepositories
{
    public interface IDocumentsRepository
    {
        Task<bool> Create(New documents);
        Task<List<Documents>?> GetAllDocuments();
        Task<Documents?> GetDocumentById(Guid id);
        Task<bool> Update(Guid id, Update updateDto);
        Task<bool> Delete(Guid id);
        IQueryable<Documents> DynamicFilter(Filter filter);
        Task<bool> AddTagsToDocument(Guid DocId, List<Guid> TagIds);
        Task<List<Documents>?> GetDocumentByTag(List<Guid> TagIds);
        Task<List<Tags>?> GetTagIdByDocumentId(Guid id);
        //tag on document operation
        /* Task<bool> SyncDocumentTag(Guid id, List<Guid>? TagIds);
         Task<DocumentDetailView?> GetDetailDocument(Guid id);*/
    }
}
