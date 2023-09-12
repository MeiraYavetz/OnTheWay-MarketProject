import React, { useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import Product from './prduct';
import { setSelectedProducts,setShopsList,setSelectedShopsList,shopsList,addSelectedShops,setListMap } from './Redux/ProductsSlice';
import AddLocationIcon from '@mui/icons-material/AddLocation';
import Button from '@mui/material/Button';
import ShoppingCartCheckoutIcon from '@mui/icons-material/ShoppingCartCheckout';
import AppBar from '@mui/material/AppBar';
import Box from '@mui/material/Box';
import ModalBasket from './Modal';
import Toolbar from '@mui/material/Toolbar';
import Typography from '@mui/material/Typography';
import IconButton from '@mui/material/IconButton';
import {createRoute} from './api';
import { TextField, MenuItem, Modal } from '@mui/material';
import SignIn from './logIn';
import DogAddressesMap from './map';
import { Navigate, useNavigate } from 'react-router-dom';
import axios from 'axios';
import './shoppingBasket.css'
const ShoppingBasket = () => {
  const navigate = useNavigate();

  const selectedProducts = useSelector((state) => state.products.selectedProducts);
  const selectedShopsList = useSelector((state) => state.products.selectedShopsList);
  const shopsList = useSelector((state) => state.products.shopsList);
  const startingPlace  = useSelector((state) => state.products.address);

  const dispatch = useDispatch();
const [screen,setScreen]=useState(false);
const [showModalBasket, setshowModalBasket] = useState(false);
const [objReq,setObjReq]=useState({});
const [showConfirmation, setShowConfirmation] = useState(false);
const [map, setMap] = useState(false);

  const handleRemoveProduct = (product) => {
    debugger
    // dispatch(setSelectedProducts(selectedProducts.filter((p) => p !== product)));
    const foundInProductList = selectedProducts.includes(product);
    const foundInShopsList = selectedShopsList.includes(product);
  
    if (foundInProductList) {
      dispatch(setSelectedProducts(selectedProducts.filter((p) => p !== product)));
    }
  
    if (foundInShopsList) {
      dispatch(setSelectedShopsList(selectedShopsList.filter((p) => p !== product)));
    }
  };
  const handleChange = (selected,key) => {
  
    setObjReq((prev) => ({
      ...prev,
      [key]: selected,
    }));
 
    };
  const handleCreate = () => {
    handleChange(selectedProducts,"products");
    handleChange(selectedShopsList,"stores")
    handleChange(startingPlace,"startingPlace")
    setShowConfirmation(true);


  };
  const handleBasket = (product) => {
    debugger
  
      dispatch(addSelectedShops(product));
    
  
    handleOpenModal();
  };
  const handleOpenModal = () => {
    setshowModalBasket(true);
  };
  const handleContinueToBuy = () => {
    setshowModalBasket(false);
  };
  const handleConfirmation = async() => {
    setShowConfirmation(false);
    debugger
    try {
      debugger
      navigate('/map');
      const response =await  axios.post(`https://localhost:7028/api/TSP`, objReq);
      debugger
      console.log(response.data); // או עשה שימוש בנתונים המתקבלים בצורה אחרת
       dispatch(setListMap(response.data));
     //alert("מחשב מסלול בעבורך");
      
    } catch (error) {
      console.error(error);
    }

  
  
  
   
  };
  const handleCancelConfirmation = () => {
    setShowConfirmation(false);
  };
  const handleClose = () => {
    setScreen(false);
    setshowModalBasket(false);
  };
  return (
    <div>
            <Box>
        <AppBar position="static" id="bar">
          <Toolbar>
          {screen&&
            <>
 <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
              רשימת חנויות
            </Typography> 
            <IconButton aria-label="save" size="large" onClick={(e)=>{setScreen(false)}}>
            <ShoppingCartCheckoutIcon id="icon" /> 
             </IconButton>
         
       </>
            } 
                   {!screen&&
            <>
 <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
              סל הקניות
            </Typography>
       </>
            } 
            
           
          </Toolbar>
        </AppBar>
      </Box> 
       {!screen&&
            <>
           
           
      {selectedProducts.map((l) => (
        <Product
          nameProduct={l}
          setSelectedProducts={setSelectedProducts}
          key={l}
          screen={'ShoppingBasket'}
          onBasket={handleRemoveProduct}
        />
      ))}
            {selectedShopsList.map((l) => (
        <Product
          nameProduct={l}
          setSelectedProducts={setSelectedProducts}
          key={l}
          screen={'ShoppingBasket'}
          onBasket={handleRemoveProduct}
        />
      ))}
       </>
            } 
              {screen&&
            <>
           
           
      {shopsList.map((l) => (
        <Product
          nameProduct={l}
          setSelectedProducts={setSelectedProducts}
          key={l}
          screen={'shop'}
          onBasket={() => handleBasket(l)}
        />
      ))}
       </>
            } 
<AppBar position="fixed" color="primary" sx={{ top: 'auto', bottom: 0 }}>
  <Toolbar sx={{ display: 'flex', justifyContent: 'center' }}>
    <Button variant="contained" color="primary" id="create" sx={{ marginRight: '10cm' }} onClick={handleCreate} disabled={selectedShopsList.length === 0 && selectedProducts.length === 0}>
      יצירת מסלול
    </Button>
    <Button variant="contained" color="primary" id="create" sx={{ marginLeft: '10cm' }} disabled={screen} onClick={(e)=>{setScreen(true)}}>
      הוספת חנויות
    </Button>
  </Toolbar>
</AppBar>
{showModalBasket && (
        <ModalBasket
          message="הידד,המוצר נוסף בהצלחה לסל"
          onContinueToBuy={handleContinueToBuy}
          onViewShoppingBasket={handleClose}
        />
      )}
     <Modal open={showConfirmation} onClose={handleCancelConfirmation}>
        <Box sx={{position: 'absolute', top: '50%', left: '50%', transform: 'translate(-50%, -50%)', width: 400, bgcolor: 'background.paper', borderRadius: '8px', p: 2 ,textAlign:'center'}}>
          <Typography variant="h6">האם אתה בטוח שסימת את קנייתך?</Typography>
         
          <Box sx={{ mt: 2 }}>
            <Button variant="outlined" onClick={handleConfirmation}>
              כן
            </Button>
            <Button variant="outlined" onClick={handleCancelConfirmation} sx={{ ml: 2 }}>
              לא
            </Button>
          </Box>
        </Box>
      </Modal>
      {map&&(
<DogAddressesMap/>
      )}
    </div>
  );
};

export default ShoppingBasket;