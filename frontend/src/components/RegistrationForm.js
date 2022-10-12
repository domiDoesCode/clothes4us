import React from 'react'
import {useForm} from 'react-hook-form'
import axios from 'axios'

export default function RegistrationForm() {    
    const {register, handleSubmit} = useForm();

    const onSubmit = data => {
        axios.post('https://localhost:44395/api/Registration', data)
        .then(res => 
            console.log(res),
            )
    }

    return (
        <div className='LoginForm'>
            <form onSubmit={handleSubmit(onSubmit)}>
                <p>Username: </p>
                <input type="text" {...register("username")} />
                <p>Password: </p>
                <input type="password" {...register("password")} />
                <p>Email: </p>
                <input type="email" {...register("email")} />
                <p>Firstname: </p>
                <input type="text" {...register("firstname")} />
                <p>Lastname: </p>
                <input type="text" {...register("lastname")} />
                <input type="submit" />
            </form> 
        </div>)
}
