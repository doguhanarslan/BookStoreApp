import React, { useContext } from "react";
import { StoreContext } from "../context/StoreContext";
import axios from "axios";

function CartItem({ cartItem }) {
  const { user, cartItems, setCartItems } = useContext(StoreContext);

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
    <div className="rounded-xl overflow-hidden p-6 shadow-2xl bg-gradient-to-br from-gray-50 to-gray-100 hover:shadow-3xl transition-shadow duration-300">
      <div className="flex justify-end mb-4">
        <button
          onClick={removeFromCart}
          className="bg-red-500 hover:bg-red-600 text-white font-semibold py-2 px-4 rounded-lg transition-colors duration-300"
        >
          Remove Item
        </button>
      </div>
      <div className="flex justify-center mb-6">
        <img
          className="w-48 h-64 object-cover rounded-lg shadow-md"
          src={cartItem.bookImage || "https://placehold.co/600x900/png"}
          alt="book cover"
        />
      </div>
      <div className="text-center mb-6">
        <h2 className="text-2xl font-bold text-gray-800 mb-2">{cartItem.bookTitle}</h2>
        <p className="text-gray-600 text-lg">{cartItem.bookAuthor} | {cartItem.categoryName}</p>
      </div>
      <div className="flex justify-between items-center bg-white p-4 rounded-lg shadow-inner">
        <p className="text-gray-700 font-medium">Quantity: {cartItem.quantity}</p>
        <p className="text-gray-700 font-medium">
          Price: <span className="text-green-600 font-bold">${cartItem.price}</span>
        </p>
      </div>
    </div>
  );
}

export default CartItem;