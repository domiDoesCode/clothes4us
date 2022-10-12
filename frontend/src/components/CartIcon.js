import React from "react";
import "./CartIcon.css";

function CartIcon(props) {
  return (
    <i className="fas fa-shopping-cart cartIcon" onClick={props.onClick} />
  );
}

export default CartIcon;
