using Api_BookButikk.Data;
using Api_BookButikk.Model;
using Api_BookButikk.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
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
        public async Task<IActionResult> GetBookById
            ([FromRoute]int bookId)
        {
            var bookById = await _bookRepository.GetBookById(bookId);
            if (bookById == null) { return NotFound(); }
            return Ok(bookById);
        }

        [HttpPost("")]
        public async Task<IActionResult> PostNewBook
            ([FromBody] BookModel bookModel)
        {
            var aNewBook = await _bookRepository.PostNewBook(bookModel);
            return CreatedAtAction(
                nameof(GetBookById),
                new { aNewBook, controller = "books" }, 
                bookModel.Id);//couldnt get id in response
        }

        [HttpPut("{bookId}")]
        public async Task<IActionResult> UpdateBook
            ([FromBody] BookModel bookModel, 
            [FromRoute]int bookId)
        {
            await _bookRepository.UpdateBook(bookId, bookModel);
            return Ok();
        }

        [HttpPatch("{bookId}")]
        public async Task<IActionResult> PatchTheBook
           ([FromBody] JsonPatchDocument bookModel,
           [FromRoute] int bookId)
        {
            await _bookRepository.PatchTheBook(bookId, bookModel);
            return Ok();
        }






    }
}
