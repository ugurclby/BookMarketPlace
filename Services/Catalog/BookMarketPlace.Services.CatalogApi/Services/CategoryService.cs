using AutoMapper;
using BookMarketPlace.Core.CustomResponse;
using BookMarketPlace.Services.CatalogApi.ConfigurationDbSettings;
using BookMarketPlace.Services.CatalogApi.Dtos;
using BookMarketPlace.Services.CatalogApi.Models;
using MongoDB.Driver;

namespace BookMarketPlace.Services.CatalogApi.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IMapper _mapper;

        private readonly IMongoCollection<Category> _mongoCollection;
        public CategoryService(IMapper mapper, IDbSettings dbSettings)
        {
            _mapper = mapper;
            var client = new MongoClient(dbSettings.ConnectionString);
            var database = client.GetDatabase(dbSettings.DBName);
            _mongoCollection = database.GetCollection<Category>(dbSettings.CategoryCollectionName);
        }
        public async Task<ICustomResponse<List<CategoryDto>>> GetAllAsync()
        {
            var categories = await _mongoCollection.Find(x => !String.IsNullOrEmpty(x.Name)).ToListAsync();

            return Response<List<CategoryDto>>.Success(_mapper.Map<List<CategoryDto>>(categories), 200);
        }
        public async Task<ICustomResponse<CategoryDto>> GetByIdAsync(string Id)
        {
            var category = await _mongoCollection.Find(x => x.Id == Id).FirstOrDefaultAsync();

            if (category == null)
            {
                return Response<CategoryDto>.Error(new List<string>() { "Category not found." }, 404);
            }

            return Response<CategoryDto>.Success(_mapper.Map<CategoryDto>(category), 200);
        }

        public async Task<ICustomResponse<CategoryDto>> CreateAsync(CategoryCreateDto category)
        {
            var newCategory = _mapper.Map<Category>(category);
            await _mongoCollection.InsertOneAsync(_mapper.Map<Category>(newCategory));
            return Response<CategoryDto>.Success(_mapper.Map<CategoryDto>(newCategory), 200); 

        }
        public async Task<ICustomResponse<string>> UpdateAsync(CategoryDto category)
        {
            await _mongoCollection.FindOneAndReplaceAsync(x => x.Id == category.Id, _mapper.Map<Category>(category));
            return ResponseNoContent<string>.Success(204);
        }
        public async Task<ICustomResponse<string>> DeleteAsync(string id)
        {
            var sonuc = await _mongoCollection.DeleteOneAsync(x => x.Id == id);
            if (sonuc.DeletedCount == 0)
            {
                return ResponseNoContent<string>.Error(new List<string> { "Category Not Found" }, 404);
            }
            return ResponseNoContent<string>.Success(204);
        }
    }
}
