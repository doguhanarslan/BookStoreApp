import Header from './components/Header'
import Home from './pages/Home'
import "./App.css";
import Cart from './pages/Cart';
import {useEffect,useState} from 'react';
import { BrowserRouter, Routes, Route } from "react-router";
import axios from 'axios';
function App() {

  
  const [userNameInput, setUserNameInput] = useState("");
  const [passwordInput, setPasswordInput] = useState("");
  const [user, setUser] = useState({});
  const [cartItem, setCartItem] = useState([]);
  const handleUserNameChange = (e) => {
    setUserNameInput(e.target.value);
    console.log(e.target.value);
  };

  const handlePasswordChange = (e) => {
    setPasswordInput(e.target.value);
    console.log(e.target.value);
  };

  const handleLoginClick = async () => {
    await userLogin();
  };
  const userLogin = async () => {
    await axios
      .get(
        `https://localhost:7118/api/Users?userName=${userNameInput}&password=${passwordInput}`
      )
      .then((response) => {
        setUser(response.data);
        console.log(response.data);
      })
      .catch((error) => {
        console.log(error);
      });
  };

  const fetchUser = async ()=>{
    try {
      await axios
        .get(`https://localhost:7118/api/Users/${user.id}`)
        .then((response) => {
          setUser(response.data);
          console.log(response.data);
        });
    } catch (error) {
        console.log(error);
    }
  }
  
  

  return(
    <BrowserRouter>
    <Header handleLoginClick={handleLoginClick} handlePasswordChange={handlePasswordChange} handleUserNameChange={handleUserNameChange} user={user} setUser={setUser} userNameInput={userNameInput} passwordInput={passwordInput}/>
      <Routes>
        <Route path="/" element={<Home user={user} />} />
        <Route path="/cart" element={<Cart cartItem={cartItem} user={user} />} />
      </Routes>
    </BrowserRouter>
  )

 
}

export default App
