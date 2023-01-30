using Api_BookButikk.Data;
using Api_BookButikk.Model;
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
        public async Task<int> AddNewBook(BookModel bookModel)
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
            var book2update=await _context.Books.FindAsync(bookId);
            if (book2update != null)
            {
                book2update.Title = bookModel.Title;
                book2update.Description = bookModel.Description;
            }

            await _context.SaveChangesAsync();  
        }

    }
}
