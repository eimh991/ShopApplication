import React from 'react';
import { BrowserRouter , Route, Routes } from 'react-router-dom';
import Navbar from './Navbar';
import HomePage from './HomePage';
import AuthPage from './component/AuthPage';
import Footer from './Footer';
import RegistrationPage from './component/RegistrationPage';
import ProductDetails from './component/ProductDetails';
import MainPage from './component/MainPage';
import ProductCard from './component/ProductCard';
import AllProductsPage from './component/AllProductsPage';

const App = () => {
  return (
    <div style={{ backgroundColor: 'rgb(241, 232, 201)', minHeight: '100vh' }}>
      <BrowserRouter>
          <Navbar />
          <Routes>
              <Route path="/" element={<HomePage />} />
              <Route path="/auth" element={<AuthPage />} />
              <Route path="/register" element={<RegistrationPage />} />
              <Route path="/cart" element={<div>Корзина</div>} />
              <Route path="/search" element={<div>Поиск</div>} />
              <Route path="/" element={<MainPage />} />
              <Route path="/product/:id" element={<ProductDetails/>} />
              <Route path="/products" element={<AllProductsPage />} />
          </Routes>
          <Footer />
      </BrowserRouter>
      </div>
  );
};

export default App;

