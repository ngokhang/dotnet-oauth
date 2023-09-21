import { Outlet, useNavigate } from "react-router-dom";

const App = () => {
  const navigate = useNavigate();
  const clickLogin = e => {
    e.preventDefault();
    navigate("/login");
  }
  const clickRegister = e => {
    e.preventDefault();
    navigate('/register');
  }

  return (
    <div>
      <h1>Login with Google</h1>
      <div>
        {
          !localStorage.getItem('at') ? (
            <button onClick={clickLogin}>Login</button>
          ) : ' '
        }
        {
          !localStorage.getItem('at') ? (
            <button onClick={clickRegister}>Register</button>
          ) : ' '
        }
      </div>
      <Outlet />
    </div>
  );
};

export default App;