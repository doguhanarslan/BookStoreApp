import React, { useContext } from 'react';
import { StoreContext } from '../context/StoreContext';
import CartItem from '../components/CartItem';

function Cart() {
  const { user, cartItems } = useContext(StoreContext);

  return (
    <div className="flex flex-col items-center justify-center min-h-screen bg-gray-100 p-4">
      {!user ? (
        <div className="bg-white p-6 rounded-lg shadow-md text-center">
          <h1 className="text-2xl font-bold text-red-500">Lütfen giriş yapın.</h1>
        </div>
      ) : cartItems.length === 0 ? (
        <div className="bg-white p-6 rounded-lg shadow-md text-center">
          <h1 className="text-2xl font-bold text-gray-700">Sepet boş</h1>
        </div>
      ) : (
        <div className="flex flex-row gap-10">
          {cartItems.map((cartItem, index) => (
            <CartItem key={index} cartItem={cartItem} />
          ))}
        </div>
      )}
    </div>
  );
}

export default Cart;