import axios from "axios";
import React, { useState } from "react";
import { IoMdCart } from "react-icons/io";
function BookCard({book,user}) {
  

  const addToCart = async () => {
    try {
      await axios
        .post(`https://localhost:7118/api/Carts/add?bookId=${book.bookId}&userId=${user.id}`)
        .then((response) => {
          console.log(response.data);
        });
    } catch (error) {
        console.log(error);
    }
  };

  return (
    <div className="rounded-xl overflow-hidden p-4 shadow-2xl items-center justify-center">
      <div className="flex items-center justify-center object-contain">
        {book.bookImage ? (
          <img
            className="w-96 object-cover rounded"
            src={book?.bookImage}
            alt="poster"
          />
        ) : (
          <img
            className="w-96 object-cover"
            src={`https://placehold.co/600x900/png`}
            alt="poster"
          />
        )}
      </div>
      <div className="px-6 py-4 flex items-center justify-center flex-col">
        <div className="font-bold text-xl mb-2">{book.bookTitle}</div>
        <p className="text-gray-700 text-base">{book.authorName}</p>
      </div>
      <div className="flex text-wrap items-center justify-center mb-4">
        <p className="text-black font-serif">
          {book.bookDescription}
        </p>
      </div>
      <div className="flex justify-between items-center text-center font-bold bg-white text-white rounded-lg">
        <button onClick={addToCart} className="text-center hover:bg-white hover:text-black hover:duration-500 items-center justify-center flex fles-row text-nowrap gap-2  bg-black rounded-lg w-[45%] p-1 text-white py-2">
          Add to Cart
          <IoMdCart className="flex text-white text-[18px]" />
        </button>
        <div>
          <p className="text-black text-[16px]">
            Price:{" "}
            <span className="text-[19px] font-thin">${book.bookPrice}</span>{" "}
          </p>
        </div>
      </div>
    </div>
  );
}

export default BookCard;
