using BookStoreApp.Entities.ComplexTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStoreApp.Business.DTOs;

namespace BookStoreApp.Business.Abstract
{
    public interface IElasticsearchService
    {
        Task IndexBookAsync(BookDetails book);
        Task<IEnumerable<BookDetails>> SearchBooksAsync(string query);
    }
}
