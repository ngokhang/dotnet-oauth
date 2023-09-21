import React from 'react'
import ReactDOM from 'react-dom/client'
import './index.css'
import { RouterProvider } from 'react-router-dom'
import routers from './routes/index.jsx'
import { GoogleOAuthProvider } from '@react-oauth/google'

ReactDOM.createRoot(document.getElementById('root')).render(
  <React.StrictMode>
    <GoogleOAuthProvider clientId='718783698594-upqe5q5q5dvicood4ve4jdt1qqqivaba.apps.googleusercontent.com'>
      <RouterProvider router={routers} />
    </GoogleOAuthProvider>
  </React.StrictMode>,
)
