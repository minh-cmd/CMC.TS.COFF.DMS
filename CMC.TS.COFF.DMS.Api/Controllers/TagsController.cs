using CMC.TS.COFF.DMS.Biz.IRepositories;
using CMC.TS.COFF.DMS.Biz.Model.Tags;
using CMC.TS.COFF.DMS.Data.Model;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CMC.TS.COFF.DMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly ITagsRepository _tagsRepository;
        public TagsController(ITagsRepository tagsRepository)
        {
            _tagsRepository = tagsRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Create(NewTag tags)
        {
            bool isSuccess = await _tagsRepository.CreateTag(tags);
            if (isSuccess)
            {
                return Ok();
            }
            return BadRequest();
        }

        /*[HttpGet]
        public async Task<IActionResult> GetAllTags()
        {
            var tags = await _tagsRepository.GetAllTags();
            if (tags != null)
            {
                return Ok(tags);
            }
            return BadRequest();
        }*/

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTagById(Guid id)
        {
            var tag = await _tagsRepository.GetTagById(id);
            if (tag != null)
            {
                return Ok(tag);
            }
            return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTag(Guid id, NewTag tags)
        {
            bool isSuccess = await _tagsRepository.UpdateTag(id, tags);
            if (isSuccess)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id) 
        { 
            bool isSuccess = await _tagsRepository.DeleteTag(id);
            if (isSuccess)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> GetTags([FromQuery]FilterTag? filter)
        {
            List<Tags>? tags = await _tagsRepository.FilterTag(filter);
            if(tags != null)
            {
                return Ok(tags);
            }
            return BadRequest();
        }

        [HttpPut("{TagId}/document")]
        public async Task<IActionResult> AddDocumentsToTag(Guid TagId, [FromBody] List<Guid> DocIds)
        {
            bool isSuccess = await _tagsRepository.AddDocumentsToTag(TagId, DocIds);
            if (isSuccess)
            {
                return Ok();
            }
            return BadRequest();
        }

    }
}
