import React, { useContext, useState } from "react";
import "./ChangePages.css";
import { useForm } from "react-hook-form";
import Button from "../components/Button";
import axios from "axios";
import { UserContext } from "../context/UserContext";
import { toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

function ChangeEmail() {
  const { register, handleSubmit } = useForm();
  const { user, setUser } = useContext(UserContext);
  const [statusCode, setStatusCode] = useState(0);

  const notify = (message) => {
    if (message === 200) {
      toast.success("Email changed ", {
        position: "bottom-right",
        autoClose: 5000,
        hideProgressBar: true,
        closeOnClick: true,
        pauseOnHover: true,
        draggable: true,
        progress: undefined,
      });
    } else if (message === 400) {
      toast.error("Email is not changed", {
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

  const onSubmit = async (data) => {
    let userId = user.userId;
    let userTmp = user;
    userTmp.email = data.email;
    setUser(userTmp);
    await axios
      .put(`http://localhost:5000/api/User/${userId}/email`, user)
      .then((res) => {
        setStatusCode(200);
      })
      .catch((error) => {
        setStatusCode(400);
      });

    switch (statusCode) {
      case 200:
        notify(200);
        break;
      case 400:
        notify(400);
        break;
    }
  };

  return (
    <div className="form_container">
      <div className="form_wrapper">
        <form className="form " onSubmit={handleSubmit(onSubmit)}>
          <input
            type="email"
            {...register("email")}
            required
            placeholder="New Email"
          />
          <Button value="Change email" />
        </form>
      </div>
    </div>
  );
}

export default ChangeEmail;
