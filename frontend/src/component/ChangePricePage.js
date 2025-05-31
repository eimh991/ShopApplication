import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { useNavigate, useParams } from 'react-router-dom';

const ChangePricePage = () => {
  const { id } = useParams(); // получаем id из URL
  const navigate = useNavigate();

  console.log('id from useParams:', id);
  console.log('Number(id):', Number(id));

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

    if (!price || price <= 0) {
      alert('Цена должна быть больше 0');
      return;
    }

    try {
      await axios.put(
        'https://localhost:5260/api/Product/price',
        {
          productId: Number(id),
          newPrice: parseFloat(price),
        },
        {
          withCredentials: true,
          headers: { 'Content-Type': 'application/json' },
        }
      );
      alert('Цена успешно обновлена');
      navigate(`/product/${id}`);  
    } catch (err) {
      console.log({ productId: Number(id), newPrice: parseFloat(price) });
      alert('Ошибка при обновлении цены');
      console.error(err);
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