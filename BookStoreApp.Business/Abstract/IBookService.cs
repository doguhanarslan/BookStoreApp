using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStoreApp.Entities.ComplexTypes;
using BookStoreApp.Entities.Concrete;

namespace BookStoreApp.Business.Abstract
{
    public interface IBookService
    {
        List<BookDetails> GetAllBooks();

        Book Add(Book book);

        Book Update(Book book);

        BookDetails GetBookByName(string name);
        BookDetails GetBookById(int bookId);
        void DeleteBookById();
    }
}
