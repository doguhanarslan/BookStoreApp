import axios from "axios";
import React, { useContext, useState } from "react";
import { IoMdCart } from "react-icons/io";
import { StoreContext } from "../context/StoreContext";
function BookCard({ book }) {
  const { user, cartItems } = useContext(StoreContext);
  const [quantity, setQuantity] = useState(1);
  const addToCart = async () => {
    try {
      await axios
        .post(
          `https://localhost:7118/api/Carts/add?bookId=${book.bookId}&userId=${user.id}&quantity=${quantity}`
        )
        .then((response) => {
          console.log(response.data);
        });
    } catch (error) {
      console.log(error);
    }
  };

  const isInCart = cartItems.some((item) => item.bookId == book.bookId);

  const handleQuantityClick = (e) => {
    let value = e.currentTarget.value;
    if (value === "+") {
      setQuantity(quantity + 1);
    } else if (value === "-" && quantity > 1) {
      setQuantity(quantity - 1);
    }
  };

  return (
    <div className="rounded-xl overflow-hidden p-4 shadow-2xl items-center justify-center">
      <div className="flex items-center justify-center object-contain">
        {book.bookImage ? (
          <img
            className="w-64 object-cover rounded"
            src={book?.bookImage}
            alt="poster"
          />
        ) : (
          <img
            className="w-64 object-cover"
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
        <p className="text-black font-serif">{book.bookDescription}</p>
      </div>
      <div className="flex justify-between items-center text-center font-bold bg-white text-white rounded-lg">
        {isInCart ? (
          <button
            onClick={addToCart}
            className="text-center hover:bg-white hover:text-black hover:duration-500 items-center justify-center flex flex-row text-nowrap gap-2  bg-black rounded-lg w-[45%] p-1 text-white py-2"
          >
            Sepetinizde
            <IoMdCart className="flex text-white hover:text-black text-[18px]" />
          </button>
        ) : (
          <button
            onClick={addToCart}
            className="text-center hover:bg-white hover:text-black hover:duration-500 items-center justify-center flex flex-row text-nowrap gap-2  bg-black rounded-lg w-[45%] p-1 text-white py-2"
          >
            Add to Cart
            <IoMdCart className="flex text-white text-[18px] hover:text-black" />
          </button>
        )}

        <div className="flex flex-row items-center justify-between gap-2">
          <div>
            <button
              value={"-"}
              onClick={handleQuantityClick}
              className="text-center text-[15px] hover:bg-white hover:text-black hover:duration-500 items-center justify-center  bg-black text-white px-[10px] rounded-full"
            >
              -
            </button>
          </div>
          <div className="flex flex-row">
            <p className="font-bold text-center text-black">{quantity}</p>
          </div>
          <div>
            <button
              value={"+"}
              onClick={handleQuantityClick}
              className="text-center text-[15px] hover:bg-white hover:text-black hover:duration-500 items-center justify-center  bg-black text-white px-[10px] rounded-full"
            >
              +
            </button>
          </div>
        </div>
        <div>
          <p className="text-black text-[16px]">
            Price:{" "}
            <span className="text-[19px] font-thin">
              ${book.bookPrice * quantity}
            </span>{" "}
          </p>
        </div>
      </div>
    </div>
  );
}

export default BookCard;
