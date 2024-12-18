import React from "react";
import { useState, useEffect } from "react";
import axios from "axios";
import BookCard from "../components/BookCard";
function Home({user}) {
  const [books, setBooks] = useState([]);
  

  useEffect(() => {
    const fetchBooks = async () => {
      try {
        await axios.get("https://localhost:7118/api/Books").then((response) => {
          setBooks(response.data);
          console.log(response.data);
        });
      } catch (error) {
        console.log(error);
      }
    };

    fetchBooks();
  }, []);

  return (
    <div className="flex flex-col items-center justify-center gap-8 px-4 py-6">
      <div className="grid gap-6 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4">
        {books.map((book, index) => (
          <BookCard user={user} key={index} book={book} />
        ))}
      </div>
    </div>
  );
}

export default Home;
