using CMC.TS.COFF.DMS.Biz.Model.Categories;
using CMC.TS.COFF.DMS.Data.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CMC.TS.COFF.DMS.Biz.IRepositories
{
    public interface ICategoriesRepository
    {
        Task<bool> Create(NewCategory news);
        Task<List<Categories>?> GetAllCategories();

    }
}
