import React from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import Navbar from './Navbar';
import HomePage from './HomePage';
import AuthPage from './component/AuthPage';
import Footer from './Footer';
import RegistrationPage from './component/RegistrationPage';

const App = () => {
  return (
      <Router>
          <Navbar />
          <Routes>
              <Route path="/" element={<HomePage />} />
              <Route path="/auth" element={<AuthPage />} />
              <Route path="/register" element={<RegistrationPage />} />
              <Route path="/cart" element={<div>Корзина</div>} />
              <Route path="/search" element={<div>Поиск</div>} />
          </Routes>
          <Footer />
      </Router>
  );
};

export default App;

