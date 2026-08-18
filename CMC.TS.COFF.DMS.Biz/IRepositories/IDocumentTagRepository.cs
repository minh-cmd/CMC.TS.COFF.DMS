using CMC.TS.COFF.DMS.Biz.Model.DocTag;
using CMC.TS.COFF.DMS.Data.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CMC.TS.COFF.DMS.Biz.IRepositories
{
    public interface IDocumentTagRepository
    {
        Task<bool> CreateDocumentTag(CreateDocTag create);
        Task<List<DocumentTag>?> GetAllDocumentTag();
        Task<DocumentTag?> GetDocumentTagById(Guid docId, Guid tagId);
        Task<bool> DeleteDocsTag(Guid docId, Guid tagId);
        Task<bool> UpdateDocsTag(Guid docId, Guid tagId, UpdateDocTag updateDocTag);
        
        //in use:
        Task<bool> AddTagsToDocument(Guid DocId, List<Guid> TagIds);
        Task<bool> AddDocumentsToTag(Guid TagId, List<Guid> DocIds);
        Task<List<Guid>?> FetchDocumentIdByTagId(List<Guid> TagIds);
        Task<List<Guid>?> FetchTagIdByDocumentId(Guid? DocId);
        Task<bool> UpdateTagIdOfDocument(Guid DocId, List<Guid>? TagIdsComming);

    }
}
