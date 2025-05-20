import { useEffect, useState } from 'react';
import axios from 'axios';
import { FaUser, FaEnvelope, FaCoins, FaUserShield } from 'react-icons/fa';
import { Link } from 'react-router-dom';

const ProfilePage = () => {
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchUserProfile = async () => {
      try {
        const authResponse = await axios.get('https://localhost:5260/api/User/getme');
        const userId = authResponse.data.id;

        const userResponse = await axios.get(`https://localhost:5260/api/User/${userId}`);
        setUser(userResponse.data);
      } catch (error) {
        console.error('Ошибка при загрузке данных пользователя:', error);
      } finally {
        setLoading(false);
      }
    };

    fetchUserProfile();
  }, []);

  if (loading) {
    return <div className="text-center mt-5">Загрузка...</div>;
  }

  if (!user) {
    return <div className="text-center mt-5 text-danger">Не удалось загрузить профиль пользователя.</div>;
  }

  return (
    <div className="container mt-4">
      <h2 className="mb-4">👤 Личный кабинет</h2>
      <div className="card shadow p-4 mx-auto" style={{ maxWidth: '500px', borderRadius: '1rem' }}>
        <div className="mb-3">
          <FaUser className="me-2 text-primary" />
          <strong>Имя пользователя:</strong> {user.userName}
        </div>
        <div className="mb-3">
          <FaEnvelope className="me-2 text-success" />
          <strong>Email:</strong> {user.email}
        </div>
        <div className="mb-3">
          <FaCoins className="me-2 text-warning" />
          <strong>Баланс:</strong> {user.balance} руб.
        </div>
        <div className="d-flex justify-content-between mt-4">
          <Link to="/cart" className="btn btn-outline-primary">
            Моя корзина
          </Link>
          <Link to="/orders" className="btn btn-outline-secondary">
            Мои заказы
          </Link>
        </div>
      </div>
    </div>
  );
};

export default ProfilePage;