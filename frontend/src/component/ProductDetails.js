import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import axios from 'axios';

const ProductDetails = () => {
  const { id } = useParams();
  const [product, setProduct] = useState(null);
  const [userRole, setUserRole] = useState(null);
  const [newImage, setNewImage] = useState(null);

  useEffect(() => {
    axios.get(`https://localhost:5260/api/Product/id?productID=${id}`)
      .then((response) => setProduct(response.data))
      .catch((error) => console.error('Error fetching product details:', error));
  }, [id]);
  
  useEffect(() => {
    axios.get('https://localhost:5260/api/User/getme', { withCredentials: true })
      .then ((response) => {
        console.log('Full user data:', response.data);
        const user = response.data;
        setUserRole(user.userRole)})
      .catch((error) => console.error('Error fetching user details:', error));
  }, []);

  // Обработка отправки нового изображения
  const handleImageUpload = async () => {
    if (!newImage) return;

    const formData = new FormData();
    formData.append('productID', id); // Передаём ID продукта
    formData.append('image', newImage); // Передаём файл изображения

    try {
      await axios.put('https://localhost:5260/api/Product/imagePath', formData, {
        headers: {
          'Content-Type': 'multipart/form-data',
        },
      });
      alert('Изображение успешно обновлено!');
      // Обновляем изображение продукта
      const updatedProduct = { ...product, imagePath: newImage.name };
      setProduct(updatedProduct);
    } catch (error) {
      console.error('Ошибка обновления изображения:', error);
      alert('Не удалось обновить изображение.');
    }
  };

  if (!product) {
    return <p>Загрузка...</p>;
  }
  console.log(userRole);

  return (
    <div style={{ display: 'flex', alignItems: 'center' }}>
      <div>
        <img 
          src={`https://localhost:5260/images/${product.imagePath}`} 
          alt={product.name} 
          style={{ maxWidth: '400px', maxHeight: '400px', objectFit: 'contain' }}
        />
      </div>
      <div style={{ marginLeft: '20px' }}>
        <h1>{product.name}</h1>
        <p>{product.description}</p>
        <p>Цена: {product.price} ₽</p>
        <p>В наличии: {product.stock} шт.</p>
        {userRole === 0 && (
          <div style={{ marginTop: '20px' }}>
            <h3>Изменить изображение:</h3>
            <input
              type="file"
              accept="image/*"
              onChange={(e) => setNewImage(e.target.files[0])}
            />
            <button onClick={handleImageUpload} style={{ marginTop: '10px' }}>
              Отправить
            </button>
          </div>
        )}
      </div>
    </div>
  );
};

export default ProductDetails;

