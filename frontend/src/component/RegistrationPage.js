import React, { useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';

const RegisterPage = () => {
  const [userName, setUserName] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');
  const navigate = useNavigate();

  const handleRegister = async (e) => {
    e.preventDefault();
    setError('');
    setSuccess('');

    try {
      const response = await axios.post(
        'https://localhost:5260/api/user',
        { userName, email, password },
        { headers: { 'Content-Type': 'application/json' } }
      );

      console.log('Регистрация успешна:', response);
      setSuccess('Регистрация прошла успешно! Перенаправление на страницу входа...');

      // Переход на страницу авторизации через несколько секунд
      setTimeout(() => {
        navigate('/auth');
      }, 2000);
    } catch (err) {
      console.error('Ошибка регистрации:', err);
      setError('Ошибка при регистрации. Проверьте введённые данные.');
    }
  };

  return (
    <div style={{ maxWidth: '400px', margin: '0 auto', padding: '20px' }}>
      <h1>Регистрация</h1>
      <form onSubmit={handleRegister}>
        <div className="form-group">
          <label htmlFor="userName">Имя пользователя</label>
          <input
            type="text"
            id="userName"
            className="form-control"
            value={userName}
            onChange={(e) => setUserName(e.target.value)}
            required
          />
        </div>
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
        {success && <p style={{ color: 'green' }}>{success}</p>}
        <button type="submit" className="btn btn-primary btn-block">
          Зарегистрироваться
        </button>
      </form>
      <div style={{ marginTop: '10px' }}>
        <p>
          Уже есть аккаунт?{' '}
          <a href="/auth" style={{ textDecoration: 'underline', color: 'blue' }}>
            Войдите
          </a>
        </p>
      </div>
    </div>
  );
};

export default RegisterPage;