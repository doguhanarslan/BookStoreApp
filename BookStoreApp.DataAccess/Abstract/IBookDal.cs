using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStoreApp.Core.DataAccess;
using BookStoreApp.Entities.ComplexTypes;
using BookStoreApp.Entities.Concrete;

namespace BookStoreApp.DataAccess.Abstract
{
    public interface IBookDal:IEntityRepository<Book>
    {
        List<BookDetails> GetAllBooks();

        BookDetails GetBookById(int bookId);

        List<BookDetails> GetBookByCategory(int categoryId);

        BookDetails GetBookByName(string name);

    }
}
