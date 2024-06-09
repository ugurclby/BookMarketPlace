using BookMarketPlace.Core.CustomController;
using BookMarketPlace.Services.CatalogApi.Dtos;
using BookMarketPlace.Services.CatalogApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookMarketPlace.Services.CatalogApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : CustomBaseController
    {
        private readonly IAuthorService _AuthorService;

        public AuthorController(IAuthorService AuthorService)
        {
            _AuthorService = AuthorService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _AuthorService.GetAllAsync();

            return CreateActionResult(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
             var result= await _AuthorService.GetByIdAsync(id);

            return CreateActionResult(result);
        } 
        [HttpPost]
        public async Task<IActionResult> Create( AuthorCreateDto Author)
        {
            var result = await _AuthorService.CreateAsync(Author);

            return CreateActionResult(result);
        }
        [HttpPut]
        public async Task<IActionResult> Update(AuthorUpdateDto Author)
        {
            var result = await _AuthorService.UpdateAsync(Author);

            return CreateActionResult(result);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _AuthorService.DeleteAsync(id);

            return CreateActionResult(result);
        }
    }
}
