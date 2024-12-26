import Header from "./components/Header";
import Home from "./pages/Home";
import "./App.css";
import Cart from "./pages/Cart";
import { useState } from "react";
import { BrowserRouter, Routes, Route } from "react-router";
import { StoreProvider } from "./context/StoreContext";
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
        </Routes>
      </BrowserRouter>
    </StoreProvider>
  );
}

export default App;
