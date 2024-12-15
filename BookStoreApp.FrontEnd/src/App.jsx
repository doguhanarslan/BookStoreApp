import { useState, useEffect } from 'react'
import './App.css'
import axios from 'axios';

function App() {
  const [books, setBooks] = useState([])


    useEffect(() => {

       axios.get("http://localhost:5004/api/books").then((response)=>setBooks(response.data));

    },[])


    return (
      <div>
        <h1>Books</h1>
        <ul>
          {books.map((book) => (
            <li key={book.bookId}>
              {book.bookTitle} - {book.authorName}
              <br/>
                  {book.bookPrice}
                  <img src={book.bookImage } />
            </li>
          ))}
        </ul>
      </div>
    );
}

export default App
