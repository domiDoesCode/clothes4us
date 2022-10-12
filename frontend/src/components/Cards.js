import React, { useState, useEffect, useContext } from "react";
import CardItem from "./CardItem";
import axios from "axios";
import "./Cards.css";
import { ProductsContext } from "../context/ProductsContext";

import { ToastContainer, toast, Flip } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
function Cards() {
  const { products, setProducts } = useContext(ProductsContext);

  useEffect(() => {
    axios
      .get("http://localhost:5000/api/Product")
      .then((res) => {
        setProducts(res.data);
        console.log(res);
      })
      .catch((err) => {
        console.log(err.message);
      });
  }, []);

  return (
    <>
      <div className="cards">
        <h1>Check out our new collection!</h1>
        <div className="cards__container">
          <div className="cards__wrapper">
            <ul className="cards__items">
              {products.map((x) => (
                <>
                  <CardItem
                    text={x.name}
                    label={x.type}
                    path={x.productId}
                    price={x.price}
                    quantity={x.quantity}
                  />
                </>
              ))}
            </ul>
          </div>
        </div>
      </div>
    </>
  );
}

export default Cards;
