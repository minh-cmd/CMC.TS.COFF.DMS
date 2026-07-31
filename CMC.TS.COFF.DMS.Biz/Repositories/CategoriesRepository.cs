using CMC.TS.COFF.DMS.Biz.IRepositories;
using CMC.TS.COFF.DMS.Data;
using CMC.TS.COFF.DMS.Biz.Model.Categories;
using System;
using System.Collections.Generic;
using System.Text;
using CMC.TS.COFF.DMS.Data.Model;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

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
                return await _context.categories.Where(a => true).ToListAsync();
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
                return await _context.categories.FirstOrDefaultAsync(a => a.Id == id);
            }
            catch (Exception e)
            {
                _logger.LogError($"Get Category by Id failed", e);
                return null;
            }
        }
    }
}
