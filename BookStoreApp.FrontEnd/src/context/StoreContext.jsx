import { createContext, useState, useEffect } from "react";
import axios from "axios";

// Context oluşturma
export const StoreContext = createContext();

// Provider bileşeni
export const StoreProvider = ({ children }) => {
  const [user, setUser] = useState(null);
  const [books, setBooks] = useState([]);
  const [reviews, setReviews] = useState([]);
  const [categories, setCategories] = useState([]);
  const [cartItems, setCartItems] = useState([]);
  const [query, setQuery] = useState('');

  // Kullanıcı bilgilerini API'den al
  const fetchUser = async () => {
    try {
      const token = localStorage.getItem('accessToken');
      const response = await axios.get(
        "https://localhost:7118/api/Auth/getUser",
        {
          headers: {
            accept: "*/*",
            Authorization: `Bearer ${token}`,
          },
        }
      );

      if (response.status === 200) {
        setUser(response.data);
        console.log("Fetched user:", response.data);
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

  // Kullanıcı giriş fonksiyonu
  const login = async (username, password) => {
    try {
      const response = await axios.post(
        "https://localhost:7118/api/Auth/login",
        { username, password },
        {
          headers: {
            "Content-Type": "application/json",
          },
        }
      );
      console.log("Login successful:", response.data);
      localStorage.setItem('accessToken', response.data.accessToken);
      localStorage.setItem('refreshToken', response.data.refreshToken);
      localStorage.setItem('userId', response.data.userId);
      fetchUser();
    } catch (error) {
      console.error("Login failed:", error.response?.data || error.message);
    }
  };

  const fetchCartItems = async (userId) => {
    try {
      const token = localStorage.getItem('accessToken');
      const response = await axios.get(
        `https://localhost:7118/api/Carts/items?userId=${userId}`,
        {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        }
      );
      return response.data;
    } catch (error) {
      console.log("Error fetching cart items:", error);
    }
  };

  // Kullanıcı çıkış fonksiyonu
  const logout = async () => {
    try {
      const token = localStorage.getItem('accessToken');
      const refreshToken = user.refreshToken;
      const userId = user.id;
      const response = await axios.post(
        "https://localhost:7118/api/Auth/logout",
        { userId, refreshToken },
        {
          headers: {
            accept: "*/*",
            Authorization: `Bearer ${token}`,
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
      console.error("Logout failed:", error.response?.data || error.message);
    }
  };

  const fetchCategories = async () => {
    try {
      const response = await axios.get("https://localhost:7118/api/Categories/");
      console.log(response.data);
      return setCategories(response.data); // Kategori verilerini döner
    } catch (error) {
      console.error("Error fetching categories:", error);
      return [];
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

  const fetchBooksByCategory = async (categoryId) => {
    try {
      const response = await axios.get(`https://localhost:7118/api/Books/GetBooksByCategoryId/${categoryId}`);
      setBooks(response.data);
      console.log(response.data);
    } catch (error) {
      console.log(error);
    }
  };

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
  }, [setCartItems]);

  useEffect(() => {
    const initialize = async () => {
      await fetchCategories();
    };
    initialize();
  }, []);

  useEffect(() => {
    const initialize = async () => {
      await fetchUser();
    };
    initialize();
  }, []);

  return (
    <StoreContext.Provider
      value={{ user, setUser, login, logout, books, cartItems, setCartItems, query, setQuery, categories, fetchBooksByCategory, reviews, setReviews }}
    >
      {children}
    </StoreContext.Provider>
  );
};

export default StoreContext;