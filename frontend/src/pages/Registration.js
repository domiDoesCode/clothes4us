import React, { useState } from "react";
import { useForm } from "react-hook-form";
import "./Registration.css";
import axios from "axios";
import { Redirect } from "react-router-dom";
import { toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

const Registration = () => {
  const { register, handleSubmit } = useForm();
  const [usernameTaken, setUsernameTaken] = useState(false);
  const [statusCode, setStatusCode] = useState(0);
  const [emailTaken, setEmailTaken] = useState(false);

  const notify = (statusCode) => {
    if (statusCode === 201) {
      toast.success("Account created ", {
        position: "bottom-right",
        autoClose: 5000,
        hideProgressBar: true,
        closeOnClick: true,
        pauseOnHover: true,
        draggable: true,
        progress: undefined,
      });
    } else {
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

  const onSubmit = (data) => {
    axios
      .post("http://localhost:5000/api/User", data)
      .then((res) => {
        console.log(data);
        if (res.status === 201) {
          setStatusCode(res.status);
          notify(201);
        }
      })
      .catch(function (error) {
        console.log(error.response);
        if (error.response.data === "Email taken") {
          setEmailTaken(true);
        } else if (error.response.data === "Username taken") {
          setUsernameTaken(true);
        }
      });
  };

  const handleUsernameClick = () => {
    setUsernameTaken(false);
  };

  const handleEmailClick = () => {
    setEmailTaken(false);
  };

  return (
    <div className="registrationFormContainer">
      <div className="content">
        <form onSubmit={handleSubmit(onSubmit)}>
          <input
            type="text"
            {...register("username")}
            required
            placeholder="Username"
            onClick={handleUsernameClick}
          />
          {usernameTaken ? <p>Username is already taken</p> : <p></p>}
          <input
            type="password"
            {...register("password")}
            required
            placeholder="Password"
          />
          <input
            type="email"
            {...register("email")}
            required
            placeholder="Email"
            onClick={handleEmailClick}
          />
          {emailTaken ? <p>Email is already taken</p> : <p></p>}
          <input
            type="text"
            {...register("firstname")}
            required
            placeholder="Firstname"
          />
          <input
            type="text"
            {...register("lastname")}
            required
            placeholder="Lastname"
          />
          <div className="btn-container">
            <button type="submit">Register</button>
            {statusCode === 201 ? (
              <>
                <Redirect to="/login"></Redirect>
              </>
            ) : (
              <></>
            )}
          </div>
        </form>
      </div>
    </div>
  );
};

export default Registration;
