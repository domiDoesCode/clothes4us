import React, { useContext, useState } from "react";
import { useForm } from "react-hook-form";
import axios from "axios";
import Button from "../components/Button";
import { Link, Redirect } from "react-router-dom";
import "./Login.css";
import { UserContext } from "../context/UserContext";
import { toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
axios.defaults.withCredentials = true;

export default function Login() {
  const { register, handleSubmit } = useForm();
  const { setUser } = useContext(UserContext);
  const [statusCode, setStatuscode] = useState(0);

  const notify = (message) => {
    if (message === 200) {
      toast.success("Login successful ", {
        position: "bottom-right",
        autoClose: 5000,
        hideProgressBar: true,
        closeOnClick: true,
        pauseOnHover: true,
        draggable: true,
        progress: undefined,
      });
    } else if (message === 401) {
      toast.error("Username or password is not correct", {
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
      .post("http://localhost:5000/api/Login", data)
      .then((res) => {
        setUser(res.data);
        setStatuscode(res.status);
        notify(200);
      })
      .catch((error) => notify(401));
  };

  return (
    <div className="loginFormContainer">
      <div className="content">
        <form onSubmit={handleSubmit(onSubmit)}>
          <input
            type="text"
            {...register("username")}
            required
            placeholder="Username"
          />
          <input
            type="password"
            {...register("password")}
            required
            placeholder="Password"
          />
          <div className="btn-container">
            <Button value="Login" />
            {statusCode === 200 ? (
              <>
                <Redirect to="/"></Redirect>
              </>
            ) : (
              <></>
            )}
            <p>OR</p>
            <Link to="/register">
              <Button value="Register" />
            </Link>
          </div>
        </form>
      </div>
    </div>
  );
}
