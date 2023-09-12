import React, { useEffect, useState } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { Button, Box } from '@mui/material';
import Product from './prduct';
import AppBar from '@mui/material/AppBar';
import Toolbar from '@mui/material/Toolbar';
import Typography from '@mui/material/Typography';
import ModalBasket from './Modal';
import {getAllProductsAndShops} from './api';
import { addSelectedProduct, removeSelectedProduct,addSelectedShops } from './Redux/ProductsSlice';
import './listProducts.css';
import ShoppingCartCheckoutIcon from '@mui/icons-material/ShoppingCartCheckout';
import IconButton from '@mui/material/IconButton';
import { setProductList, setSelectedProducts,setShopsList } from './Redux/ProductsSlice';
import { Navigate, useNavigate } from 'react-router-dom';
import { TextField, MenuItem, Modal } from '@mui/material';
import Address from './address';

const ListProducts = () => {
  const navigate = useNavigate();
  const listShops=["11","22","33"];
  const listP = ["aaaa","bbbbb","cccc","ddddd","llll","ooo","pppp","aaaa","bbbbb","cccc","ddddd","llll","ooo","pppp","cccc","ddddd","llll","ooo","pppp"];
  const productList = useSelector((state) => state.products.productList);
  const shopsList = useSelector((state) => state.products.shopsList);
  const selectedProducts = useSelector(
    (state) => state.products.selectedProducts
  );
  const address = useSelector(
    (state) => state.products.address
  );
  const [showModalBasket, setshowModalBasket] = useState(false);
  const [showModalAddress, setshowModalAddress] = useState(false);

  const dispatch = useDispatch(); 
  const handleContinueToBuy = () => {
    setshowModalBasket(false);
  };

  const handleModalCloseAddress = () => {
    if(address!=""){
    setshowModalAddress(false);
  }
  };
  
  const handleOpenModal = () => {
    setshowModalBasket(true);
  };
  
  const handleViewShoppingBasket = () => {
    navigate("/shoppingBasket");
    setshowModalBasket(false);
  };

  useEffect(() => {
    const fetchData = async () => {
      debugger
      // let dataShops=getAllProductsAndShops().stores;
      // let dataProducts=getAllProductsAndShops().products;

      // dispatch(setShopsList(dataShops));
      // dispatch(setProductList(dataProducts));
      // dispatch(setProductList(listP));
      // dispatch(setShopsList(listShops));
      
      getAllProductsAndShops()
       .then((res) => {
        debugger
        dispatch(setShopsList(res.data.stores));
        dispatch(setProductList(res.data.products));
      })
       .catch((error) => {
       console.error(error); 
       });
    
 
    };

    fetchData();
  }, [dispatch]);

  useEffect(() => {
    setshowModalAddress(true) // Display an alert when the screen is loaded
  }, []);


  const handleBasket = (product) => {
    debugger
    const foundInProductList = productList.includes(product);
    const foundInShopsList = shopsList.includes(product);
  
    if (foundInProductList) {
      dispatch(addSelectedProduct(product));
    }
  
    if (foundInShopsList) {
      dispatch(addSelectedShops(product));
    }
  
    handleOpenModal();
  };

  const handleRemoveProduct = (product) => {
    dispatch(removeSelectedProduct(product));
  };

  return (
    <>
      <Box>
        <AppBar position="static" id="bar">
          <Toolbar>
            <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
              רשימת המוצרים
            </Typography>
            <IconButton aria-label="save" size="large" onClick={(e)=>{navigate("/shoppingBasket")}}>
            <ShoppingCartCheckoutIcon id="icon" /> 
             </IconButton>
         
          </Toolbar>
        </AppBar>
      </Box> 
      <div style={{ overflowY: 'scroll', maxHeight: 'calc(150vh - 200px)' }}>
        {productList.map((product) => (
          <Product
            nameProduct={product}
            setSelectedProducts={setSelectedProducts}
            key={product}
            screen={'shop'}
            onModal={handleOpenModal}
            onBasket={() => handleBasket(product)}
          />
        ))}
      </div>
      <AppBar
        position="fixed"
        color="primary"
        sx={{
          top: 'auto',
          bottom: 0,
          width: '50%',
          left: '50%',
          transform: 'translateX(-50%)',
          backgroundColor: '#004834',
        }}
      ></AppBar>
      {showModalBasket && (
        <ModalBasket
          message="הידד,המוצר נוסף בהצלחה לסל"
          onContinueToBuy={handleContinueToBuy}
          onViewShoppingBasket={handleViewShoppingBasket}
        />
      )}
      {showModalAddress && (
        <Modal  open ={showModalAddress} onClose={handleModalCloseAddress} center>
          <Box sx={{ position: 'absolute', top: '50%', left: '50%', transform: 'translate(-50%, -50%)', width: 400, bgcolor: 'background.paper', borderRadius: '8px', p: 2 }}>
            <Address onCreate={handleModalCloseAddress}/> 
          </Box>
        </Modal>
      )}
    </>
  );
};

export default ListProducts;
