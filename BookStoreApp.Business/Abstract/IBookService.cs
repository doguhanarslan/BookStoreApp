﻿using System;
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
        List<Book> GetAllBooks();

        Book Add(Book book);

        Book Update(Book book);

        Book GetBookByName(string name);
        Book? GetBookById(int bookId);
        void DeleteBookById();
    }
}