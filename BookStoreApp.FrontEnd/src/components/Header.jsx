import { IoMdCart } from "react-icons/io";
import { FaSignInAlt, FaSignOutAlt } from "react-icons/fa";
import React, { useContext, useState } from "react";
import { useNavigate } from "react-router-dom";
import { StoreContext } from "../context/StoreContext";
import logo from "../assets/booklogo.png";

function Header() {
  const { user, logout, query, setQuery, categories,fetchBooksByCategory } = useContext(StoreContext);
  const navigate = useNavigate();
  const [userNameInput, setUserNameInput] = useState("");
  const [passwordInput, setPasswordInput] = useState("");

  const handleInputChange = (e) => {
    setQuery(e.target.value);
    console.log(user);
  };

  const handleLogoutClick = () => {
    logout(user.userName);
  };

  const handleCartClick = () => {
    navigate("/cart");
    window.location.reload();
  };

  const handleHomeClick = () => navigate("/");

  const handleLoginPageClick = () => navigate("/login");

  const handleSignupPageClick = () => navigate("/signup");

  const handleCategoryClick = (categoryId) => {
    fetchBooksByCategory(categoryId);
  };

  const profileImageUrl = user ? `https://localhost:7118${user.profileImage}` : '';

  return (
    <header className="fixed top-0 left-0 w-full bg-white border-b-red-500 border-2 rounded-bl-3xl rounded-br-3xl text-white p-2 flex justify-between items-center shadow-md z-50">
      <div className="my-2 flex flex-row justify-between items-center">
        <button onClick={handleHomeClick}>
          <div className="hover:shadow-xl p-2 hover:duration-500 hover:border-b-[0.1em] hover:border-red-500 rounded-md">
            <img src={logo} className="h-12 w-12" />
          </div>
        </button>
      </div>
      <div className="flex flex-row items-center justify-center text-center">
        <input
          type="text"
          value={query}
          onChange={handleInputChange}
          placeholder="Search for books..."
          className="p-2 m-1 rounded-lg w-[400px] text-center text-lg font-bold text-black border-2 border-gray-300 focus:outline-none focus:border-blue-500 transition duration-300 ease-in-out"
        />
      </div>
      <div className="flex items-center font-serif text-[18px] space-x-4">
        {categories.map((category) => (
          <button
            key={category.categoryId}
            onClick={() => handleCategoryClick(category.categoryId)}
            className="hover:shadow-lg px-4 py-2 rounded-md bg-white text-black hover:border-b-[0.1em] hover:border-b-red-600 transition duration-300"
          >
            {category.categoryName}
          </button>
        ))}
      </div>
      <div className="flex items-center">
        {user && (
          <>
            <button onClick={handleCartClick} className="bg-white hover:bg-gray-200 text-black p-2 rounded-lg mr-2 flex items-center">
              Cart
              <IoMdCart className="ml-1 text-black text-[14px]" />
            </button>
            <button onClick={handleLogoutClick} className="bg-white hover:bg-gray-200 text-black p-2 rounded-lg flex items-center">
              <FaSignOutAlt className="mr-1 text-black text-[14px]" />
              Logout
            </button>
            <img
              src={profileImageUrl}
              alt="Profile"
              className="h-10 w-10 rounded-full ml-2"
            />
          </>
        )}
        {!user && (
          <>
            <button onClick={handleSignupPageClick} className="bg-white hover:shadow-2xl hover:border-b-[0.1em] duration-300 hover:border-b-red-600 text-red-500 hover:duration-150 p-2 rounded-lg flex items-center">
              <FaSignInAlt className="mr-1 text-red-500 text-[16px]" />
              Signup
            </button>
            <button onClick={handleLoginPageClick} className="bg-white hover:shadow-2xl hover:border-b-[0.1em] duration-300 hover:border-b-red-600 text-red-500 hover:duration-150 p-2 rounded-lg flex items-center">
              <FaSignInAlt className="mr-1 text-red-500 text-[16px]" />
              Login
            </button>
          </>
        )}
      </div>
    </header>
  );
}

export default Header;