using CMC.TS.COFF.DMS.Biz.IRepositories;
using CMC.TS.COFF.DMS.Biz.Model.Documents;
using CMC.TS.COFF.DMS.Biz.Repositories;
using CMC.TS.COFF.DMS.Data.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CMC.TS.COFF.DMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly IDocumentsRepository _documentsService;
        private readonly ILogger<DocumentsController> _logger;

        public DocumentsController(IDocumentsRepository documentsService, ILogger<DocumentsController> logger)
        {
            _documentsService = documentsService;
            _logger = logger;
        }
        [HttpPost]
        public async Task<IActionResult> AddNewDocument(New news)
        {
            bool isSuccess = await _documentsService.Create(news);
            if (isSuccess)
            {
                return Ok();
            }
            return BadRequest();
        }

        /*[HttpGet("{id::guid}")]
        public async Task<IActionResult> GetDocumentById(Guid id)
        {
            //return Ok(await _documentsService.GetDocumentById(id));
            //return Ok(await  _documentsService.GetDetailDocument(id));
        }*/

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, Update updateDto)
        {
            bool isSuccess = await _documentsService.Update(id, updateDto);
            if (isSuccess)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id) 
        { 
            bool isSuccess = await _documentsService.Delete(id);
            if (isSuccess)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> DynamicFilter([FromQuery] Filter filter)
        {
            var queryable = await _documentsService.DynamicFilter(filter).ToListAsync();
            if (queryable.Count > 0 || queryable != null) 
            {
                return Ok(queryable);
            }
            return BadRequest();
        }

        [HttpPut("{DocId:guid}/tag")]
        public async Task<IActionResult> AddTagToDocument(Guid DocId, [FromBody] List<Guid> TagIds)
        {
            bool isSuccess = await _documentsService.AddTagsToDocument(DocId,TagIds);
            if (isSuccess)
            {
                return Ok();
            }
            else
                return BadRequest();
        }


        [HttpGet("{id:guid}/tag")]
        public async Task<IActionResult> GetTagIdByDocumentId(Guid id)
        {
            List<Tags>? a = await _documentsService.GetTagIdByDocumentId(id);
            if (a == null || a.Count <= 0)
            {
                return BadRequest();
            }
            return Ok(a);
        }
    }
}
