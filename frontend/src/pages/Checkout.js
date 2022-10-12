import Button from "../components/Button";
import React, { useContext, useEffect, useState } from "react";
import { AddressContext } from "../context/AddressContext";
import { CartContext } from "../context/CartContext";
import { Redirect } from "react-router-dom";
import axios from "axios";
import "./Checkout.css";
import { UserContext } from "../context/UserContext";
import { toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

function Checkout() {
  const { address } = useContext(AddressContext);
  const { cartProducts, refreshCartProducts } = useContext(CartContext);
  const { user } = useContext(UserContext);
  const [totalPrice, setTotalPrice] = useState(0);
  const [statusCode, setStatusCode] = useState(0);

  const notify = (message) => {
    if (message === 200) {
      toast.success("Successfully ordered ", {
        position: "bottom-right",
        autoClose: 5000,
        hideProgressBar: true,
        closeOnClick: true,
        pauseOnHover: true,
        draggable: true,
        progress: undefined,
      });
    } else if (message === 400) {
      toast.error("Something went wrong", {
        position: "bottom-right",
        autoClose: 5000,
        hideProgressBar: true,
        closeOnClick: true,
        pauseOnHover: true,
        draggable: true,
        progress: undefined,
      });
    }
  };

  useEffect(() => {
    let sumTotal = 0;
    if (cartProducts != null) {
      cartProducts.forEach((x) => {
        sumTotal += x.price;
      });
      setTotalPrice(sumTotal);
    }
  }, [cartProducts]);

  let userId = user.userId;

  const onClickBuy = async () => {
    await axios
      .post(`http://localhost:5000/api/User/${userId}/orders`)
      .then((res) => {
        notify(200);
        setStatusCode(200);
        refreshCartProducts();
      })
      .catch((error) => {
        notify(400);
      });
  };

  return (
    <div className="checkout_container">
      {user == null ? <Redirect to="/login"></Redirect> : <></>}
      <div>
        {cartProducts != null ? (
          <>
            <h3>Products:</h3>
            {cartProducts.map((x) => (
              <>
                <p>Brand : {x.brand}</p>
                <p>Name : {x.name}</p>
                <p>quantity : {x.quantity}</p>
                <p>Price: {x.price * x.quantity} dkk</p>
              </>
            ))}
          </>
        ) : (
          <></>
        )}
        {address != null ? (
          <>
            <h3>Shipping info:</h3>
            <p>Country: {address.country}</p>
            <p>City: {address.city}</p>
            <p>Address: {address.address}</p>
          </>
        ) : (
          <></>
        )}
      </div>
      <h2>Total price: {totalPrice} dkk</h2>
      <div className="checkout_arrow_buy_button">
        <i className="fas fa-arrow-left"></i>
        <Button value="Buy" onClick={onClickBuy} />
        {statusCode === 200 ? <Redirect to="/"></Redirect> : <></>}
      </div>
    </div>
  );
}

export default Checkout;
