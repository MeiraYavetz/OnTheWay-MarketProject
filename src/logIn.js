import React, { useState } from 'react';
import { Button, Box } from '@mui/material';
import './logIn.css'; 
import { Navigate, useNavigate } from 'react-router-dom';
import { signIn } from '../src/api';
import axios from 'axios';

const SignIn = () => {
  const navigate = useNavigate();
  const [logIn, setLogIn] = useState({
    customerName: '',
    password: ''
  });

  const handleSubmit =  (e) => {debugger
    isLogin();
//     const response= await signIn(logIn);//
//    debugger
// if(response){debugger
//       if(logIn.password=="m214"&&logIn.customerName=="m_meira"){
//            navigate("/createStore");
//       }
//       else{
//            navigate("/listProducts");
//     }
// }
// else{
//   alert("פרטים שגויים-הקש שוב")
// }
//navigate("/listProducts");
  };
const isLogin=async()=>{
  const response=  signIn(logIn);//
  debugger
if(response){debugger
     if(logIn.password=="m214"&&logIn.customerName=="m_meira"){
          navigate("/createStore");
     }
     else{
          navigate("/listProducts");
   }
}
else{
 alert("פרטים שגויים-הקש שוב")
}

}
  return (
    <Box component="form" onSubmit={handleSubmit} dir="rtl">
      <div id="divLogin">
        <div id="login" className="login-form-container">
          <header id="h">התחברות למערכת</header>
          <fieldset id="feild">
            <div className="input-wrapper">
              <input
                id="in"
                type="text"
                placeholder="שם"
                onChange={(e) =>
                  setLogIn({ ...logIn, customerName: e.target.value })
                }
              />
            </div>
            <div className="input-wrapper">
              <input
                id="in"
                type="password"
                placeholder="סיסמה"
                onChange={(e) =>
                  setLogIn({ ...logIn, password: e.target.value })
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

export default SignIn;
