using CMC.TS.COFF.DMS.Biz.IRepositories;
using CMC.TS.COFF.DMS.Data;
using CMC.TS.COFF.DMS.Biz.Model.Categories;
using System;
using System.Collections.Generic;
using System.Text;
using CMC.TS.COFF.DMS.Data.Model;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace CMC.TS.COFF.DMS.Biz.Repositories
{
    public class CategoriesRepository : ICategoriesRepository
    {
        private readonly SQLServerDbContext _context;
        private readonly ILogger<CategoriesRepository> _logger;
        public CategoriesRepository (SQLServerDbContext context, ILogger<CategoriesRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> Create(NewCategory news)
        {
            try
            {
                _logger.LogInformation($"starting create operation in categories");
                _context.categories.Add(news.CategoriesNew());
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed create operation in categories {e}");
                return false;
            }
        }

        public async Task<List<Categories>?> GetAllCategories()
        {
            try
            {
                _logger.LogInformation($"start fetching categories operation");
                return await _context.categories.Where(a => a.IsDeleted == false).ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed fetching categories {e}");
                return null;
            }
        }

        public async Task<Categories?> GetCategoryById(Guid id)
        {
            try
            {
                _logger.LogInformation($"start fetching category with id = {id}");
                return await _context.categories.FirstOrDefaultAsync(a => a.Id == id && a.IsDeleted == false);
            }
            catch (Exception e)
            {
                _logger.LogError($"Get Category by Id failed {e.Message}");
                return null;
            }
        }

        public async Task<bool> Update(Guid id, NewCategory news)
        {
            try
            {
                _logger.LogInformation($"starting update operation for category {id}");
                Categories? targetCartegory = await GetCategoryById(id);
                if (targetCartegory != null)
                {
                    targetCartegory.Name = news.Name;
                    targetCartegory.Code = news.Code;
                    targetCartegory.Description = news.Description;
                    return await _context.SaveChangesAsync() > 0;
                }
                _logger.LogError($"can't find category in update operation");
                return false;
            }
            catch (Exception e)
            {
                _logger.LogInformation($"Update operation failed {e.Message}");
                return false;
            }
        }

        public async Task<bool> Delete(Guid id)
        {
            try
            {
                _logger.LogInformation("starting delete operation");
                Categories? categories = await GetCategoryById(id);
                if (categories != null)
                {
                    categories.IsDeleted = true;
                    return await _context.SaveChangesAsync() > 0;
                }
                _logger.LogError($"can't find category {id}");
                return false;
            }
            catch (Exception e)
            {
                _logger.LogError($"delete operation failed {e.Message}");
                return false;
            }
            
        }
    }
}
