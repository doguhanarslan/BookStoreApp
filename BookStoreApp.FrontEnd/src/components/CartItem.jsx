import React from 'react'
import BookCard from './BookCard'

function CartItem({cartItem}) {
  return (
    <div className="rounded-xl overflow-hidden p-4 shadow-2xl items-center justify-center">
          <div className="flex items-center justify-center object-contain">
            {cartItem.bookImage ? (
              <img
                className="w-96 object-cover rounded"
                src={cartItem?.bookImage}
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
            <div className="font-bold text-xl mb-2">{cartItem.bookTitle}</div>
            <p className="text-gray-700 text-base">{cartItem.bookAuthor}</p>
          </div>
          <div className="flex text-wrap items-center justify-center mb-4">
            <p className="text-black font-serif">
              {cartItem.bookDescription}
            </p>
          </div>
          <div className="flex justify-between items-center text-center font-bold bg-white text-white rounded-lg">
            <p>Quantity: {cartItem.quantity}</p>
            <div>
              <p className="text-black text-[16px]">
                Price:{" "}
                <span className="text-[19px] font-thin">${cartItem.price}</span>{" "}
              </p>
            </div>
          </div>
        </div>
  )
}

export default CartItem