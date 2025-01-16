import { createContext, useState, useEffect } from "react";
import axios from "axios";

// Context oluşturma
export const StoreContext = createContext();

// Provider bileşeni
export const StoreProvider = ({ children }) => {
  const [user, setUser] = useState(null);
  const [books, setBooks] = useState([]);
  const [cartItems, setCartItems] = useState([]);
  const [query, setQuery] = useState('');

  // Kullanıcı bilgilerini API'den al
  const fetchUser = async () => {
    try {
      const response = await axios.get(
        "https://localhost:7118/api/Users/loggedUser",
        {
          withCredentials: true,
          headers: {
            accept: "*/*",
          },
        }
      );

      if (response.status === 200) {
        setUser(response.data.user);
        console.log("Fetched user:", response.data.user);
      } else {
        setUser(null);
        console.log("No user logged in");
      }
    } catch (error) {
      if (error.response) {
        if (error.response.status === 401) {
          console.error("User not logged in:", error.response.data.message);
        } else if (error.response.status === 404) {
          console.error("User not found:", error.response.data.message);
        } else {
          console.error("Error fetching user:", error.response.data.message);
        }
      } else {
        console.error("Error fetching user:", error.message);
      }
    }
  };

  const fetchCartItems = async (userId) => {
    try {
      const response = await axios.get(
        `https://localhost:7118/api/Carts/items?userId=${userId}`
      );
      setCartItems(response.data);
    } catch (error) {
      console.log("Error fetching cart items:", error);
    }
  };

  // Kullanıcı giriş fonksiyonu
  const login = async (username, password) => {
    try {
      const response = await axios.post(
        "https://localhost:7118/api/Users/login",
        { username, password },
        {
          withCredentials: true,
          headers: {
            "Content-Type": "application/json",
          },
        }
      );
      console.log("Login successful:", response.data);
      setUser(response.data.user);
      localStorage.setItem('user', JSON.stringify(response.data.user));
      window.location.reload();
    } catch (error) {
      console.error("Login failed:", error.response?.data || error.message);
    }
  };

  // Kullanıcı çıkış fonksiyonu
  const logout = async (username) => {
    try {
      const response = await axios.post(
        "https://localhost:7118/api/Users/logout",
        { username },
        {
          withCredentials: true,
          headers: {
            accept: "*/*",
          },
        }
      );
      if (response.status === 200) {
        setUser(null);
        console.log("Logout successful");
        localStorage.clear();
        sessionStorage.clear();
        window.location.reload();
      }
    } catch (error) {
      console.error("Logout failed:", error);
    }
  };

  const fetchBooks = async () => {
    try {
      let response;
      if (query) {
        setBooks([]); // books state'ini sıfırla
        response = await axios.get(`https://localhost:7118/api/Books/search?query=${query}`);
      } else {
        response = await axios.get("https://localhost:7118/api/Books");
      }
      setBooks(response.data);
      console.log(response.data);
    } catch (error) {
      console.log(error);
    }
  };

  useEffect(() => {
    const initialize = async () => {
      await fetchUser();
    };
    initialize();
  }, []);

  useEffect(() => {
    const initialize = async () => {
      await fetchBooks();
    };
    initialize();
  }, [query]);

  useEffect(() => {
    if (user) {
      fetchCartItems(user.id);
    }
  }, [user]);

  return (
    <StoreContext.Provider
      value={{ user, setUser, login, logout, books, cartItems, setCartItems, query, setQuery }}
    >
      {children}
    </StoreContext.Provider>
  );
};