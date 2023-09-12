import React, { useState } from 'react';
import './homePage.css'; 
import Button from '@mui/material/Button';
import { Navigate, useNavigate } from 'react-router-dom';

const HomePage = () => {
    const navigate = useNavigate();
    const handleLogIn = () => {
        navigate("/logIn");
    
      };
      const handleSignUp = () => {
        navigate("/signUp");
    
      };
  return (
    <div className="containerHome">
          <h1>ברוך הבא</h1>
      <Button
        id="btn-1"
        variant="contained"
        size="large"
        color="primary"
        onClick={handleLogIn}
      >
        התחברות
      </Button>
      <Button
        id="btn-2"
        variant="contained"
        size="large"
        color="primary"
        onClick={handleSignUp}
      >
        הרשמה
      </Button>
    
    </div>
  );
}

export default HomePage;
