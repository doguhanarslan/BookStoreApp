import Header from "./components/Header";
import Home from "./pages/Home";
import "./App.css";
import Cart from "./pages/Cart";
import { useState } from "react";
import { BrowserRouter, Routes, Route } from "react-router";
import { StoreProvider } from "./context/StoreContext";
import Book from "./pages/Book";
function App() {


  

  return (
    <StoreProvider>
      <BrowserRouter>
        <Header />
        <Routes>
          <Route path="/" element={<Home  />} />
          <Route
            path="/cart"
            element={<Cart  />}
          />
          <Route path="/book/:bookTitle" element={<Book/>}/>
        </Routes>
      </BrowserRouter>
    </StoreProvider>
  );
}

export default App;
