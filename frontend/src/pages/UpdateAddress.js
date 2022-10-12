import React, { useContext } from "react";
import axios from "axios";
import { useForm } from "react-hook-form";
import "./Shipping.css";
import Button from "../components/Button";
import { UserContext } from "../context/UserContext";
axios.defaults.withCredentials = true;

function UpdateAddress() {
  const { register, handleSubmit } = useForm();
  const { user } = useContext(UserContext);

  let userId = user.userId;

  const onSubmit = (data) => {
    axios
      .put(`http://localhost:5000/api/User/${userId}/address`, data)
      .then((res) => {})
      .catch((error) => console.log(error.request));
  };

  return (
    <div className="shipping_container">
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
          <Button value="Change address" />
        </div>
      </form>
    </div>
  );
}

export default UpdateAddress;
