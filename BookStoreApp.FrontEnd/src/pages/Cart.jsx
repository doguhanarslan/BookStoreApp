import React, { useContext } from 'react'
import CartItem from '../components/CartItem';
import { StoreContext } from '../context/StoreContext';
function Cart() {

  const {user,cartItems,setCartItems} = useContext(StoreContext);
  
  return (
    <div className='flex flex-row gap-10'>
      {user ? cartItems.map((cartItem, index) => {
        return <CartItem key={index} cartItem={cartItem} />;
      }): <div>
        <h1>Lütfen giriş yapın.</h1>
        </div>}
      
    </div>
  )
}

export default Cart