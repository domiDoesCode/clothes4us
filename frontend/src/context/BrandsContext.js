import { createContext, useState, useEffect } from "react";
import axios from "axios";
axios.defaults.withCredentials = true;

export const BrandsContext = createContext(null);

export const BrandsProvider = (props) => {
  const [brands, setBrands] = useState([]);

  useEffect(async () => {
    const res = await axios
      .get("http://localhost:5000/api/Brand")
      .catch((err) => {
        console.log(err.message);
      });
    if (res == null) {
      setBrands([]);
    } else {
      setBrands(res.data);
    }
  }, []);

  const value = {
    brands,
    setBrands,
  };

  return (
    <BrandsContext.Provider value={value}>
      {props.children}
    </BrandsContext.Provider>
  );
};
