using Api_BookButikk.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api_BookButikk.Repository
{
    public interface IBookRepository
    {
        Task<List<BookModel>> GetAllBooks();
        Task<BookModel> GetBookById(int bookId);
    }
}
