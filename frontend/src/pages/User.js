import React, { useContext } from "react";
import { UserContext } from "../context/UserContext";
import { AddressContext } from "../context/AddressContext";
import Button from "../components/Button";
import { Link } from "react-router-dom";
import "./User.css";

const User = () => {
  const { user } = useContext(UserContext);
  const { address } = useContext(AddressContext);

  return (
    <div className="user">
      <div className="user_container">
        {user ? (
          <div className="user_info">
            <h1>
              {user.firstname} {user.lastname}
            </h1>
            <h5>#{user.userId}</h5>
            <h2>Username :</h2>
            <p>{user.username}</p>
            <hr
              style={{
                color: "white",
                height: 2,
                width: 100,
              }}
            />
            <h2>Email address:</h2>
            <p>{user.email}</p>
            <div className="change_password_email_btn">
              <Link to="/changepassword">
                <Button value="Change password" />
              </Link>
              <Link to="/changeemail">
                <Button value="Change email" />
              </Link>
            </div>
            <div className="user_address_info">
              <h3>Country:</h3>
              <p>{address.country}</p>
              <hr
                style={{
                  color: "white",
                  height: 2,
                  width: 100,
                }}
              />
              <h3>City:</h3>
              <p>{address.city}</p>
              <hr
                style={{
                  color: "white",
                  height: 2,
                  width: 100,
                }}
              />
              <h3>Street, number:</h3>
              <p>{address.address}</p>
            </div>
          </div>
        ) : (
          <></>
        )}
      </div>
    </div>
  );
};

export default User;
