import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useParams, useNavigate } from 'react-router-dom';

function EditProductsPage() {
  const { id } = useParams();
  const navigate = useNavigate();

  const [description, setDescription] = useState('');
  const [product, setProduct] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    async function fetchProduct() {
      try {
        const authRes = await axios.get('https://localhost:5260/api/User/getme');
        const role = authRes.data.role;
        if (role !== 'Admin') {
          navigate('/');
          return;
        }

        const res = await axios.get(`https://localhost:5260/api/Product/${id}`);
        setProduct(res.data);
        setDescription(res.data.description); 
      } catch (err) {
        setError('Ошибка при загрузке товара');
      } finally {
        setLoading(false);
      }
    }

    fetchProduct();
  }, [id, navigate]);

  if (loading) return <p>Загрузка...</p>;
  if (error) return <p>{error}</p>;
  if (!product) return <p>Товар не найден</p>;

  async function handleSubmit(e) {
    e.preventDefault();

    try {
      const formData = new FormData();

      formData.append('ProductId', product.id);
      formData.append('Name', product.title);
      formData.append('Description', description);
      formData.append('Price', product.price.toString());
      formData.append('Stock', (product.stock || 0).toString());
      formData.append('CategoryTitle', product.categoryTitle || '');

      await axios.put('https://localhost:5260/api/Product', formData, {
        headers: { 'Content-Type': 'multipart/form-data' }
      });

      alert('Описание успешно обновлено');
      navigate('/admin/edit-products');
    } catch {
      alert('Ошибка при обновлении описания');
    }
  }

  return (
    <div>
      <h2>Изменить описание товара: {product.title}</h2>
      <form onSubmit={handleSubmit}>
        <div className="mb-3">
          <label htmlFor="description" className="form-label">Описание</label>
          <textarea
            id="description"
            className="form-control"
            value={description}
            onChange={e => setDescription(e.target.value)}
            rows={5}
            required
          />
        </div>

        <button type="submit" className="btn btn-primary">Сохранить</button>
      </form>
    </div>
  );
}

export default EditProductsPage;