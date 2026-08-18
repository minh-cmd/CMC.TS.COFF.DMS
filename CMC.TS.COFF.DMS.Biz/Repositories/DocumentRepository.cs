using CMC.TS.COFF.DMS.Biz.IRepositories;
using CMC.TS.COFF.DMS.Biz.Model.DocTag;
using CMC.TS.COFF.DMS.Biz.Model.Documents;
using CMC.TS.COFF.DMS.Biz.Model.Tags;
using CMC.TS.COFF.DMS.Data;
using CMC.TS.COFF.DMS.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks.Dataflow;

namespace CMC.TS.COFF.DMS.Biz.Repositories
{
    public class DocumentRepository : IDocumentsRepository
    {
        private readonly SQLServerDbContext _context;
        private readonly IDocumentTagRepository _documentTagRepository;
        private readonly ILogger<DocumentRepository> _logger;

        public DocumentRepository(SQLServerDbContext context, ILogger<DocumentRepository> logger, IDocumentTagRepository documentTagRepository)
        {
            _context = context;
            _logger = logger;
            _documentTagRepository = documentTagRepository;
        }
        public async Task<bool> Create(New news)
        {
            try
            {
                _logger.LogInformation($"start create new documents {news.Title}");
                _context.documents.Add(news.DocumentsToNew());
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "create new documents failed");
                return false;
            }

        }
        public async Task<List<Documents>?> GetAllDocuments()
        {
            try
            {
                _logger.LogInformation("start to fetch Documents");
                List<Documents> documents = await _context.documents.Where(u=> u.IsDeleted == false).ToListAsync();
                _logger.LogInformation("Successfully fetched {Count} documents.", documents.Count);
                return documents;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "failed to fetch Documents");
                return null;
            }
        }
        public async Task<Documents?> GetDocumentById(Guid id)
        {
            try
            {
                _logger.LogInformation("fetch document by ID start");
                var doc = await _context.documents.FirstOrDefaultAsync(docs => docs.Id == id && docs.IsDeleted == false);
                _logger.LogInformation($"Successfully fetched {doc.Id} documents.");
                return doc;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "fetch document by ID failed");
                return null;
            }
        }
        public async Task<bool> Update(Guid id, Update updateDto)
        {
            try
            {
                _logger.LogInformation("update document start");
                Documents? a = await GetDocumentById(id);

                await Task.Delay(100); // testing race condition

                if (a != null)
                {
                    _logger.LogInformation("founded document");
                    updateDto.DocumentsToUpdate(a);
                    return await _context.SaveChangesAsync() > 0;
                }
                _logger.LogInformation("can't find document");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "update document failed");
                return false;
            }
        }
        public async Task<bool> Delete(Guid id)
        {
            try
            {
                _logger.LogInformation($"delete start {id}");
                Documents? docs = await GetDocumentById(id);
                if(docs == null)
                {
                    _logger.LogInformation($"can't find {id}");
                    return false;
                }
                docs.IsDeleted = true;
                _logger.LogInformation($"Delete {docs.Id} document successfully");
                return await _context.SaveChangesAsync()>0 ;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "delete failed");
                return false;
            }
        }
        public IQueryable<Documents> DynamicFilter(Filter filter)
        {
            IQueryable<Documents> queryable = _context.documents.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(filter.Title))
            {
                string searchterm = filter.Title;   
                queryable = queryable.Where(a=>a.Title.ToLower().Contains(filter.Title) || a.Description.ToLower().Contains(filter.Title));
            }

            if (!string.IsNullOrWhiteSpace(filter.Extension))
            {
                filter.Extension.Trim().ToLower();
                queryable = queryable.Where(a=>a.Extension.ToLower() == filter.Extension);
            }

            if (!string.IsNullOrWhiteSpace(filter.ContentType))
            {
                filter.ContentType.Trim().ToLower();
                queryable = queryable.Where(a => a.ContentType.ToLower() == filter.ContentType);
            }

            if (filter.FileSize != 0)
            {
                queryable = queryable.Where(a => a.FileSize > filter.FileSize);
            }

            //filter documents theo các tag 
            if (filter.TagIds != null && filter.TagIds.Count>0)
            {
                queryable = from dt in _context.docsTags
                            join docs in queryable on dt.DocumentId equals docs.Id
                            where filter.TagIds.Contains(dt.TagId)
                            group docs by docs into g 
                            //không hiểu group by hoạt động cụ thể như thế nào.
                            //Dùng cái nào trước dùng cái nào sau?
                            //Tại sao lại dùng group docs thay vì group dt?
                            //Tại sao lại dùng by docs thay vì by dt?

/*                          có vẻ dùng "group docs" hay "group dt" là như nhau. 
 *                          Kể cả dùng "group dt by dt into g" vẫn không sao. 
 *                          Vì nó lấy từ 2 bảng join với nhau
*/                          where g.Count() == filter.TagIds.Count()
                            select g.Key;
            }
            return queryable;
        }
        public async Task<bool> AddTagsToDocument(Guid DocId, List<Guid> TagIds)
        {
            try
            {
                _logger.LogInformation("Add many tag to document operation");
                bool isSuccess = await _documentTagRepository.AddTagsToDocument(DocId, TagIds);
                if (isSuccess) 
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Add many tag to document operation failed");
                return false;
            }
        }
        public async Task<List<Documents>?> GetDocumentByTag(List<Guid> TagIds)
        {
            try
            {
                _logger.LogInformation("start GetDocumentByTag operation");
                List<Guid>? DocsId = await _documentTagRepository.FetchDocumentIdByTagId(TagIds);
                var documents = await _context.documents.Where(docs => TagIds.Contains(docs.Id)).ToListAsync();
                if(documents== null)
                {
                    _logger.LogError("GetDocumentByTag operation return null");
                    return null;
                }
                return documents;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "GetDocumentByTag operation Failed");
                return null;
            }
        }
        public async Task<List<Tags>?> GetTagIdByDocumentId(Guid id)
        {
            try
            {
                List<Guid>? tagIds = await _documentTagRepository.FetchTagIdByDocumentId(id);
                if (tagIds == null)
                {
                    _logger.LogError("Get TagId by DocumentId return null");
                    return null;
                }
                return await _context.tags.Where(tag => tagIds.Contains(tag.Id)).ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Add tag to many document operation failed");
                return null;
            }
        }

        //tag on document operation
        /*private async Task<bool> SyncDocumentTag(Guid DocsId, List<Guid>? TagIds)
        {
            try
            {
                _logger.LogInformation($"start SyncDocumentTag operation {TagIds}");

                if(TagIds == null && TagIds.Count == 0)
                {
                    _logger.LogInformation($"doesn't change tag of the document {DocsId}");
                    return false;
                }

                //deleted tag cũ nếu không thấy trong DTO addTagsToDocument.
                //Lấy ra list danh sách các DocumentTag với DocumentId trùng với DTO addTagsToDocument
                */
        /*List<DocumentTag> documentTags = await _context.docsTags.Where(dt => dt.DocumentId == addTagsToDocument.DocumentId).ToListAsync();
                List<DocumentTag> needToDeleted = documentTags.Where(dt=> !addTagsToDocument.TagIdList.Contains(dt.TagId)).ToList();

                _context.RemoveRange(needToDeleted);
                
                _logger.LogInformation($"delete old tag {addTagsToDocument.DocumentId}");*/
        /*

                List<DocumentTag> needToDeleted = await (from dt in _context.docsTags
                                                  where dt.DocumentId == DocsId
                                                  && !TagIds.Contains(dt.TagId)
                                                  select dt).ToListAsync();

                //added thêm cái mới nếu nó mới
                //check DTO addTagsToDocument.TagId so với TagId trong database nó có mới không, rồi lấy nó ra
                //check mới như thế nào nhanh nhất? 1.Lấy ra hết các TagId của document đó trong database rồi lấy danh sách đó check với DTO
                */
        /*List<Guid> existingTagId = documentTags.Select(dt=>dt.TagId).ToList();
                List<Guid> TagIdNeedToInsert = addTagsToDocument.TagIdList.Where(at => !existingTagId.Contains(at)).ToList();

                List<DocumentTag> needToBeInserted = TagIdNeedToInsert.Select(id => new DocumentTag
                {
                    DocumentId = addTagsToDocument.DocumentId,
                    TagId = id,
                    CreatedAt = DateTime.UtcNow,
                }).ToList();
                
                _logger.LogInformation($"insert new tag operation {addTagsToDocument.DocumentId}");
                _context.AddRange(needToBeInserted);
                await _context.SaveChangesAsync();*/
        /*

                List<Guid> existingTagId = await (from dt in _context.docsTags
                                           where dt.DocumentId == DocsId
                                           select dt.TagId).ToListAsync();

                List<Guid> needToInsertId = TagIds.Where(tagsid => !existingTagId.Contains(tagsid)).ToList();

                List<DocumentTag> needToInsert = needToInsertId.Select(a=> new DocumentTag
                {
                    DocumentId = DocsId,
                    TagId = a,
                    CreatedAt = DateTime.UtcNow,
                }).ToList();

                if (needToInsert.Count > 0) 
                { 
                    _context.AddRange(needToInsert);
                }

                if (needToDeleted.Count>0)
                {
                    _context.RemoveRange(needToDeleted);
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e,"SyncDocumentTag operation failed");
                return false;
            }
        }
        public async Task<DocumentDetailView?> GetDetailDocument(Guid id)
        {
            try
            {
                _logger.LogInformation($"start fetching detail document {id}");
                */
        /*DocumentDetailView? a =  await _context.documents.Where(docs => docs.Id == id && docs.IsDeleted == false)
                    .Join(_context.docsTags, docs => docs.Id, dt=>dt.DocumentId, (docs, dt) => new { docs, dt })
                    .Join(_context.tags, combine=>combine.dt.TagId, tag=>tag.Id, (combine, tag) => new { combine, tag })
                    .GroupBy(x => new
                    {
                        x.combine.docs.Id,
                        x.combine.docs.Title,
                        x.combine.docs.Description,
                        x.combine.docs.ContentType,
                        x.combine.docs.Extension,
                    })
                    .Select(a => new DocumentDetailView
                    {
                        Id = a.Key.Id,
                        Title = a.Key.Title,
                        Description = a.Key.Description,
                        ContentType = a.Key.ContentType,
                        Extension = a.Key.Extension,
                        Tags = a.Select(x => new TagView
                        {
                            Id = x.tag.Id,
                            Name = x.tag.Name
                        }).ToList()
                    }).FirstOrDefaultAsync();*/
        /*

                DocumentDetailView? a = await (from docs in _context.documents
                        where docs.Id == id && docs.IsDeleted == false
                        select new DocumentDetailView
                        {
                            Id = docs.Id,
                            ContentType = docs.ContentType,
                            Extension   = docs.Extension,
                            Description = docs.Description,
                            Title = docs.Title
                        }).FirstOrDefaultAsync();
                if (a == null) 
                {
                    _logger.LogInformation("can't find detail document");
                    return null;
                }
                a.Tags = await (from dt in _context.docsTags
                         join tags in _context.tags on dt.TagId equals tags.Id
                         where dt.DocumentId == id
                         select new TagView
                         {
                             Id = dt.TagId,
                             ColorHex = tags.ColorHex,
                             Name = tags.Name,
                         }).ToListAsync();
                return a;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "fetch detail document failed");
                return null;
            }
        }*/

    }
}
