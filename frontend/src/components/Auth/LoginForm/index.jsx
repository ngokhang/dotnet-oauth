import { useGoogleLogin } from "@react-oauth/google";
import axios from "axios";
import { useState } from "react";
import { useNavigate } from 'react-router-dom';

const index = () => {
  const [username, setUsername] = useState();
  const [password, setPassword] = useState();
  const navigate = useNavigate();
  const handleOnSuccess = async (code) => {
    const result = await axios.post('https://localhost:7007/auth/google', { code: code.code }, {
      headers: { "Content-Type": "application/json" } // config
    });
    if (result.status === 200) {
      const data = result.data;
      const accessToken = data.value.access_token;
      const refreshToken = data.value.refresh_token;
      localStorage.setItem('at_google', accessToken);
      localStorage.setItem('rt_google', refreshToken);
    }
    navigate('/profile');
  }
  const clickLogin = useGoogleLogin({
    onSuccess: codeResponse => handleOnSuccess(codeResponse),
    onError: errorMsg => console.log(">>> ", errorMsg),
    flow: 'auth-code',
  });
  const submitForm = async (e) => {
    e.preventDefault();
    const userData = {
      username, password
    };
    try {
      const result = await axios.post("https://localhost:7007/api/auth/login", userData);
      localStorage.setItem('at', result.data.data.accessToken);
      localStorage.setItem('rt', result.data.data.refreshToken);
      navigate("/");
    } catch (error) {
      console.log(error.response.data.message);
    }

  }
  return (
    <div>
      <form onSubmit={submitForm}>
        <div>
          <label htmlFor="username" style={{ display: 'block' }}>Username</label>
          <input type="text" name="username" id="username" placeholder='username' onChange={e => setUsername(e.target.value)} />
        </div>
        <div>
          <label htmlFor="password" style={{ display: 'block' }}>Password</label>
          <input type="text" name="password" id="password" placeholder='password' onChange={e => setPassword(e.target.value)} />
        </div>
        <button style={{ display: 'block' }}>Login</button>
      </form>
      <button onClick={clickLogin}>Login with Google</button>
    </div >
  );
};

export default index;