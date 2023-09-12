import logo from './logo.svg';
import './App.css';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import HomePage from './homePage';
import SignIn from './logIn';
import SignUp from './signup';
import ShoppingBasket from './shoppingBasket';
import ListProducts from './listProducts';
import { Provider } from 'react-redux';
import { configureStore } from '@reduxjs/toolkit';
import productsReducer from '../src/Redux/ProductsSlice';
import ListShops from './listShops';
import StoreManager from './AdminPage';
import DogAddressesMap from './map';

function App() {
  const store = configureStore({
    reducer: {
      products: productsReducer,
    },
  });

  return (
    <Provider store={store}>
      <Router>
        <div>
          <Routes>
            <Route exact path="/" element={<HomePage />} />
            <Route exact path="/logIn" element={<SignIn />} />
            <Route exact path="/signUp" element={<SignUp />} />
            <Route exact path="/listProducts" element={<ListProducts />} />
            <Route exact path="/shoppingBasket" element={<ShoppingBasket />} />
            <Route exact path="/shops" element={<ListShops />} />
            <Route exact path="/createStore" element={<StoreManager />} />
            <Route exact path="/map" element={<DogAddressesMap />} />

          </Routes>
        </div>
      </Router>
    </Provider>
 
  );
}

export default App;
