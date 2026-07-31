using CMC.TS.COFF.DMS.Biz.IRepositories;
using CMC.TS.COFF.DMS.Biz.Model.Categories;
using CMC.TS.COFF.DMS.Data.Migrations;
using CMC.TS.COFF.DMS.Data.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CMC.TS.COFF.DMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoriesRepository _categoriesRepository;
        public CategoriesController(ICategoriesRepository categoriesRepository) 
        { 
            _categoriesRepository = categoriesRepository;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(NewCategory newCategory)
        {
            bool isSuccess = await _categoriesRepository.Create(newCategory);
            if (isSuccess)
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
