import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { useNavigate, useParams } from 'react-router-dom';

const ChangePricePage = () => {
  const { id } = useParams(); // получаем id из URL
  const navigate = useNavigate();

  const [price, setPrice] = useState('');
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    if (!id) {
      setError('ID продукта не задан');
      setLoading(false);
      return;
    }

    const fetchProduct = async () => {
      try {
        const response = await axios.get(`https://localhost:5260/api/Product/${id}`, {
          withCredentials: true,
        });
        setPrice(response.data.price);
        setLoading(false);
      } catch (err) {
        setError('Ошибка загрузки продукта');
        setLoading(false);
      }
    };

    fetchProduct();
  }, [id]);

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!price) {
      alert('Цена не может быть пустой');
      return;
    }

    const formData = new FormData();
    formData.append('productId', id);
    formData.append('price', price);

    try {
      await axios.put('https://localhost:5260/api/Product/price', formData, {
        withCredentials: true,
        headers: { 'Content-Type': 'multipart/form-data' },
      });
      alert('Цена успешно обновлена');
      navigate('/admin/products');
    } catch (err) {
      alert('Ошибка при обновлении цены');
    }
  };

  if (loading) return <p>Загрузка...</p>;
  if (error) return <p>{error}</p>;

  return (
    <div className="container mt-4">
      <h2>Изменение цены продукта</h2>
      <form onSubmit={handleSubmit}>
        <div className="mb-3">
          <label htmlFor="price" className="form-label">Новая цена</label>
          <input
            id="price"
            type="number"
            className="form-control"
            value={price}
            onChange={(e) => setPrice(e.target.value)}
            required
            min="0"
            step="0.01"
          />
        </div>
        <button type="submit" className="btn btn-primary">Обновить цену</button>
      </form>
    </div>
  );
};

export default ChangePricePage;