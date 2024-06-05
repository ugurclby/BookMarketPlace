using AutoMapper;
using BookMarketPlace.Core.CustomResponse;
using BookMarketPlace.Services.CatalogApi.ConfigurationDbSettings;
using BookMarketPlace.Services.CatalogApi.Dtos;
using BookMarketPlace.Services.CatalogApi.Models;
using MongoDB.Driver;

namespace BookMarketPlace.Services.CatalogApi.Services
{
    public class BookService : IBookService
    {
        private readonly IMapper _mapper;

        private readonly IMongoCollection<Book> _bookCollection;
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly IMongoCollection<Author> _authorCollection;
        public BookService(IMapper mapper, IDbSettings dbSettings)
        {
            _mapper = mapper;
            var client = new MongoClient(dbSettings.ConnectionString);
            var database = client.GetDatabase(dbSettings.DBName);
            _bookCollection = database.GetCollection<Book>(dbSettings.BookCollectionName);
            _categoryCollection = database.GetCollection<Category>(dbSettings.CategoryCollectionName);
            _authorCollection = database.GetCollection<Author>(dbSettings.AuthorCollectionName);
        }
        public async Task<Response<List<BookDto>>> GetAllAsync()
        {
            var books = await _bookCollection.Find(x => true).ToListAsync();

            if (books.Any())
            {
                List<Author> authorList = new List<Author>();
                foreach (var book in books)
                {
                    book.Category = await _categoryCollection.Find(x => x.Id == book.CategoryId).FirstAsync();

                    book.Authors = await _authorCollection.Find(x => book.AuthorsIds.Equals(x)).ToListAsync();

                    //foreach (var author in book.AuthorsIds)
                    //{
                    //    authorList.Add(_authorCollection.Find(x => x.Id == author).First());
                    //}
                    //if (authorList.Count>0)
                    //{
                    //    book.Authors = authorList;
                    //}
                }
            }
            else
            {
                books = new List<Book>();
            }

            return Response<List<BookDto>>.Success(_mapper.Map<List<BookDto>>(books), 200);
        }
        public async Task<Response<BookDto>> GetByIdAsync(string Id)
        {
            var book = await _bookCollection.Find(x => x.Id == Id).FirstOrDefaultAsync();

            if (book == null)
            {
                return Response<BookDto>.Error(new List<string> { "Book not found" }, 404);
            }

            book.Category = await _categoryCollection.Find(x => x.Id == book.CategoryId).FirstAsync();
            book.Authors = await _authorCollection.Find(x => book.AuthorsIds.Equals(x)).ToListAsync();


            return Response<BookDto>.Success(_mapper.Map<BookDto>(book), 200);
        }
        public async Task<Response<List<BookDto>>> GetAllByUserAsync(string userId)
        {
            var books = await _bookCollection.Find(x => x.CreatedUser == userId).ToListAsync();

            if (books == null)
            {
                return Response<List<BookDto>>.Error(new List<string> { "Book not found" }, 404);
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

            return Response<List<BookDto>>.Success(_mapper.Map<List<BookDto>>(books), 200);
        }
        public async Task<Response<BookDto>> CreateAsync(BookCreateDto book)
        {
            var newBook = _mapper.Map<Book>(book);

            newBook.CreatedTime= DateTime.Now;

            await _bookCollection.InsertOneAsync(newBook);
            
            return Response<BookDto>.Success(_mapper.Map<BookDto>(newBook), 200);
        }

        public async Task<ResponseNoContent> UpdateAsync(BookUpdateDto book)
        {
            var updateBook = _mapper.Map<Book>(book);
            
            var result = _bookCollection.FindOneAndReplaceAsync(x=>x.Id==book.Id, updateBook);

            if (result ==null)
            {
                return ResponseNoContent.Error(new List<string> { "Book not found" }, 404);
            }
            return ResponseNoContent.Success(204);
        }
        public async Task<ResponseNoContent> DeleteAsync(string id)
        {
            var result = await _bookCollection.DeleteOneAsync(x=>x.Id==id);    

            if (result.DeletedCount==0)
            {
                return ResponseNoContent.Error(new List<string> { "Book not found" }, 404);
            }
            return ResponseNoContent.Success(204);
        }
    }
}
