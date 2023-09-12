import { createSlice } from '@reduxjs/toolkit';

const initialState = {
  productList: [],
  shopsList: [],
  selectedProducts: [],
  selectedShopsList: [],
  address:"",
  listMap:[],
};

const productsSlice = createSlice({
  name: 'products',
  initialState,
  reducers: {
    setProductList: (state, action) => {
      state.productList = action.payload;
    },
    setAddress: (state, action) => {
      state.address = action.payload;
    },
    setListMap: (state, action) => {
      state.listMap = action.payload;
    },
    setSelectedProducts: (state, action) => {
      state.selectedProducts = action.payload;
    },
    setSelectedShopsList: (state, action) => {
      state.selectedShopsList = action.payload;
    },
    setShopsList: (state, action) => {
      state.shopsList = action.payload;
    },
    addSelectedProduct: (state, action) => {
      state.selectedProducts.push(action.payload);
    },
    addSelectedShops: (state, action) => {
      state.selectedShopsList.push(action.payload);
    },
    removeSelectedProduct: (state, action) => {
      state.selectedProducts = state.selectedProducts.filter(
        (product) => product !== action.payload
      );
    },
    removeSelectedShops: (state, action) => {
      state.selectedShopsList = state.selectedShopsList.filter(
        (shop) => shop !== action.payload
      );
    },
    
  },
});

export const {addSelectedShops,setAddress,
  setSelectedShopsList,
  setShopsList,
  setProductList,
  setSelectedProducts,
  removeSelectedShops,
  addSelectedProduct,
  removeSelectedProduct,
  setListMap,
} = productsSlice.actions;

export default productsSlice.reducer;
