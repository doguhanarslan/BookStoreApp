import React, { useContext } from "react";
import { useState, useEffect } from "react";
import axios from "axios";
import BookCard from "../components/BookCard";
import { StoreContext } from "../context/StoreContext";
import SearchBooks from "../components/SearchBooks";
function Home() {
  
  const {books,query} = useContext(StoreContext);

  return (
    <div className="flex flex-col items-center mt-10 justify-center gap-8 px-4 py-6">
      {query != '' ?<div className="flex items-center justify-center mx-auto w-full h-full"> <SearchBooks/></div> : <div className="grid gap-6 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4">
        {books.map((book, index) => (
          <BookCard key={index} book={book} />
        ))}
      </div>}
    </div>
  );
}

export default Home;
