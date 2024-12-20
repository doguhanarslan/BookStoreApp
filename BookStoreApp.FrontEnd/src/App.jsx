import Header from "./components/Header";
import Home from "./pages/Home";
import "./App.css";
import Cart from "./pages/Cart";
import { useEffect, useState } from "react";
import { BrowserRouter, Routes, Route } from "react-router";
import axios from "axios";
function App() {
  const [userNameInput, setUserNameInput] = useState("");
  const [passwordInput, setPasswordInput] = useState("");
  const [user, setUser] = useState(null);
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
    await login(userNameInput, passwordInput);
  };
  const handleLogoutClick = async () => {
    await logout(user.userName);
  };
  const login = async (username, password) => {
    try {
      const response = await axios.post(
        "https://localhost:7118/api/Users/login",
        {
          username,
          password,
        },
        {
          headers: {
            'accept': '*/*',
            "Content-Type": "application/json",
          },
          withCredentials: true, // Cookie'lerin gönderilmesi ve alınması için gerekli
        }
      );
      fetchUser();
      console.log("Login successful:", response.data);
      window.location.reload();
      return response.data;
    } catch (error) {
      console.error("Login failed:", error.response?.data || error.message);
      throw error;
    }
  };

  const fetchUser = async () => {
    try {
      await axios
        .get(`https://localhost:7118/api/Users/loggedUser`, {
          withCredentials: true,
          headers: {
            accept: "*/*", // Accept başlığını ekle
          },
        })
        .then((response) => {
          if(response.status === 404){
            setUser(null);
            console.log('No user logged in');
          }
          setUser(response.data.user);
          console.log(response.data);
        });
    } catch (error) {
      console.log(error);
    }
  };
  useEffect(() => {
    
    fetchUser();
  }, []);


  const logout = async (username) =>{
    try {
      const response = await axios.post(`https://localhost:7118/api/Users/logout`,{username}, {
        withCredentials: true,
        headers: {
          accept: "*/*", // Accept başlığını ekle
        },
      });
      if(response.status === 200){
        setUser(null);
        console.log('Logout successful');
          // localStorage veya sessionStorage'daki kullanıcı bilgilerini temizleyin
          localStorage.clear();
          sessionStorage.clear();
          window.location.reload();
          // Kullanıcıyı giriş sayfasına yönlendirin
      }
    } catch (error) {
      console.log(error);
    }
  };




  return (
    <BrowserRouter>
      <Header
        handleLoginClick={handleLoginClick}
        handlePasswordChange={handlePasswordChange}
        handleUserNameChange={handleUserNameChange}
        handleLogoutClick={handleLogoutClick}
        user={user}
        setUser={setUser}
        userNameInput={userNameInput}
        passwordInput={passwordInput}
      />
      <Routes>
        <Route path="/" element={<Home user={user} />} />
        <Route
          path="/cart"
          element={<Cart cartItem={cartItem} user={user} />}
        />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
