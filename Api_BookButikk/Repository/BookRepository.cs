using Api_BookButikk.Data;
using Api_BookButikk.Model;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Api_BookButikk.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly BookButikkDbContext _context;

        public BookRepository(BookButikkDbContext context)
        {
            _context = context;
        }

        
        //since this is an asyn method, the return expression must be a type of <list<books>>, 
        //rather than Task<List<Books>>

        //list of Books, need to be converted to bookmodel
        //cannot convert genericlist<data.books> to genericlist<model.bookmodel>
        //howeve this hardcoding conversion is not ideal when there are many records available.
        //best practice is using mapper

        //var records=_context.Books.ToListAsync();

        //data was manually inserted in mssql server
        public async Task<List<BookModel>> GetAllBooks()
        { 
            var records = await _context.Books.Select(b => new BookModel()
            {
                Id = b.Id,
                Title = b.Title,
                Description = b.Description
            }).ToListAsync();

            return records;
        }

        public async Task<BookModel> GetBookById(int bookId)
        {
            //to fetch data based on other column, use where
            var bookById = await _context.Books
                .Where(b=>b.Id == bookId).Select(b => new BookModel()
            {
                Id = b.Id,
                Title = b.Title,
                Description = b.Description
            }).FirstOrDefaultAsync();

            return bookById;
        }

        //we're adding a book to database, but database doesnt understand bookmodel.
        //interprete bookmodel type to book type

        //public async Task<BookModel> AddBook(BookModel bookModel)
        public async Task<int> PostNewBook(BookModel bookModel)
        {
            var newBook = new Books()
            {
                Title = bookModel.Title,
                Description = bookModel.Description
            };

            _context.Books.Add(newBook);
            await _context.SaveChangesAsync();
            return newBook.Id;
        }

       
        public async Task UpdateBook(int bookId, BookModel bookModel)
        {
            //var book2update=await _context.Books.FindAsync(bookId);
            //if (book2update != null)
            //{
            //    book2update.Title = bookModel.Title;
            //    book2update.Description = bookModel.Description;
            //}
            
            //above we reach database 2 times for one update, best practice will be:
            var newBook = new Books()
            {
                Id= bookId,
                Title = bookModel.Title,
                Description = bookModel.Description
            };

            _context.Books.Update(newBook);
            await _context.SaveChangesAsync();
        }


        //put updates all data of a record. if one property is updated but not the others, they all erase
        //patch updates only wanted properties (rows of table), doesnt touch other props
        //jsonpatch and newtonsjon packes are loaded for patch method
        public async Task PatchTheBook(int bookId, JsonPatchDocument bookModel)
        { 
        var book = await _context.Books.FindAsync(bookId);
            if (book!=null) 
            {
            bookModel.ApplyTo(book);
            await _context.SaveChangesAsync();
            }
        
        }

        public async Task DeleteABook(int bookId)
        {
            //var book2delete = await _context.Books.FindAsync(bookId);
            
            //insteadof reaching db and finding id,
            //create the object with id and send to delete/remove
            var book2delete = new Books() { Id=bookId};

             _context.Books.Remove(book2delete);
            await _context.SaveChangesAsync();
        }

    }
}
