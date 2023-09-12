import React, { useState } from 'react';
import { Button, Box } from '@mui/material';
import './signup.css'; 
import { Navigate, useNavigate } from 'react-router-dom';
import { signUpApi } from '../src/api';

const SignUp = () => {
  const navigate = useNavigate();
  const [signUp, setSignUp] = useState({
    customerName: '',
    email: '',
    passward: ''
  });

  const handleSubmit = async (e) => {
    debugger
    const response= signUpApi(signUp);
   if(response)
         {
          debugger
          navigate('/listProducts');

         console.log(response.data);
      }
    else{
        alert("נתונים שגויים")
         }
     
    
  
  };

  return (
    <Box component="form" onSubmit={handleSubmit} dir="rtl">
      <div id="divLogin">
        <div id="login" className="login-form-container">
          <header id="h">הרשמה למערכת</header>
          <fieldset id="feild">
            <div className="input-wrapper">
              <input
                id="in"
                type="text"
                placeholder="שם"
                onChange={(e) =>
                    setSignUp({ ...signUp, customerName: e.target.value })
                }
              />
               
            </div> <div className="input-wrapper">
              <input
                id="in"
                type="text"
                placeholder="מיל"
                onChange={(e) =>
                    setSignUp({ ...signUp, email: e.target.value })
                }
              />
            </div>
            <div className="input-wrapper">
              <input
                id="in"
                type="password"
                placeholder="סיסמה"
                onChange={(e) =>
                    setSignUp({ ...signUp, passward: e.target.value })
                }
              />
            </div>
            <Button id="continue" type="submit">
              כניסה
            </Button>
          </fieldset>
        </div>
      </div>
    </Box>
  );
};

export default SignUp;
