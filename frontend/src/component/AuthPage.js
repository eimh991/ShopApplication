import React, { useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import Cookies from 'js-cookie';

const AuthPage = () => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const navigate = useNavigate();

  const handleLogin = async (e) => {
    e.preventDefault();
    setError(''); // Сбрасываем ошибку перед запросом

    try {
      const response = await axios.post(
        'https://localhost:5260/api/Auth',
        { email, password },
        { withCredentials: true } // Указываем, что работаем с куками
      );

      setTimeout(() => {
        navigate('/');
      }, 100);
      console.log('Авторизация успешна:', response.data.token);
      const token = response.data;
      Cookies.set('ck', token, { secure: true, sameSite: 'None' });
    
      
    } catch (err) {
      console.error('Ошибка авторизации:', err);
      setError('Неправильный email или пароль');
    }
  };

  return (
    <div style={{ maxWidth: '400px', margin: '0 auto', padding: '20px' }}>
      <h1>Авторизация</h1>
      <form onSubmit={handleLogin}>
        <div className="form-group">
          <label htmlFor="email">Email</label>
          <input
            type="email"
            id="email"
            className="form-control"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
          />
        </div>
        <div className="form-group">
          <label htmlFor="password">Пароль</label>
          <input
            type="password"
            id="password"
            className="form-control"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
          />
        </div>
        {error && <p style={{ color: 'red' }}>{error}</p>}
        <button type="submit" className="btn btn-primary btn-block">
          Войти
        </button>
      </form>
      <div style={{ marginTop: '10px' }}>
        <p>
          Нет аккаунта?{' '}
          <a href="/register" style={{ textDecoration: 'underline', color: 'blue' }}>
            Зарегистрируйтесь
          </a>
        </p>
      </div>
    </div>
  );
};

export default AuthPage;
