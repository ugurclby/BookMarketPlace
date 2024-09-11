using BookMarketPlace.Core.CustomController;
using BookMarketPlace.Core.CustomResponse;
using BookMarketPlace.Services.PhotoApi.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookMarketPlace.Services.PhotoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotosController : CustomBaseController
    {
        [HttpPost]

        public async Task<IActionResult> PhotoSave(IFormFile formFile,CancellationToken cancellationToken)
        {
            if (formFile != null && formFile.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos",formFile.FileName);

                using var stream = new FileStream(path, FileMode.Create);
                await formFile.CopyToAsync(stream, cancellationToken); 

                PhotoDto photo = new() { Url= "photos/" + formFile.FileName };

                return CreateActionResult(Response<PhotoDto>.Success(photo,200));
            }

            return CreateActionResult(Response<PhotoDto>.Error(new List<string> { "Dosya Boş" }, 400));

        }

        [HttpDelete]
        public IActionResult PhotoDelete(string photoUrl)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos", photoUrl);

            if (!System.IO.File.Exists(path))
            {
                return CreateActionResult(Response<Object>.Error(new List<string> { "Dosya Bulunamadı" }, 404));
            }

            System.IO.File.Delete(path);

            return CreateActionResult(Response<Object>.Success("Dosya Silindi", 200));

        }

    }
}
