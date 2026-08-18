using CMC.TS.COFF.DMS.Biz.Model.Documents;
using CMC.TS.COFF.DMS.Biz.Model.Categories;

using CMC.TS.COFF.DMS.Data.Model;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace CMC.TS.COFF.DMS.Biz
{
    public static class Mapper
    {
        public static Documents DocumentsToNew(this New news)
        {
            if (news == null)
            {
                return null;
            }
            return new Documents
            {
                Title = news.Title,
                Description = news.Description,
                ContentType = news.ContentType,
                Extension = news.Extension,
            };
        }
        public static void DocumentsToUpdate(this Update news, Documents docs)
        {
            if (news == null)
                return;
            docs.Title = news.Title;
            docs.Extension = news.Extension;
            docs.ContentType = news.ContentType;
            docs.Description = news.Description;
        }
        public static Categories CategoriesNew(this NewCategory newCategory)
        {
            return new Categories
            {
                Name = newCategory.Name,
                Description = newCategory.Description,
                Code = newCategory.Code,
            };

        }

    }
}
