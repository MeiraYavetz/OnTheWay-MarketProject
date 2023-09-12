import React, { useState } from 'react';
import { Button, Box } from '@mui/material';
import Card from '@mui/material/Card';
import CardActions from '@mui/material/CardActions';
import CardContent from '@mui/material/CardContent';
import Typography from '@mui/material/Typography';
import IconButton from '@mui/material/IconButton';
import DeleteIcon from '@mui/icons-material/Delete';
import AddShoppingCartIcon from '@mui/icons-material/AddShoppingCart';
import './prduct.css'; // Import the CSS file
import Modal from './Modal'; // Import the Modal component
import { useSelector, useDispatch } from 'react-redux';
import { setProductList,setSelectedProducts } from './Redux/ProductsSlice';


const Product = ({ nameProduct,
  setSelectedProducts,
  screen,
  onModal,
  onClose,
  onBasket,}) => {
  const [showModal, setShowModal] = useState(false);
  const dispatch = useDispatch(); 

  const handleContinueToBuy = () => {
    setShowModal(false);
  };

  const handleViewShoppingBasket = () => {
    setShowModal(false);

  };
  
  const handleBasket = () => {
    debugger;
    onBasket(nameProduct);
  };

  const handleClick = () => {
    setSelectedProducts((prevProducts) => [...prevProducts, nameProduct]);
  };
  const handleRemoveProduct = () => {
    onBasket(nameProduct); 
  };


  return (
    <Card id="cardcss">
      <CardContent>
        <Typography sx={{ fontSize: 20 }} className="headercss">
          <b>{nameProduct}</b>
        </Typography>
      </CardContent>
      <CardActions>
        {screen === 'shop' && (
          <IconButton
            color="primary"
            aria-label="add to shopping cart"
            id="ll"
            onClick={handleBasket}
          >
            <AddShoppingCartIcon />
          </IconButton>
        )}
        {screen === 'ShoppingBasket' && (
          <IconButton
          aria-label="delete"
          size="large"
          id="ll"
          onClick={handleRemoveProduct}
          >
            <DeleteIcon fontSize="inherit" />
          </IconButton>
        )}
      </CardActions>

    </Card>
  );
};

export default Product;
