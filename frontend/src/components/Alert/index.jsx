import React, { useState } from 'react';
import useProfileUser from '../../hooks/useProfileUser';
import { useNavigate } from 'react-router-dom';

const index = () => {
  const username = useProfileUser();
  const navigate = useNavigate();

  const clickLogout = (e) => {
    e.preventDefault();
    localStorage.removeItem('at');
    localStorage.removeItem('rt');
    navigate("/");
  }

  return (
    <div>
      <h1>Hello, {username}</h1>
      <button onClick={clickLogout}>Logout</button>
    </div>
  );
};

export default index;