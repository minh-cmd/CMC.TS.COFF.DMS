using CMC.TS.COFF.DMS.Biz.IRepositories;
using CMC.TS.COFF.DMS.Biz.IServices;
using CMC.TS.COFF.DMS.Biz.Model.Documents;
using CMC.TS.COFF.DMS.Data.Model;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace CMC.TS.COFF.DMS.Biz.Services
{
    public class DocumentsService : IDocumentsService
    {
        private readonly IDocumentsRepository _documentsRepository;

        public DocumentsService(IDocumentsRepository documentsRepository)
        {
            _documentsRepository = documentsRepository;
        }
        public async Task<bool> NewDocument(New news)
        {
            if (news != null)
            {
                Documents document = new Documents(news.Title, news.Description, news.ContentType, news.Extension);
                return await _documentsRepository.Create(document);
            }
            return false;
        }
    }
}
