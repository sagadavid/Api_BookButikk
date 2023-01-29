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

        public async Task<List<BookModel>> GetAllBooks()
        
        //since this is an asyn method, the return expression must be a type of <list<books>>, 
        //rather than Task<List<Books>>
        {
            //list of Books, need to be converted to bookmodel
            //cannot convert genericlist<data.books> to genericlist<model.bookmodel>
        //var records=_context.Books.ToListAsync();

            //data manually inserted in mssql server
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
                .Where(b=>b.Id== bookId).Select(b => new BookModel()
            {
                Id = b.Id,
                Title = b.Title,
                Description = b.Description
            }).FirstOrDefaultAsync();

            return bookById;
        }


    }
}
