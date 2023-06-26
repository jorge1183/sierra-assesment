import { useState } from "react";
import { useForm } from "react-hook-form";
import { Button, Form, Spinner } from "reactstrap"
import CustomInput from "./common/CustomInput";
const { useHistory } = require("react-router-dom");

interface ILogin {
    username: string;
    password: string;
}
const Login = () => {
    const [loading, setLoading] = useState(false);
    const { control, handleSubmit } = useForm({ defaultValues: { username: "", password: "" } });
    const history = useHistory();

    const submit = async (data: ILogin) => {
        try {
            setLoading(true);
            const fetchResult = await fetch(`api/session/login?username=${data.username}&password=${data.password}`, { method: "POST" });

            if (!fetchResult.ok) {
                if (fetchResult.status === 403) {
                    alert("Invalid credentials");
                } else {
                    throw (fetchResult.statusText);
                }
                return;
            }

            const response = await fetchResult.json();
            sessionStorage.setItem("token", response.accessToken);
            alert("User logged in successfully");
            history.push("/");
        } catch (error) {
            console.log("Unexpected error", error);
            alert("An unexpected error occurred, please review console log");
        } finally {
            setLoading(false);
        }
    }
    return (
        <Form onSubmit={handleSubmit(submit)}>
            <CustomInput label="User name" name="username" control={control} type="text" />
            <CustomInput label="Password" name="password" control={control} type="password" />
            {
                loading
                    ? (<Spinner> </Spinner>)
                    : (<Button type="submit">Login</Button>)
            }
        </Form>
    );
}

export default Login;