using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStoreApp.Business.Abstract;
using BookStoreApp.DataAccess.Abstract;
using BookStoreApp.Entities.ComplexTypes;
using BookStoreApp.Entities.Concrete;

namespace BookStoreApp.Business.Concrete.Managers
{
    public class BookManager:IBookService
    {
        private IBookDal _bookDal;
        public BookManager(IBookDal bookDal)
        {
            _bookDal = bookDal;
        }

        public List<Book> GetAllBooks()
        {
            return _bookDal.GetAll();
        }

        public Book Add(Book book)
        {
            return _bookDal.Add(book);
        }

        public Book Update(Book book)
        {
            return _bookDal.Update(book);
        }

        public Book? GetBookById(int id)
        {
            return _bookDal.Get(b => b.Id == id);
        }

        public void DeleteBookById()
        {
            throw new NotImplementedException();
        }
    }
}
