import React, { useContext, useState } from "react";
import { Link } from "react-router-dom";
import "./Cards.css";
import CartIcon from "./CartIcon";
import axios from "axios";
import { toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import { CartContext } from "../context/CartContext";
import { UserContext } from "../context/UserContext";
axios.defaults.withCredentials = true;

const CardItem = (props) => {
  const { refreshCartProducts } = useContext(CartContext);
  const { user } = useContext(UserContext);
  const notify = (message) => {
    switch (message) {
      case "success": {
        toast.success("Product added to cart ", {
          position: "bottom-right",
          autoClose: 5000,
          hideProgressBar: true,
          closeOnClick: true,
          pauseOnHover: true,
          draggable: true,
          progress: undefined,
        });
        break;
      }
      case "alreadyadded": {
        toast.error("Already added", {
          position: "bottom-right",
          autoClose: 5000,
          hideProgressBar: true,
          closeOnClick: true,
          pauseOnHover: true,
          draggable: true,
          progress: undefined,
        });
        break;
      }
      case "no user logged in": {
        toast.error("You are not logged in", {
          position: "bottom-right",
          autoClose: 5000,
          hideProgressBar: true,
          closeOnClick: true,
          pauseOnHover: true,
          draggable: true,
          progress: undefined,
        });
        break;
      }
    }
  };
  let userId;
  if (user != null) {
    userId = user.userId;
  }

  const onClick = async (data) => {
    data = {
      productId: props.path,
    };
    try {
      const res = await axios.post(
        `http://localhost:5000/api/User/${userId}/Cart`,
        data
      );
      if (res.status == 200) {
        notify("success");
        refreshCartProducts();
      }
    } catch (error) {
      switch (error.response.status) {
        case 403: {
          notify("alreadyadded");
          break;
        }
        case 400: {
          notify("no user logged in");
          break;
        }
      }
    }
  };
  return (
    <>
    <>
      {props.quantity > 0 ? (
        <div>
          <li className="cards__item">
            <Link className="cards__item__link" to="/">
              <figure
                className="cards__item__pic-wrap"
                data-category={props.label}
              >
                <img
                  className="cards__item__img"
                  alt="Travel Image"
                  src="data:image/jpeg;base64,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"
                />
              </figure>
              <div className="cards__item__info">
                <div className="testdiv">
                  <h5 className="cards__item__text">{props.text}</h5>
                  <h6 className="cards_item_price">{props.price} dkk</h6>
                  {props.quantity <= 5 ? (
                    <h6 className="cards_item_quantity">
                      Only {props.quantity} left
                    </h6>
                  ) : (
                    <></>
                  )}
                </div>
                <CartIcon onClick={onClick} />
              </div>
            </Link>
          </li>
        </div>
      ) : (
        <></>
      )}
    </>
    </>
  );
};

export default CardItem;
