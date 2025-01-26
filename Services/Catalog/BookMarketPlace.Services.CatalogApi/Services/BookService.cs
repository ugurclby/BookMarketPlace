using AutoMapper;
using BookMarketPlace.Core.CustomResponse;
using BookMarketPlace.Core.Messages;
using BookMarketPlace.Services.CatalogApi.ConfigurationDbSettings;
using BookMarketPlace.Services.CatalogApi.Dtos;
using BookMarketPlace.Services.CatalogApi.Models;
using MassTransit;
using MongoDB.Driver;

namespace BookMarketPlace.Services.CatalogApi.Services
{
    public class BookService : IBookService
    {
        private readonly IMapper _mapper;

        private readonly IMongoCollection<Book> _bookCollection;
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly IMongoCollection<Author> _authorCollection;
        private readonly IPublishEndpoint _publishEndpoint;

        public BookService(IMapper mapper, IDbSettings dbSettings, IPublishEndpoint publishEndpoint)
        {
            _mapper = mapper;
            var client = new MongoClient(dbSettings.ConnectionString);
            var database = client.GetDatabase(dbSettings.DBName);
            _bookCollection = database.GetCollection<Book>(dbSettings.BookCollectionName);
            _categoryCollection = database.GetCollection<Category>(dbSettings.CategoryCollectionName);
            _authorCollection = database.GetCollection<Author>(dbSettings.AuthorCollectionName);
            _publishEndpoint = publishEndpoint;
        }
        public async Task<ICustomResponse<List<BookDto>>> GetAllAsync()
        {
            var books = await _bookCollection.Find(x => true).ToListAsync();

            if (books.Any())
            {
                List<Author> authorList = new List<Author>();
                foreach (var book in books)
                {
                    book.Category = await _categoryCollection.Find(x => x.Id == book.CategoryId).FirstAsync();

                }

            }
            else
            {
                books = new List<Book>();
            }

            return Core.CustomResponse.Response<List<BookDto>>.Success(_mapper.Map<List<BookDto>>(books), 200);
        }
        public async Task<ICustomResponse<BookDto>> GetByIdAsync(string Id)
        {
            var book = await _bookCollection.Find(x => x.Id == Id).FirstOrDefaultAsync();

            if (book == null)
            {
                return Core.CustomResponse.Response<BookDto>.Error(new List<string> { "Book not found" }, 404);
            }

            book.Category = await _categoryCollection.Find(x => x.Id == book.CategoryId).FirstAsync();
            book.Authors = await _authorCollection.Find(x => book.AuthorsIds.Equals(x)).ToListAsync();


            return Core.CustomResponse.Response<BookDto>.Success(_mapper.Map<BookDto>(book), 200);
        }
        public async Task<ICustomResponse<List<BookDto>>> GetAllByUserAsync(string userId)
        {
            var books = await _bookCollection.Find(x => x.CreatedUser == userId).ToListAsync();

            if (books == null)
            {
                return Core.CustomResponse.Response<List<BookDto>>.Error(new List<string> { "Book not found" }, 404);
            } 

            if (books.Any())
            {
                List<Author> authorList = new List<Author>();
                foreach (var book in books)
                {
                    book.Category = await _categoryCollection.Find(x => x.Id == book.CategoryId).FirstAsync();

                    book.Authors = await _authorCollection.Find(x => book.AuthorsIds.Equals(x)).ToListAsync();
                }
            }
            else
            {
                books = new List<Book>();
            }

            return Core.CustomResponse.Response<List<BookDto>>.Success(_mapper.Map<List<BookDto>>(books), 200);
        }
        public async Task<ICustomResponse<BookDto>> CreateAsync(BookCreateDto book)
        {
            var newBook = _mapper.Map<Book>(book);

            newBook.CreatedTime= DateTime.Now;

            await _bookCollection.InsertOneAsync(newBook);
            
            return Core.CustomResponse.Response<BookDto>.Success(_mapper.Map<BookDto>(newBook), 200);
        }

        public async Task<ICustomResponse<string>> UpdateAsync(BookUpdateDto book)
        {
            var updateBook = _mapper.Map<Book>(book);
            
            var result = await _bookCollection.FindOneAndReplaceAsync(x=>x.Id==book.Id, updateBook);

            if (result ==null)
            {
                return ResponseNoContent<string>.Error(new List<string> { "Book not found" }, 404);
            }

            await _publishEndpoint.Publish<BookNameChangedEvent>(new BookNameChangedEvent { BookId = book.Id, NewName = book.Name });

            return ResponseNoContent<string>.Success(204);
        }
        public async Task<ICustomResponse<string>> DeleteAsync(string id)
        {
            var result = await _bookCollection.DeleteOneAsync(x=>x.Id==id);    

            if (result.DeletedCount==0)
            {
                return ResponseNoContent<string>.Error(new List<string> { "Book not found" }, 404);
            }
            return ResponseNoContent<string>.Success(204);
        }
    }
}
