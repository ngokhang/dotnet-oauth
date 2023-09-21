import axios from 'axios';
import React, { useState } from 'react';

const index = () => {
  const [username, setUsername] = useState();
  const [password, setPassword] = useState();
  const submitForm = async (e) => {
    e.preventDefault();
    const userData = {
      _id: '', username, password, rule: "user"
    };
    const result = await axios.post("https://localhost:7007/api/auth/register", userData);
    console.log(result);
  }
  return (
    <form onSubmit={submitForm}>
      <div>
        <label htmlFor="username" style={{ display: 'block' }}>Username</label>
        <input type="text" name="username" id="username" placeholder='username' onChange={e => setUsername(e.target.value)} />
      </div>
      <div>
        <label htmlFor="password" style={{ display: 'block' }}>Password</label>
        <input type="text" name="password" id="password" placeholder='password' onChange={e => setPassword(e.target.value)} />
      </div>
      <button style={{ display: 'block' }}>Register</button>
    </form>
  );
};

export default index;