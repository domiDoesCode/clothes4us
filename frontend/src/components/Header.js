import React, { useContext, useState, useEffect } from "react";
import "./Header.css";
import { Link } from "react-router-dom";
import Button from "../components/Button";
import { UserContext } from "../context/UserContext";
import CartIcon from "./CartIcon";
import axios from "axios";
import { CartContext } from "../context/CartContext";

const Header = () => {
  const { user, setUser } = useContext(UserContext);
  const { cartProducts } = useContext(CartContext);
  const [quantity, setQuantity] = useState(0);

  useEffect(() => {
    let sumQuantity = 0;
    if (cartProducts.length > 0) {
      cartProducts.forEach((x) => {
        sumQuantity += x.quantity;
      });
      setQuantity(sumQuantity);
    }
  }, [cartProducts]);

  const onClickLogout = () => {
    setUser(null);
    axios.post("http://localhost:5000/api/User/logout");
  };

  return (
    <div className="header">
      <Link to="/">
        <i className="fas fa-tshirt"></i>
      </Link>
      <div className="navbar">
        <Link to="/">
          <h4>Home</h4>
        </Link>
        <Link to="/brands">
          <h4>Brands</h4>
        </Link>
        <h4>Contact</h4>
        <h4>Shipping</h4>
      </div>
      {user ? (
        <div className="if-logged-in">
          <p>
            Welcome,
            <Link to="/user" className="if-logged-in-user-link">
              {user.firstname}
            </Link>
          </p>
          <div>
            {cartProducts.length > 0 ? (
              <>
                <div className="dot">
                  <p>{quantity}</p>
                </div>
              </>
            ) : (
              <></>
            )}

            <Link to="/cart" className="if-logged-in-cart-icon">
              <CartIcon />
            </Link>
          </div>
          <Button value="Log out" onClick={onClickLogout} />
        </div>
      ) : (
        <>
          <Link to="/login">
            <Button value="Login" />
          </Link>
          <Link to="/register">
            <Button value="Register" />
          </Link>
        </>
      )}
    </div>
  );
};

export default Header;
