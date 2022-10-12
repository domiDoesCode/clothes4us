import React from "react";
import Login from "../pages/Login";
import LeftSideNav from "./LeftSideNav";
import { Switch, Route } from "react-router-dom";
import Home from "../pages/Home";
import Registration from "../pages/Registration";
import Cart from "../pages/Cart";
import User from "../pages/User";
import Shipping from "../pages/Shipping";
import UpdateAddress from "../pages/UpdateAddress";
import Checkout from "../pages/Checkout";
import Brands from "../pages/Brands";
import ChangeEmail from "../pages/ChangeEmail";
import ChangePassword from "../pages/ChangePassword";

const Container = () => {
  return (
    <div className="container">
      <LeftSideNav className="leftNavBar" />
      <Switch>
        <Route exact path="/" component={Home}></Route>
        <Route exact path="/login" component={Login}></Route>
        <Route exact path="/register" component={Registration}></Route>
        <Route exact path="/cart" component={Cart}></Route>
        <Route exact path="/user" component={User}></Route>
        <Route exact path="/shipping" component={Shipping}></Route>
        <Route exact path="/updateaddress" component={UpdateAddress}></Route>
        <Route exact path="/checkout" component={Checkout}></Route>
        <Route exact path="/brands" component={Brands}></Route>
        <Route exact path="/changeemail" component={ChangeEmail}></Route>
        <Route exact path="/changepassword" component={ChangePassword}></Route>
      </Switch>
    </div>
  );
};

export default Container;
