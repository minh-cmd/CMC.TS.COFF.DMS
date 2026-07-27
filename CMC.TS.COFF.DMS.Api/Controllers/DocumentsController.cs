using CMC.TS.COFF.DMS.Biz.IServices;
using CMC.TS.COFF.DMS.Biz.Model.Documents;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CMC.TS.COFF.DMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly IDocumentsService _documentsService;

        public DocumentsController (IDocumentsService documentsService)
        {
            _documentsService = documentsService;
        }
        // GET: api/<DocumentsController>
        [HttpPost]
        public async Task<IActionResult> AddNewDocument(New news)
        {
            bool isSuccess = await _documentsService.NewDocument(news);
            if (isSuccess)
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
