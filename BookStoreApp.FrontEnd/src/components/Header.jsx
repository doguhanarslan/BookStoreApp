import { IoMdCart } from "react-icons/io";
import { FaSignInAlt,FaSignOutAlt } from "react-icons/fa";
import React, { useContext, useState } from "react";
import { useNavigate } from "react-router-dom";
import { StoreContext } from "../context/StoreContext";

function Header() {
  const { user, login, logout, query, setQuery } = useContext(StoreContext);
  const navigate = useNavigate();
  const [userNameInput, setUserNameInput] = useState("");
  const [passwordInput, setPasswordInput] = useState("");

  const handleInputChange = (e) => {
    setQuery(e.target.value);
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

  return (
    <header className="fixed top-0 left-0 w-full bg-black rounded-bl-lg rounded-br-lg text-white p-2 flex justify-between items-center shadow-md z-50">
      <div className="my-2 flex flex-row justify-between items-center">
        <button onClick={handleHomeClick}>
          <div className="bg-white hover:bg-black hover:text-white flex p-2 text-black border-black rounded-md">
            <h1 className="font-bold">Books</h1>
          </div>
        </button>
      </div>
      <div className="flex flex-row items-center justify-center text-center">
        <input
          type="text"
          value={query}
          onChange={handleInputChange}
          placeholder="Search for books..."
          className="p-2 m-1 rounded-lg w-[400px] text-center text-lg font-bold  text-black border-2 border-gray-300 focus:outline-none focus:border-blue-500 transition duration-300 ease-in-out"
        />
      </div>
      <div className="flex items-center">
        {user && (<>
          <button onClick={handleCartClick} className="bg-white hover:bg-gray-200 text-black p-2 rounded-lg mr-2 flex items-center">
            Cart
            <IoMdCart className="ml-1 text-black text-[14px]" />
          </button>
          <button onClick={handleLogoutClick} className="bg-white hover:bg-gray-200 text-black p-2 rounded-lg flex items-center">
          <FaSignOutAlt className="mr-1 text-black text-[14px]" />
          Logout
        </button>
        </>
        )}
        {!user && (
          <button onClick={handleLoginPageClick} className="bg-white hover:bg-gray-200 text-black p-2 rounded-lg flex items-center">
            <FaSignInAlt className="mr-1 text-black text-[14px]" />
            Login
          </button>
        )}
      </div>
    </header>
  );
}

export default Header;