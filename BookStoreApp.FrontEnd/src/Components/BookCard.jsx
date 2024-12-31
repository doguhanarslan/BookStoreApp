import axios from "axios";
import React, { useContext, useState, useEffect } from "react";
import { IoMdCart } from "react-icons/io";
import { useNavigate } from 'react-router-dom';
import { StoreContext } from "../context/StoreContext";
import { v4 as uuidv4 } from 'uuid';
import { FaStar, FaStarHalfAlt, FaRegStar } from 'react-icons/fa';

function BookCard({ book }) {
  const { user, cartItems, setCartItems } = useContext(StoreContext);
  const [quantity, setQuantity] = useState(1);
  const [notifications, setNotifications] = useState([]);
  const [isInCart, setIsInCart] = useState(false);
  const navigate = useNavigate();

  useEffect(() => {
    setIsInCart(cartItems.some((item) => item.bookId === book.bookId));
  }, [cartItems, book.bookId]);

  const addToCart = async () => {
    try {
      const response = await axios.post(
        `https://localhost:7118/api/Carts/add?bookId=${book.bookId}&userId=${user.id}&quantity=${quantity}`
      );
      setCartItems([...cartItems, response.data]);
      setIsInCart(true); // Butonun metnini güncelle
      const notificationId = uuidv4();
      setNotifications((prevNotifications) => [
        ...prevNotifications,
        { id: notificationId, message: 'Ürün sepete eklendi!' }
      ]);
      setTimeout(() => {
        setNotifications((prevNotifications) =>
          prevNotifications.filter((notification) => notification.id !== notificationId)
        );
      }, 3000); // 3 saniye sonra uyarıyı gizle
    } catch (error) {
      console.log(error);
    }
  };

  const handleQuantityClick = (e) => {
    const value = e.currentTarget.value;
    if (value === "+") {
      setQuantity(quantity + 1);
    } else if (value === "-" && quantity > 1) {
      setQuantity(quantity - 1);
    }
  };

  const convertToSlug = (text) => {
    const trMap = {
      'çÇ': 'c',
      'ğĞ': 'g',
      'şŞ': 's',
      'üÜ': 'u',
      'ıİ': 'i',
      'öÖ': 'o'
    };
    for (let key in trMap) {
      text = text.replace(new RegExp('[' + key + ']', 'g'), trMap[key]);
    }
    return text.replace(/\s+/g, '-').toLowerCase();
  };

  const handleViewBook = () => {
    const slug = convertToSlug(book.bookTitle);
    navigate(`/book/${slug}`, { state: { book } });
  };

  const renderStars = (rating) => {
    const stars = [];
    for (let i = 1; i <= 5; i++) {
      if (i <= rating) {
        stars.push(<FaStar key={i} className="text-yellow-500" />);
      } else if (i === Math.ceil(rating) && !Number.isInteger(rating)) {
        stars.push(<FaStarHalfAlt key={i} className="text-yellow-500" />);
      } else {
        stars.push(<FaRegStar key={i} className="text-yellow-500" />);
      }
    }
    return stars;
  };

  return (
    <div className="max-w-md mx-auto bg-white shadow-lg rounded-lg overflow-hidden flex transition-transform transform hover:scale-105">
      <div className="w-1/3 h-auto">
        <img
          className="w-full h-full object-contain bg-gray-200"
          src={book.bookImage}
          alt={book.bookTitle}
        />
      </div>
      <div className="p-4 flex flex-col justify-between w-2/3">
        <div>
          <h2 className="text-xl font-bold text-gray-800">{book.bookTitle}</h2>
          <p className="text-gray-600 mt-2 text-sm">{book.bookDescription}</p>
          <p className="text-gray-800 mt-2 text-sm">
            <span className="font-semibold">Author:</span> {book.authorName}
          </p>
          <p className="text-gray-800 mt-2 text-sm">
            <span className="font-semibold">Price:</span> ${book.bookPrice * quantity}
          </p>
          <div className="flex items-center mt-2">
            {renderStars(book.bookRate)}
            <span className="ml-2 text-gray-800 text-sm">({book.bookRate})</span>
            <span className="ml-2 text-gray-800 text-sm">({book.reviewCount} reviews)</span>
          </div>
        </div>
        <div className="flex items-center mt-4">
          <button
            value="-"
            onClick={handleQuantityClick}
            className="text-center text-[12px] hover:bg-gray-200 hover:text-black hover:duration-500 items-center justify-center bg-gray-300 text-black px-[8px] rounded-full"
          >
            -
          </button>
          <p className="mx-2 font-bold text-center text-black">{quantity}</p>
          <button
            value="+"
            onClick={handleQuantityClick}
            className="text-center text-[12px] hover:bg-gray-200 hover:text-black hover:duration-500 items-center justify-center bg-gray-300 text-black px-[8px] rounded-full"
          >
            +
          </button>
        </div>
        <div className="flex items-center justify-between mt-4">
          <button
            onClick={addToCart}
            className="flex items-center justify-center bg-black text-white font-bold py-1 px-2 rounded hover:bg-gray-800 transition-colors duration-300 text-sm"
          >
            {isInCart ? "In Cart" : "Add to Cart"}
            <IoMdCart className="ml-1 text-white text-[14px]" />
          </button>
          <button
            onClick={handleViewBook}
            className="flex items-center justify-center bg-gray-800 text-white font-bold py-1 px-2 rounded hover:bg-black transition-colors duration-300 text-sm"
          >
            İncele
          </button>
        </div>
      </div>
      <div className="fixed top-20 right-4 space-y-4 z-50">
        {notifications.map((notification) => (
          <div key={notification.id} className="bg-green-500 text-white py-2 px-4 rounded-lg shadow-lg animate-bounce">
            {notification.message}
          </div>
        ))}
      </div>
    </div>
  );
}

export default BookCard;