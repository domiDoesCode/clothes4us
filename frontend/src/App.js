import "./App.css";
import Header from "./components/Header";
import { BrowserRouter as Router } from "react-router-dom";
import Container from "./components/Container";
import { UserProvider } from "./context/UserContext";
import { AddressProvider } from "./context/AddressContext";
import { CartProvider } from "./context/CartContext";
import React from "react";
import { ProductsProvider } from "./context/ProductsContext";
import { BrandsProvider } from "./context/BrandsContext";
import { ToastContainer, Flip } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

function App() {
  return (
    <Router>
      <UserProvider>
        <AddressProvider>
          <CartProvider>
            <ProductsProvider>
              <BrandsProvider>
                <Header />
                <Container />
                <div className="rightbar"></div>
              </BrandsProvider>
            </ProductsProvider>
          </CartProvider>
        </AddressProvider>
      </UserProvider>
      <ToastContainer transition={Flip} />
    </Router>
  );
}

export default App;
