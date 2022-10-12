import { createContext, useState, useEffect, useContext } from "react";
import { UserContext } from "./UserContext";
import axios from "axios";
axios.defaults.withCredentials = true;

export const AddressContext = createContext(null);

export const AddressProvider = (props) => {
  const [address, setAddress] = useState(null);
  const { user } = useContext(UserContext);
  let userId;
  if (user != null) {
    userId = user.userId;
  }

  const refreshAddress = () => {
    getAddress();
  };

  const getAddress = async () => {
    const res = await axios
      .get(`http://localhost:5000/api/User/${userId}/address`)
      .catch((err) => {
        console.log(err.message);
      });
    if (res == null) {
      setAddress([]);
    } else {
      setAddress(res.data);
    }
  };

  const value = {
    address,
    setAddress,
    refreshAddress,
  };

  return (
    <AddressContext.Provider value={value}>
      {props.children}
    </AddressContext.Provider>
  );
};
