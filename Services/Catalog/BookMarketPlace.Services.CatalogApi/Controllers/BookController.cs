using BookMarketPlace.Core.CustomController;
using BookMarketPlace.Services.CatalogApi.Dtos;
using BookMarketPlace.Services.CatalogApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookMarketPlace.Services.CatalogApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : CustomBaseController
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _bookService.GetAllAsync();

            return CreateActionResult(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
             var result= await _bookService.GetByIdAsync(id);

            return CreateActionResult(result);
        }
        [HttpGet]
        [Route("/api/[controller]/GetAllByUserId/{userId}")]
        public async Task<IActionResult> GetAllByUserId(string userId)
        {
            var result = await _bookService.GetAllByUserAsync(userId);

            return CreateActionResult(result);
        }
        [HttpPost]
        public async Task<IActionResult> Create(BookCreateDto book)
        {
            var result = await _bookService.CreateAsync(book);

            return CreateActionResult(result);
        }
        [HttpPut]
        public async Task<IActionResult> Update(BookUpdateDto book)
        {
            var result = await _bookService.UpdateAsync(book);

            return CreateActionResult(result);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _bookService.DeleteAsync(id);

            return CreateActionResult(result);
        }
    }
}
