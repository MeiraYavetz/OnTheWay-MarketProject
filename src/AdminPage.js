import React, { useEffect,useState } from 'react';
import { Button, TextField, Modal, Box, Typography } from '@mui/material';
import { useSelector, useDispatch } from 'react-redux';
import { setShopsList } from './Redux/ProductsSlice';
//import {getAllProductsAndShops} from './api'
import {getAllProductsAndShops,createShop,removeShop} from './api'
import './AdminPage.css';

const StoreManager = () => {
  const [storeName, setStoreName] = useState('');
  const [productNames, setProductNames] = useState([]);
  const [city, setCity] = useState('');
  const [street, setStreet] = useState('');
  const [number, setNumber] = useState('');
  const [modalOpen, setModalOpen] = useState(false);
  const [checkedItems, setCheckedItems] = useState([]);

  const handleCheckboxChange = (event) => {debugger
    const itemName = event.target.name;

    if (event.target.checked) {
      setCheckedItems([...checkedItems, itemName]);
    } else {
      setCheckedItems(checkedItems.filter((item) => item !== itemName));
    }
  };
  const  remove  = async() => {
    checkedItems.forEach((itemName) => {
      removeShop(itemName);
    });
  };
 
  const dispatch=useDispatch();
  const shopsList = useSelector((state) => state.products.shopsList);
  useEffect(() => {
    const fetchData = async () => {
       //let dataShops=getShops().listOfShops;
     
      //dispatch(setShopsList(dataShops));
      getAllProductsAndShops()
       .then((res) => {
        debugger
        dispatch(setShopsList(res.data.stores));
        
      })
 
    };

    fetchData();
  }, [dispatch]);
  const [ObjReq, setObjReq] = useState({ });
  const handleChange = (selected,key) => {
    debugger
   setObjReq((prev) => ({
     ...prev,
     [key]: selected,
   }));
   };
   const handleCreateStore = () => {
    const address = `${street} ${number}, ${city}`;
    handleChange(productNames,"products")
    handleChange(storeName,"storeName")
    handleChange(address,"streatName")    
    
    setModalOpen(true);
  };

  const handleAddProduct = () => {
    setProductNames([...productNames, '']);
  };

  const handleProductNameChange = (index, value) => {
    const updatedProductNames = [...productNames];
    updatedProductNames[index] = value;
    setProductNames(updatedProductNames);
  };

  const handleModalClose = () => {
    setModalOpen(false);
    createShop(ObjReq);
  };

  return (
    <div className="containerAdmin">
    <div className="flex-container">
      <div className="first">    
       <Typography variant="subtitle1" component="div" className="title" textAlign="right">
          שם החנות:
        </Typography>
        <TextField
          label="Store Name"
          value={storeName}
          onChange={(e) => setStoreName(e.target.value)}
          className="store-name-field"
        />

        <div className="address-section">
          <Typography variant="h6" component="div" gutterBottom>
            כתובת:
          </Typography>
          <TextField
            label="City"
            value={city}
            onChange={(e) => setCity(e.target.value)}
            className="address-field"
          />
          <TextField
            label="Street"
            value={street}
            onChange={(e) => setStreet(e.target.value)}
            className="address-field"
          />
          <TextField
            label="Number"
            value={number}
            onChange={(e) => setNumber(e.target.value)}
            className="address-field"
          />
        </div>

        <div className="product-section">
          <Typography variant="subtitle1" component="div" gutterBottom>
            Products:
          </Typography>
          {productNames.map((productName, index) => (
            <TextField
              key={index}
              label={`Product ${index + 1}`}
              value={productName}
              onChange={(e) => handleProductNameChange(index, e.target.value)}
              className="product-field"
            />
          ))}
          <Button variant="contained" onClick={handleAddProduct} className="add-product-button">
            Add Product
          </Button>
        </div>
        <Button variant="contained" onClick={handleCreateStore} className="create-store-button">
          יצירת חנות
        </Button>
      </div>

      <div>
        
        <div className="second">
        <Typography variant="subtitle1" component="div" className="title" textAlign="right">
          בחר חנות למחיקה:
        </Typography>
    
        <div className="product-section">
        {shopsList.map((item) => (
        <div key={item}>
          <label>
            <input
              type="checkbox"
              name={item}
              onChange={handleCheckboxChange}
              checked={checkedItems.includes(item)}
            />
            {item}
          </label>
        </div>
      ))}
          
        </div>
        
        <Button onClick={remove} >מחק</Button>
      
          
        </div>
        
      </div>
</div>
      <Modal open={modalOpen} onClose={handleModalClose}>
        <Box className="modal-container">
          <Typography variant="h6" component="div" className="modal-title" gutterBottom>
            Store: {storeName}
          </Typography>
          <Typography variant="subtitle1" component="div" className="modal-section-title" gutterBottom>
            Products:
          </Typography>
          {productNames.map((productName, index) => (
            <Typography variant="body1" className="modal-product" key={index} gutterBottom>
              {productName}
            </Typography>
            
          ))}

          <Typography variant="h6" component="div" className="modal-section-title" gutterBottom>
            Address:
          </Typography>
          <Typography variant="body1" component="div" gutterBottom>
            City: {city}
          </Typography>
          <Typography variant="body1" component="div" gutterBottom>
            Street: {street}
          </Typography>
          <Typography variant="body1" component="div" gutterBottom>
            Number: {number}
          </Typography>
          <Button onClick={handleModalClose}>סיום</Button>
        </Box>
      </Modal>
    </div>
  );
};

export default StoreManager;
