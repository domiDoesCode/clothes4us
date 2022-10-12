import Button from "../components/Button";
import { useForm } from "react-hook-form";
import React, { useEffect, useContext } from "react";
import "./Shipping.css";
import axios from "axios";
import { Link } from "react-router-dom";
import { AddressContext } from "../context/AddressContext";
import { UserContext } from "../context/UserContext";
axios.defaults.withCredentials = true;

function Checkout() {
  const { register, handleSubmit } = useForm();
  const { address, setAddress } = useContext(AddressContext);
  const { user } = useContext(UserContext);
  let userId;
  if (user != null) {
    userId = user.userId;
  }

  const onSubmit = async (data) => {
    await axios.post(`http://localhost:5000/api/User/${userId}/address`);
  };

  useEffect(async () => {
    const res = await axios.get(
      `http://localhost:5000/api/User/${userId}/address`
    );
    setAddress(res.data);
  }, []);

  return (
    <div className="shipping_container">
      {address === [] ? (
        <form className="shipping_form" onSubmit={handleSubmit(onSubmit)}>
          <input
            type="text"
            {...register("country")}
            required
            placeholder="Country"
          ></input>
          <input
            type="text"
            {...register("city")}
            required
            placeholder="City"
          ></input>
          <input
            type="text"
            {...register("address")}
            required
            placeholder="Address"
          ></input>
          <div className="shipping_form_button_container">
            <Button value="Add address" />
          </div>
        </form>
      ) : (
        <div className="address_exists_container">
          {address != null ? (
            <>
              <h3>Country: {address.country}</h3>
              <h3>City: {address.city}</h3>
              <h3>Address: {address.address}</h3>
              <div className="address_exists_button_container">
                <Link to="/updateaddress">
                  <Button value="Change address" />
                </Link>
                <Link to="/checkout">
                  <Button value="Check out" />
                </Link>
              </div>
            </>
          ) : (
            <></>
          )}
        </div>
      )}
    </div>
  );
}

export default Checkout;
