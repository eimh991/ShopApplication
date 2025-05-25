import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';

const DeleteProductsPage = () => {
  const [products, setProducts] = useState([]);
  const [userRole, setUserRole] = useState(null);
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();

  useEffect(() => {
    const checkAccessAndFetchProducts = async () => {
      try {
        const authRes = await axios.get('https://localhost:5260/api/User/getme');
        if (!['Admin'].includes(authRes.data.role)) {
          return navigate('/');
        }
        setUserRole(authRes.data.role);

        const productsRes = await axios.get('https://localhost:5260/api/Product?paginateSize=10000&page=1');
        setProducts(productsRes.data);
      } catch (err) {
        console.error('Ошибка загрузки:', err);
        navigate('/');
      } finally {
        setLoading(false);
      }
    };

    checkAccessAndFetchProducts();
  }, [navigate]);

  const handleDelete = async (productId) => {
    if (!window.confirm('Вы уверены, что хотите удалить этот товар?')) return;

    try {
      await axios.delete(`https://localhost:5260/api/Product?productId=${productId}`);
      setProducts(products.filter(p => p.productId !== productId));
    } catch (error) {
      console.error('Ошибка при удалении продукта:', error);
      alert('Ошибка при удалении. Попробуйте позже.');
    }
  };

  if (loading) return <div className="text-center mt-5">Загрузка...</div>;

  return (
    <div className="container mt-4">
      <h2 className="mb-4">🗑️ Удаление продуктов</h2>
      <div className="row">
        {products.map(product => (
          <div key={product.productId} className="col-md-4 mb-4">
            <div className="card h-100 shadow-sm">
              <img
                src={`https://localhost:5260/images/${product.imagePath}`}
                className="card-img-top"
                alt={product.name}
                style={{ height: '200px', objectFit: 'cover' }}
              />
              <div className="card-body d-flex flex-column">
                <h5 className="card-title">{product.name}</h5>
                <button
                  onClick={() => handleDelete(product.productId)}
                  className="btn btn-danger mt-auto"
                >
                  Удалить
                </button>
              </div>
            </div>
          </div>
        ))}
        {products.length === 0 && <p className="text-muted">Нет доступных товаров для удаления.</p>}
      </div>
    </div>
  );
};

export default DeleteProductsPage;