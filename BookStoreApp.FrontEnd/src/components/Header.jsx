import React from "react";
import { IoMdCart } from "react-icons/io";
import {useNavigate} from 'react-router-dom';
function Header() {
    const navigate = useNavigate();

    const handleCartClick = () => {
      navigate("/cart");
    };
    const handleHomeClick = ()=>{
        navigate("/");
    }

  return (
    <header className="fixed top-0 left-0 w-full bg-black rounded-bl-lg rounded-br-lg text-white p-2 flex justify-between items-center shadow-md z-50">
      <div className="my-2 flex flex-row justify-between items-center">
        <button onClick={handleHomeClick}>
        <div className="bg-white hover:bg-black hover:text-white flex p-2 text-black border-black rounded-md">
          <h1 className="font-bold">Books</h1>
        </div>
        </button>
      </div>
            <button onClick={handleCartClick} >
        <div className="p-1 gap-2 flex-row items-center justify-center flex hover:bg-white hover:text-black rounded-2xl hover:duration-500">
            <p className="text-[18px]">Cart</p>
            <IoMdCart className="text-[28px]"/>
        </div>
            </button>
    </header>
  );
}

export default Header;
