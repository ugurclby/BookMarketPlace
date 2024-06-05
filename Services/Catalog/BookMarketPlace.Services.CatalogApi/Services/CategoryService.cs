using AutoMapper;
using BookMarketPlace.Core.CustomResponse;
using BookMarketPlace.Services.CatalogApi.ConfigurationDbSettings;
using BookMarketPlace.Services.CatalogApi.Dtos;
using BookMarketPlace.Services.CatalogApi.Models;
using MongoDB.Driver;

namespace BookMarketPlace.Services.CatalogApi.Services
{
    public class CategoryService: ICategoryService
    {
        private readonly IMapper _mapper;

        private readonly IMongoCollection<Category> _mongoCollection;
        public CategoryService(IMapper mapper,IDbSettings dbSettings)
        {
            _mapper = mapper;
            var client = new MongoClient(dbSettings.ConnectionString);
            var database = client.GetDatabase(dbSettings.DBName);
            _mongoCollection = database.GetCollection<Category>(dbSettings.CategoryCollectionName);
        }
        public async Task<Response<List<CategoryDto>>> GetAll()
        {
            var categories = await _mongoCollection.Find(x => !String.IsNullOrEmpty(x.Name)).ToListAsync();

            return Response<List<CategoryDto>>.Success(_mapper.Map<List<CategoryDto>>(categories),200);
        }
        public async Task<Response<CategoryDto>> GetById(string Id)
        {
            var category = await _mongoCollection.Find(x => x.Id == Id).FirstOrDefaultAsync();

            if (category==null)
            {
                return Response<CategoryDto>.Error(new List<string>() { "Category not found." }, 404);
            }

            return Response<CategoryDto>.Success(_mapper.Map<CategoryDto>(category), 200);
        }

        public async Task<Response<CategoryDto>> CreateAsync(Category category)
        {
            await _mongoCollection.InsertOneAsync(category);
            return Response<CategoryDto>.Success(_mapper.Map<CategoryDto>(category),200);
        }
    }
}
