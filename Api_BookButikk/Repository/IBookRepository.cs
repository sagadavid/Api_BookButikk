using Api_BookButikk.Model;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api_BookButikk.Repository
{
    public interface IBookRepository
    {
        Task<List<BookModel>> GetAllBooks();
        Task<BookModel> GetBookById(int bookId);
        Task<int> PostNewBook(BookModel bookModel);
        Task UpdateBook(int bookId, BookModel bookModel);
        Task PatchTheBook(int bookId, JsonPatchDocument bookModel);
    }
}
