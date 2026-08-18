using CMC.TS.COFF.DMS.Biz.IRepositories;
using CMC.TS.COFF.DMS.Biz.Model.DocTag;
using CMC.TS.COFF.DMS.Data;
using CMC.TS.COFF.DMS.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace CMC.TS.COFF.DMS.Biz.Repositories
{
    public class DocumentsTagRepository : IDocumentTagRepository
    {
        private readonly SQLServerDbContext _context;
        private ILogger<DocumentsTagRepository> _logger;
        public DocumentsTagRepository(SQLServerDbContext context, ILogger<DocumentsTagRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> CreateDocumentTag(CreateDocTag create)
        {
            try
            {
                _logger.LogInformation("Start create DocumentTag operation {id}", create.DocumentId);
                DocumentTag documentTag = new DocumentTag
                {
                    DocumentId = create.DocumentId,
                    TagId = create.TagId,
                    CreatedAt = DateTime.UtcNow,
                };
                _context.docsTags.Add(documentTag);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Create DocumentTag operation failed");
                return false;
            }
        }
        public async Task<bool> DeleteDocsTag(Guid docId, Guid tagId)
        {
            try
            {
                _logger.LogInformation("Start delete DocumentTag operation document: {id} and tag: {tagid}", docId, tagId);
                DocumentTag? documentTag = await GetDocumentTagById(docId, tagId); 
                if(documentTag == null)
                {
                    _logger.LogError("Can't find DocumentTag");
                    return false;
                }
                else
                {
                    _context.docsTags.Remove(documentTag);
                    await _context.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Delete documentTags failed");
                return false;
            }
        }
        public async Task<List<DocumentTag>?> GetAllDocumentTag()
        {
            try
            {
                _logger.LogInformation("Start get all DocumentTag operation");
                return await _context.docsTags.Where(dt => true).ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Get all DocumentTag operation failed");
                return null;
            }
        }
        public async Task<DocumentTag?> GetDocumentTagById(Guid docId, Guid tagId)
        {
            try
            {
                _logger.LogInformation("Start get DocumentTag by id operation document: {id} and tag: {tagid}", docId, tagId);
                DocumentTag? documentTag = await _context.docsTags.FirstOrDefaultAsync(dt=> dt.DocumentId == docId  && dt.TagId == tagId);
                return documentTag;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Get documentTags by Id failed");
                return null;
            }
        }
        public async Task<bool> UpdateDocsTag(Guid docId, Guid tagId, UpdateDocTag updateDocTag)
        {
            try
            {
                _logger.LogInformation("Start update DocumentTag operation");
                DocumentTag? documentTag = await GetDocumentTagById(docId, tagId);
                if (documentTag == null)
                {
                    _logger.LogError("Can't find DocumentTag");
                    return false;
                }
                
                //mapping
                documentTag.TagId = updateDocTag.TagId;
                documentTag.DocumentId = updateDocTag.DocId;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Update DocumentTag operation failed");
                return false;
            }
        }

        //in use:
        public async Task<bool> AddTagsToDocument(Guid DocId, List<Guid> TagIds)
        {
            try
            {
                _logger.LogInformation("Start adding many tags to document operation");
                var add = TagIds.Select(tag => new DocumentTag
                {
                    DocumentId = DocId,
                    CreatedAt = DateTime.UtcNow,
                    TagId = tag
                });
                _context.docsTags.AddRange(add);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "adding many tags to document operation failed");
                return false;
            }
        }
        public async Task<bool> AddDocumentsToTag(Guid TagId, List<Guid> DocIds) 
        {
            try
            {
                _logger.LogInformation("Start adding one tag many document operation");


                var add = DocIds.Select(docs => new DocumentTag
                {
                    DocumentId = docs,
                    CreatedAt = DateTime.UtcNow,
                    TagId = TagId
                });
                _context.docsTags.AddRange(add);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "adding one tag to many document operation failed");
                return false;
            }
        }
        public async Task<List<Guid>?> FetchDocumentIdByTagId(List<Guid> TagIds)
        {
            try
            {
                _logger.LogInformation("Start Fetching DocumentId By TagId");
                var queryable = from dt in _context.docsTags
                                where TagIds.Contains(dt.TagId)
                                group dt by dt.DocumentId into g
                                where g.Count() == TagIds.Count()
                                select g.Key;
                return await queryable.ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "FetchDocumentIdByTagId failed");
                return null;
            }
        }
        public async Task<List<Guid>?> FetchTagIdByDocumentId(Guid? DocsId)
        {
            try
            {
                _logger.LogInformation("Start Fetching TagId By DocumentId");
                List<Guid>? tagIds = await _context.docsTags.Where(dt=>dt.DocumentId == DocsId).Select(dt=> dt.TagId).ToListAsync();
                if(tagIds == null || tagIds.Count <= 0)
                {
                    _logger.LogError("Fetching TagId By DocumentId return nothing");
                    return null;
                }
                return tagIds;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "FetchTagIdByDocumentId failed");
                return null;
            }
        }
        public async Task<bool> UpdateTagIdOfDocument(Guid DocId, List<Guid>? TagIdsComming)
        {
            try
            {
                _logger.LogInformation("Update tag of document failed");
                if (TagIdsComming == null ) 
                {
                    _logger.LogError("TagId incomming is null");
                    return false;
                }
                
                //phần update thì sẽ đặt tiêu chuẩn của các tagid mới ở mức ưu tiên cao nhất
                //nếu ở database có tagId mà ở phần TagIdsComming không có, thì sẽ xoá hết tagId cũ
                //Nếu ở database có tagId trùng với TagIdsComming thì giữ nguyên.
                // Nếu ở database không có tagId giống với phần TagIdsComming thì sẽ thêm vào.

                //cần phải xoá
                List<DocumentTag>? needToDelete = await _context.docsTags.Where(dt=> !TagIdsComming.Contains(dt.TagId) && dt.DocumentId == DocId).ToListAsync();
                if(needToDelete.Count > 0)
                {
                    _context.RemoveRange(needToDelete);
                }

                //cần phải thêm vào
                //lọc tagIds mới phải thêm vào 
                //Phải tìm list id của tagId đang tồn tại
                HashSet<Guid> existingTagIds = await _context.docsTags.Where(dt => dt.DocumentId == DocId).Select(dt=>dt.TagId).ToHashSetAsync();
                HashSet<Guid> needToInsertTagId = TagIdsComming.Where(tagsid=>!existingTagIds.Contains(tagsid)).ToHashSet();
                HashSet<DocumentTag> needToInsert = needToInsertTagId.Select(id=> new DocumentTag
                {
                    DocumentId = DocId,
                    TagId = id,
                    CreatedAt = DateTime.UtcNow,
                }).ToHashSet();
                
                if(needToInsert.Count > 0)
                {
                    _context.AddRange(needToInsert);
                }
                return await _context.SaveChangesAsync() > 0;

            }
            catch (Exception e)
            {
                _logger.LogError(e, "Update tag of document failed");
                return false;
            }
        }
        public async Task<bool> DeleteTagOfDocument(Guid docId)
        {
            try
            {
                _logger.LogInformation("Delete tag from document start");
                var a = (from dt in _context.docsTags
                         where dt.DocumentId == docId
                         select dt);
                _context.RemoveRange(a);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Delete tag from Document failed");
                return false;
            }
        }
        public async Task<bool> DeleteDocumentOfTag(Guid TagId)
        {
            try
            {
                _logger.LogInformation("Delete tag from document start");
                var a = (from dt in _context.docsTags
                        where dt.TagId == TagId
                        select dt);
                _context.RemoveRange(a);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Delete document from tag failed");
                return false;
            }
        }
    }
}
