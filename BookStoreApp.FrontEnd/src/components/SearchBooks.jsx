import React, { useState, useEffect, useContext } from 'react';
import axios from 'axios';
import BookCard from './BookCard';
import { StoreContext } from '../context/StoreContext';

function SearchBooks() {
  const {books,query,setQuery} = useContext(StoreContext);
  

  

  return (
    <div className="w-full h-full mx-auto p-6 bg-white shadow-md rounded-lg text-center flex items-center justify-center flex-col">
      
      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-4">
        {books.map((book) => (
          <BookCard key={book.bookId} book={book} />
        ))}
      </div>
    </div>
  );
}

export default SearchBooks;