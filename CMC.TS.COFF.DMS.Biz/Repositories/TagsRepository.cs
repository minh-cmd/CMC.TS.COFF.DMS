using CMC.TS.COFF.DMS.Biz.IRepositories;
using CMC.TS.COFF.DMS.Biz.Model.Tags;
using CMC.TS.COFF.DMS.Data;
using CMC.TS.COFF.DMS.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace CMC.TS.COFF.DMS.Biz.Repositories
{
    public class TagsRepository : ITagsRepository
    {
        private readonly SQLServerDbContext _context;
        private readonly ILogger<TagsRepository> _logger;
        private readonly IDocumentTagRepository _documentTagRepository;

        public TagsRepository(SQLServerDbContext context, ILogger<TagsRepository> logger, IDocumentTagRepository documentTagRepository)
        {
            _logger = logger;
            _context = context;
            _documentTagRepository = documentTagRepository;
        }
        public async Task<bool> CreateTag(NewTag tags)
        {
            try
            {
                _logger.LogInformation($"start creating tag operation {tags.Name}");
                Tags tag = new Tags(tags.Name, tags.ColorHex);
                _context.tags.Add(tag);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception e)
            {
                _logger.LogError($"Create Tag operation failed {e.Message}");
                return false;
            }
        }
        public async Task<bool> DeleteTag(Guid id)
        {
            try
            {
                _logger.LogInformation($"start deleting tags operation");
                Tags? tag = await GetTagById(id);
                if (tag != null)
                {
                    tag.IsDeleted = true;
                    return await _context.SaveChangesAsync() > 0;
                }
                _logger.LogError($"DeleteTag operation can't find id: {id}");
                return false;
            }
            catch (Exception e)
            {
                _logger.LogError($"DeleteTag operation failed {e.Message}");
                return false;
            }
        }
        public async Task<List<Tags>?> FilterTag(FilterTag filterTag)
        {
            try
            {
                _logger.LogInformation($"start filtering operation");
                IQueryable<Tags> queryable = _context.tags;

                if (!string.IsNullOrEmpty(filterTag.Name))
                {
                    _logger.LogInformation($"adding name filter");
                    string searchTerm = filterTag.Name.ToLower().Trim();
                    queryable = queryable.Where(q => q.Name.ToLower().Trim().Contains(searchTerm) && q.IsDeleted == false);
                }

                if (!string.IsNullOrEmpty(filterTag.ColorHex))
                {
                    _logger.LogInformation($"adding color filter");
                    string searchTerm = filterTag.ColorHex.ToLower().Trim();
                    queryable = queryable.Where(q => q.ColorHex.ToLower().Trim().Contains(searchTerm) && q.IsDeleted == false);
                }

                if (filterTag.CreatedAtFrom.HasValue)
                {
                    _logger.LogInformation($"adding create from date filter");
                    queryable = queryable.Where(q => q.CreatedAt >= filterTag.CreatedAtFrom);
                }

                if (filterTag.CreatedAtTo.HasValue)
                {
                    _logger.LogInformation($"adding create to date filter");
                    queryable = queryable.Where(q => q.CreatedAt <= filterTag.CreatedAtTo);
                }

                return await queryable.ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"Filter operation failed {e.Message}");
                return null;
            }
        }
        public async Task<List<Tags>?> GetAllTags()
        {
            try
            {
                _logger.LogInformation($"start fetching all tags operation");
                return await _context.tags.Where(t => t.IsDeleted == false).ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"Get all tags {e.Message}");
                return null;
            }
        }
        public async Task<Tags?> GetTagById(Guid id)
        {
            try
            {
                _logger.LogInformation($"start get tag by id operation {id}");
                Tags? tags = await _context.tags.FirstOrDefaultAsync(t => t.Id == id);
                return tags;
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to get tag by Id {e.Message}");
                return null;
            }
        }
        public async Task<bool> UpdateTag(Guid id, NewTag newTags)
        {
            try
            {
                _logger.LogInformation($"UpdateTag operation start {id}");
                Tags? tag = await GetTagById(id);
                if (tag != null)
                {
                    tag.Name = newTags.Name;
                    tag.ColorHex = newTags.ColorHex;
                    return await _context.SaveChangesAsync() > 0;
                }
                _logger.LogError($"UpdateTag operation can't find id: {id}");
                return false;
            }
            catch (Exception e)
            {
                _logger.LogError($"UpdateTag operation Failed {e.Message}");
                return false;
            }
        }
        public async Task<bool> AddDocumentsToTag(Guid TagId, List<Guid> DocIds)
        {
            try
            {
                _logger.LogInformation("Add tag to many document operation");
                bool isSuccess = await _documentTagRepository.AddDocumentsToTag(TagId,DocIds);
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
                _logger.LogError(e, "Add tag to many document operation failed");
                return false;
            }
        }

    }
}
