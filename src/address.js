import React, { useState } from 'react';
import { TextField, Box, Typography } from '@mui/material';
import { useSelector, useDispatch } from 'react-redux';
import Button from '@mui/material/Button';
import {setAddress } from './Redux/ProductsSlice';
const Address = ({onCreate}) => {
  const [street, setStreet] = useState('');
  const [numOfStreet, setNumOfStreet] = useState('');
  const dispatch = useDispatch(); 
  const handleFieldChange = (field, value) => {
    if (field === 'street') {
      setStreet(value);
    } else if (field === 'numOfStreet') {
      setNumOfStreet(value);
    }
  };

  const handleFormSubmit = () => {
    const formattedAddress = `{"${street} ${numOfStreet}, ירושלים"}`;
    dispatch(setAddress(formattedAddress));
    
    onCreate()
  };

  const isFormFilled = street.trim() !== '' && numOfStreet.trim() !== '';

  return (
<Box sx={{ p: 4 }}>
      <Typography variant="h5" id="Title" sx={{ mb: 2, textAlign: 'center' }}>
        כתובת התחלתית
      </Typography>
      <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center' }}>
        <TextField
          fullWidth
          label="רחוב"
          value={street}
          onChange={(e) => handleFieldChange('street', e.target.value)}
          sx={{ mb: 2 }}
          required
        />
        
      </Box>
      <TextField
        fullWidth
        type="number"
        label="מספר"
        value={numOfStreet}
        onChange={(e) => handleFieldChange('numOfStreet', e.target.value)}
        sx={{ mb: 2 }}
        required
      />
      <Button variant="outlined" onClick={handleFormSubmit} disabled={!isFormFilled} sx={{ width: '100%', display: 'flex', justifyContent: 'center', alignItems: 'center' }}>
          create
        </Button>
    </Box>
  );
};

export default Address;
