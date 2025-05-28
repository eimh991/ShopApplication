import { useEffect, useState } from 'react';
import axios from 'axios';
import { FaUser, FaEnvelope, FaCoins, FaUserShield } from 'react-icons/fa';
import { Link, useNavigate } from 'react-router-dom';

const ProfilePage = () => {
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();
  const [userRole, setUserRole] = useState(null);

  useEffect(() => {
    const fetchUserProfile = async () => {
      try {
        const authResponse = await axios.get('https://localhost:5260/api/User/getme');
        const userId = authResponse.data.id;
        setUserRole(authResponse.data.role);

        const userResponse = await axios.get(`https://localhost:5260/api/User/${userId}`);
        setUser(userResponse.data);
        console.log(userRole)
      } catch (error) {
        console.error('Ошибка при загрузке данных пользователя:', error);
      } finally {
        setLoading(false);
      }
    };

    fetchUserProfile();
  }, []);

   const handleCreateProduct = () => {
    navigate('/create-product');
  };

  if (loading) {
    return <div className="text-center mt-5">Загрузка...</div>;
  }

  if (!user) {
    return <div className="text-center mt-5 text-danger">Не удалось загрузить профиль пользователя.</div>;
  }

  const handleTopUpClick = () => {
    navigate('/top-up');
  };

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
        <div className="d-flex flex-wrap justify-content-between gap-2 mt-4">
          <Link to="/cart" className="btn btn-outline-primary">
            🛒 Моя корзина
          </Link>
          <Link to="/orders" className="btn btn-outline-secondary">
            📦 Мои заказы
          </Link>
          <button onClick={handleTopUpClick} className="btn btn-outline-success">
            💰 Пополнить баланс
          </button>
        </div>
        {(userRole === 'Admin' || userRole === 'Manager') && (
          <div className="d-flex flex-wrap justify-content-start gap-2 mt-3">
            <button onClick={handleCreateProduct} className="btn btn-success">
              ➕ Создать товар
            </button>
            {userRole === 'Admin' && (
              <>
                <Link to="/admin/delete-products" className="btn btn-danger">
                  🗑️ Удалить товар
                </Link>
                <Link to="/admin/change-role" className="btn btn-warning">
                  🛠️ Сменить роль пользователя
                </Link>
              </>
            )}
          </div>
        )}
      </div>
    </div>
  );
};

export default ProfilePage;