import axios from 'axios';
import { useState, useEffect } from 'react';
import { Link, useNavigate } from 'react-router-dom';

function EditProductsListPage() {
  const [products, setProducts] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const navigate = useNavigate();

  useEffect(() => {
    async function fetchData() {
      try {
        const authRes = await axios.get('https://localhost:5260/api/User/getme');
        const role = authRes.data.role;
        if (!['Admin'].includes(role)) {
          navigate('/');
          return;
        }

        const productsRes = await axios.get('https://localhost:5260/api/Product', {
          params: { paginateSize: 10000, page: 1 }
        });
        setProducts(productsRes.data);
      } catch (err) {
        setError('Ошибка при загрузке данных');
      } finally {
        setLoading(false);
      }
    }

    fetchData();
  }, [navigate]);

  if (loading) return <p>Загрузка...</p>;
  if (error) return <p>{error}</p>;

  return (
    <div>
      <h2>Редактировать товары</h2>
      <div className="row">
        {products.map(product => (
          <div key={product.id} className="card m-2" style={{ width: '18rem' }}>
             <img
                src={`https://localhost:5260/images/${product.imagePath}`}
                className="card-img-top"
                alt={product.name}
                style={{ height: '200px', objectFit: 'cover' }}
              />
            <div className="card-body">
              <h5 className="card-title">{product.title}</h5>
              <p className="card-text">{product.description}</p>
              <Link to={`/admin/edit-products/${product.id}`} className="btn btn-primary m-1">
                Изменить описание
              </Link>
              <Link to={`/admin/change-price/${product.id}`} className="btn btn-secondary m-1">
                Изменить цену
              </Link>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}

export default EditProductsListPage;
