import React from 'react'
import BookCard from '../components/BookCard';
import {useEffect,useState} from 'react';
import axios from 'axios';
import CartItem from '../components/CartItem';
function Cart({user}) {

  const [cartItems, setCartItems] = useState([]);
  const fetchCartItems = async () => {
    try {
      await axios
        .get(`https://localhost:7118/api/Carts/items?userId=${user.id}`)
        .then((response) => {
          setCartItems(response.data);
          console.log(response.data);
        });
    } catch (error) {
        console.log(error);
    }
  }
  useEffect(() => {
    fetchCartItems();
  }, []);

  return (
    <div>
      {cartItems.map((cartItem, index) => {
        return <CartItem key={index} cartItem={cartItem} />;
      })}
    </div>
  )
}

export default Cart