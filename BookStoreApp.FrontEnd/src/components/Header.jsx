import React from "react";
import { IoMdCart } from "react-icons/io";
import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";
function Header({
  userNameInput,
  passwordInput,
  user,
  setUser,
  handleUserNameChange,
  handlePasswordChange,
  handleLoginClick,
  handleLogoutClick,
}) {
  const navigate = useNavigate();

  const handleCartClick = () => {
    navigate("/cart");
  };
  const handleHomeClick = () => {
    navigate("/");
  };

  return (
    <header className="fixed top-0 left-0 w-full bg-black rounded-bl-lg rounded-br-lg text-white p-2 flex justify-between items-center shadow-md z-50">
      <div className="my-2 flex flex-row justify-between items-center">
        <button onClick={handleHomeClick}>
          <div className="bg-white hover:bg-black hover:text-white flex p-2 text-black border-black rounded-md">
            <h1 className="font-bold">Books</h1>
          </div>
        </button>
      </div>
        {!user && (
      <div>
        <input
          onChange={handleUserNameChange}
          type="text"
          placeholder="Username"
          value={userNameInput}
          className="p-1 m-1 rounded-lg text-black"
        />
        <input
          onChange={handlePasswordChange}
          type="password"
          placeholder="Password"
          value={passwordInput}
          className="p-1 m-1 rounded-lg text-black"
        />
          <button
          onClick={handleLoginClick}
          className="p-1 m-1 bg-white text-black rounded-lg"
        >
          Login
        </button>
      </div>
      )}
        <div className="flex flex-row gap-3">
          {user && 
          <div className="text-white gap-3 flex flex-row justify-center items-center">
            <div>
            <button
            onClick={handleLogoutClick}
            className="p-1 m-1 bg-white text-black rounded-lg"
          >
            Logout
          </button>
            </div>
            <div>
              <img className="w-14 rounded-full h-14" src={user.profileImage} />
            </div>
            <div className="text-[15px] font-bold text-white flex flex-row justify-between gap-1 items-center">
              <p>{user.firstName}</p>
              <p>{user.lastName}</p>
            </div>
          </div>
          }
      <button onClick={handleCartClick}>
          <div className="p-1 gap-2 flex-row items-center justify-center flex hover:bg-white hover:text-black rounded-2xl hover:duration-500">
            <p className="text-[18px]">Cart</p>
            <IoMdCart className="text-[28px]" />
          </div>
      </button>
        </div>
    </header>
  );
}

export default Header;
