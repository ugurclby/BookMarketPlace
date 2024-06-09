using AutoMapper;
using BookMarketPlace.Core.CustomResponse;
using BookMarketPlace.Services.CatalogApi.ConfigurationDbSettings;
using BookMarketPlace.Services.CatalogApi.Dtos;
using BookMarketPlace.Services.CatalogApi.Models;
using MongoDB.Driver;

namespace BookMarketPlace.Services.CatalogApi.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IMapper _mapper;

        private readonly IMongoCollection<Author> _mongoCollection;
        public AuthorService(IMapper mapper, IDbSettings dbSettings)
        {
            _mapper = mapper;
            var client = new MongoClient(dbSettings.ConnectionString);
            var database = client.GetDatabase(dbSettings.DBName);
            _mongoCollection = database.GetCollection<Author>(dbSettings.AuthorCollectionName);
        }
        public async Task<ICustomResponse<List<AuthorDto>>> GetAllAsync()
        {
            var authors = await _mongoCollection.Find(x => !String.IsNullOrEmpty(x.Name)).ToListAsync();

            return Response<List<AuthorDto>>.Success(_mapper.Map<List<AuthorDto>>(authors), 200);
        }
        public async Task<ICustomResponse<AuthorDto>> GetByIdAsync(string Id)
        {
            var author = await _mongoCollection.Find(x => x.Id == Id).FirstOrDefaultAsync();

            if (author == null)
            {
                return Response<AuthorDto>.Error(new List<string>() { "Author not found." }, 404);
            }

            return Response<AuthorDto>.Success(_mapper.Map<AuthorDto>(author), 200);
        }

        public async Task<ICustomResponse<AuthorDto>> CreateAsync(AuthorCreateDto Author)
        {
            var newAuthor = _mapper.Map<Author>(Author);
            await _mongoCollection.InsertOneAsync(newAuthor);
            return Response<AuthorDto>.Success(_mapper.Map<AuthorDto>(newAuthor), 200); 

        }
        public async Task<ICustomResponse<string>> UpdateAsync(AuthorUpdateDto Author)
        {
            await _mongoCollection.FindOneAndReplaceAsync(x => x.Id == Author.Id, _mapper.Map<Author>(Author));
            return ResponseNoContent<string>.Success(204);
        }
        public async Task<ICustomResponse<string>> DeleteAsync(string id)
        {
            var sonuc = await _mongoCollection.DeleteOneAsync(x => x.Id == id);
            if (sonuc.DeletedCount == 0)
            {
                return ResponseNoContent<string>.Error(new List<string> { "Author Not Found" }, 404);
            }
            return ResponseNoContent<string>.Success(204);
        } 
    }
}
