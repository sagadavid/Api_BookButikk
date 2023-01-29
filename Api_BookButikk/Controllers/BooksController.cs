using Api_BookButikk.Model;
using Api_BookButikk.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api_BookButikk.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;

        public BooksController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }


        [HttpGet("")]
        public async Task<IActionResult> GetAllBooks() 
        {
        var books = await _bookRepository.GetAllBooks();
            return Ok(books);
        }

        [HttpGet("{bookId}")]
        public async Task<IActionResult> GetBookById([FromRoute]int bookId)
        {
            var bookById = await _bookRepository.GetBookById(bookId);
            if (bookById == null) { return NotFound(); }
            return Ok(bookById);
        }

        [HttpPost("")]
        public async Task<IActionResult> AddBook([FromBody] BookModel bookModel)
        {
            var newBookId = await _bookRepository.AddBook(bookModel);
            //return Ok(newBook);
            return CreatedAtAction(
                nameof(GetBookById),
                new { id = newBookId, controller = "books" },
                bookModel);
        }


    }
}
