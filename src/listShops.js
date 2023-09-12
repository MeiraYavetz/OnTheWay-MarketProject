import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import Product from './prduct';
import { setSelectedProducts } from './Redux/ProductsSlice';

const ListShops = () => {
  const selectedProducts = useSelector((state) => state.products.selectedProducts);
  const dispatch = useDispatch();

  const handleRemoveProduct = (product) => {
    debugger
    dispatch(setSelectedProducts(selectedProducts.filter((p) => p !== product)));
  };

  return (
    <div>
      {selectedProducts.map((l) => (
        <Product
          nameProduct={l}
          setSelectedProducts={setSelectedProducts}
          key={l}
          screen={'ShoppingBasket'}
          onBasket={handleRemoveProduct}
        />
      ))}
    </div>
  );
};

export default ListShops;
