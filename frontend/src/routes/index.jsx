import { createBrowserRouter } from "react-router-dom";
import App from "../App";
import LoginForm from '../components/Auth/LoginForm';
import RegisterForm from '../components/Auth/RegisterForm';
import Alert from "../components/Alert";

const routers = createBrowserRouter([
  {
    path: "/",
    element: <App />,
    children: [
      {
        path: "profile",
        element: <Alert />,
      }
    ]
  },
  {
    path: "/login",
    element: <LoginForm />
  },
  {
    path: '/register',
    element: <RegisterForm />
  }
]);

export default routers;
