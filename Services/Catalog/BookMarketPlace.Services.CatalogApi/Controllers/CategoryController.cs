using BookMarketPlace.Core.CustomController;
using BookMarketPlace.Services.CatalogApi.Dtos;
using BookMarketPlace.Services.CatalogApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookMarketPlace.Services.CatalogApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : CustomBaseController
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _categoryService.GetAllAsync();

            return CreateActionResult(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
             var result= await _categoryService.GetByIdAsync(id);

            return CreateActionResult(result);
        } 
        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateDto category)
        {
            var result = await _categoryService.CreateAsync(category);

            return CreateActionResult(result);
        }
        [HttpPut]
        public async Task<IActionResult> Update(CategoryDto category)
        {
            var result = await _categoryService.UpdateAsync(category);

            return CreateActionResult(result);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _categoryService.DeleteAsync(id);

            return CreateActionResult(result);
        }
    }
}
