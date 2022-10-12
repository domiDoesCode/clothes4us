import { createContext, useState, useEffect, useContext } from "react";
import { UserContext } from "../context/UserContext";
import axios from "axios";
axios.defaults.withCredentials = true;

export const CartContext = createContext(null);

export const CartProvider = (props) => {
  const [cartProducts, setCartProducts] = useState([]);
  const { user } = useContext(UserContext);
  let userId;
  if (user != null) {
    userId = user.userId;
  }

  const getCart = async () => {
    const res = await axios
      .get(`http://localhost:5000/api/User/${userId}/cart`)
      .catch((err) => {
        console.log(err.message);
      });
    console.log(res);
    if (res == null) {
      setCartProducts([]);
    } else {
      setCartProducts(res.data);
    }
  };

  useEffect(async () => {
    getCart();
  }, []);

  const refreshCartProducts = async () => {
    await getCart();
  };

  const value = {
    cartProducts,
    setCartProducts,
    refreshCartProducts,
  };

  return (
    <CartContext.Provider value={value}>{props.children}</CartContext.Provider>
  );
};
