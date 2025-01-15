import React, { useContext, useState } from "react";
import { StoreContext } from "../context/StoreContext";
import axios from "axios";
function CartItem({ cartItem }) {
  const { user,cartItems,setCartItems } = useContext(StoreContext);
  const removeFromCart = async () => {
    try {
      await axios.delete(
        `https://localhost:7118/api/Carts/removeFromCart?bookId=${cartItem.bookId}&userId=${user.id}`,
        {
          withCredentials: true,
          headers: {
            accept: "*/*",
          },
        }
      );
      setCartItems(cartItems.filter(item => item.bookId !== cartItem.bookId));
    } catch (error) {
      console.log(error);
    }
  };

  return (
    <div className="rounded-xl overflow-hidden p-4 shadow-2xl items-center justify-center">
      <div className="flex items-center justify-end mb-2">
        <button
          onClick={removeFromCart}
          className="bg-black hover:bg-red-600 p-2 text-white duration-500 font-bold rounded-lg"
        >
          Remove Item
        </button>
      </div>
      <div className="flex items-center justify-center object-contain">
        {cartItem.bookImage ? (
          <img
            className="w-72 object-cover rounded"
            src={cartItem?.bookImage}
            alt="poster"
          />
        ) : (
          <img
            className="w-78 object-cover"
            src={`https://placehold.co/600x900/png`}
            alt="poster"
          />
        )}
      </div>
      <div className="px-6 py-4 flex items-center justify-center flex-col">
        <div className="font-bold text-xl mb-2">{cartItem.bookTitle}</div>
        <p className="text-gray-700 text-base">{cartItem.bookAuthor}</p>
      </div>
      
      <div className="flex justify-between items-center text-center font-bold bg-white text-black rounded-lg">
        <p>Quantity: {cartItem.quantity}</p>
        <div>
          <p className="text-black text-[16px]">
            Price:{" "}
            <span className="text-[19px] font-thin">${cartItem.price}</span>{" "}
          </p>
        </div>
      </div>
    </div>
  );
}

export default CartItem;
