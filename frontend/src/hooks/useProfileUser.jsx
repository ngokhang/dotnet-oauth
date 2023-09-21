import axios from 'axios';
import React, { useEffect, useState } from 'react';

const useProfileUser = () => {
  const [username, setUsername] = useState('');

  const fetchData = async () => {
    const res = await axios.get('https://localhost:7007/auth/google/me', { headers: { Authorization: `Bearer ${localStorage.getItem('at_google')}` } });
    console.log(res);
    setUsername(res.data.name);
    localStorage.setItem('at', res.data.access_token);
    localStorage.setItem('rt', res.data.refresh_token);
  }

  useEffect(() => {
    fetchData();
  }, []);

  return username;
};

export default useProfileUser;