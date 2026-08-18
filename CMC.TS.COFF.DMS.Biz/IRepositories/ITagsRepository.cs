using CMC.TS.COFF.DMS.Biz.Model.Tags;
using CMC.TS.COFF.DMS.Data.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CMC.TS.COFF.DMS.Biz.IRepositories
{
    public interface ITagsRepository
    {
        Task<bool> CreateTag(NewTag tags);

        Task<List<Tags>?> GetAllTags();

        Task<Tags?> GetTagById(Guid id);

        Task<bool> DeleteTag(Guid id);

        Task<bool> UpdateTag(Guid id, NewTag tags);

        Task<List<Tags>?> FilterTag(FilterTag tags);
        Task<bool> AddDocumentsToTag(Guid TagId, List<Guid> DocIds);

    }
}
