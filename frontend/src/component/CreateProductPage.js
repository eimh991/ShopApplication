import React, { useState, useEffect } from 'react';
import axios from 'axios';

const CreateProductPage = () => {
  const [name, setName] = useState('');
  const [categoryTitle, setCategoryTitle] = useState('');
  const [description, setDescription] = useState('');
  const [price, setPrice] = useState('');
  const [stock, setStock] = useState('');
  const [image, setImageFile] = useState(null);
  const [categories, setCategories] = useState([]);

  useEffect(() => {
    const fetchCategories = async () => {
      try {
        const response = await axios.get("https://localhost:5260/api/Category", {
          withCredentials: true,
        });
        setCategories(response.data);
      } catch (error) {
        console.error("Ошибка при загрузке категорий:", error);
      }
    };

    fetchCategories();
  }, []);

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!name || !categoryTitle || !description || !price || !stock) {
      alert('Пожалуйста, заполните все поля');
      return;
    }

    const formData = new FormData();
    formData.append('Name', name);
    formData.append('CategoryTitle', categoryTitle);
    formData.append('Description', description);
    formData.append('Price', parseFloat(price)); 
    formData.append('Stock', parseInt(stock, 10)); 
    if (image) {
      formData.append("Image", image);
    }

    console.log([...formData.entries()]);

    try {
      const response = await axios.post('https://localhost:5260/api/Product', formData, {
        withCredentials: true,
        headers: {
          'Content-Type': 'multipart/form-data',
        },
      });

      if (response.status === 200) {
        alert('Товар успешно создан!');
        // очистка формы
        setName('');
        setCategoryTitle('');
        setDescription('');
        setPrice('');
        setStock('');
        setImageFile(null);
      }
    } catch (error) {
      console.error('Ошибка при создании товара:', error);
      console.log('Ответ от сервера:', error.response?.data);
      alert('Ошибка при создании товара');
    }
  };

  return (
    <div className="container mt-4">
      <h2>Создать товар</h2>
      <form onSubmit={handleSubmit}>
        <div className="mb-3">
          <label className="form-label">Название</label>
          <input
            type="text"
            className="form-control"
            value={name}
            onChange={(e) => setName(e.target.value)}
          />
        </div>

        <div className="mb-3">
          <label className="form-label">Категория</label>
          <select
            className="form-select"
            value={categoryTitle}
            onChange={(e) => setCategoryTitle(e.target.value)}
          >
            <option value="">Выберите категорию</option>
            {categories.map((cat) => (
              <option key={cat.categoryId} value={cat.categoryName}>
                {cat.categoryName}
              </option>
            ))}
          </select>
        </div>

        <div className="mb-3">
          <label className="form-label">Описание</label>
          <textarea
            className="form-control"
            value={description}
            onChange={(e) => setDescription(e.target.value)}
          />
        </div>

        <div className="mb-3">
          <label className="form-label">Цена</label>
          <input
            type="number"
            className="form-control"
            value={price}
            onChange={(e) => setPrice(e.target.value)}
          />
        </div>

        <div className="mb-3">
          <label className="form-label">Количество на складе</label>
          <input
            type="number"
            className="form-control"
            value={stock}
            onChange={(e) => setStock(e.target.value)}
          />
        </div>

        <div className="mb-3">
          <label className="form-label">Изображение</label>
          <input
            type="file"
            className="form-control"
            onChange={(e) => setImageFile(e.target.files[0])}
            accept="image/*"
          />
        </div>

        <button type="submit" className="btn btn-success">
          ✅ Создать товар
        </button>
      </form>
    </div>
  );
};

export default CreateProductPage;