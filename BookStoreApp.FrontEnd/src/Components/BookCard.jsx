import { useState, useEffect } from 'react'
import reactLogo from './assets/react.svg'
import viteLogo from '/vite.svg'
import './App.css'
import axios from 'axios';

function BookCard() {

    return (
      <div>
        <h1>Books</h1>
        <ul>
          {books.map((book) => (
            <li key={book.bookId}>
              {book.bookTitle} - {book.authorName}
            </li>
          ))}
        </ul>
      </div>
    );
}

export default App
