import { createContext, useState, useEffect } from "react";
import axios from "axios";
axios.defaults.withCredentials = true;

export const ProductsContext = createContext(null);

export const ProductsProvider = (props) => {
  const [products, setProducts] = useState([]);

  useEffect(async () => {
    const res = await axios
      .get("http://localhost:5000/api/Product")
      .catch((err) => {
        console.log(err.message);
      });
    if (res == null) {
      setProducts([]);
    } else {
      setProducts(res.data);
    }
  }, []);

  const value = {
    products,
    setProducts,
  };

  return (
    <ProductsContext.Provider value={value}>
      {props.children}
    </ProductsContext.Provider>
  );
};
