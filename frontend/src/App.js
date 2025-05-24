import React from 'react';
import { BrowserRouter , Route, Routes } from 'react-router-dom';
import Navbar from './Navbar';
import HomePage from './HomePage';
import AuthPage from './component/AuthPage';
import Footer from './Footer';
import RegistrationPage from './component/RegistrationPage';
import ProductDetails from './component/ProductDetails';
import MainPage from './component/MainPage';
import OrdersPage from './component/OrderPage';
import AllProductsPage from './component/AllProductsPage';
import CartPage from './component/CartPage';
import ProfilePage from './component/ProfilePage';
import CreateProductPage from './component/CreateProductPage';
import DeleteProductsPage from './component/DeleteProductsPage';

const App = () => {
  return (
    <div style={{ backgroundColor: 'rgb(241, 232, 201)', minHeight: '100vh' }}>
      <BrowserRouter>
          <Navbar />
          <Routes>
              <Route path="/" element={<HomePage/>} />
              <Route path="/auth" element={<AuthPage />} />
              <Route path="/register" element={<RegistrationPage />} />
              <Route path="/cart" element={<CartPage />} />
              <Route path="/search" element={<div>Поиск</div>} />
              <Route path="/" element={<MainPage />} />
              <Route path="/product/:id" element={<ProductDetails />} />
              <Route path="/products" element={<AllProductsPage />} />
              <Route path="/profile" element={<ProfilePage/>} />
              <Route path="/orders" element={<OrdersPage/>} />
              <Route path="/create-product" element={<CreateProductPage />} />
              <Route path="/admin/delete-products" element={<DeleteProductsPage />} />
          </Routes>
          <Footer />
      </BrowserRouter>
      </div>
  );
};

export default App;

