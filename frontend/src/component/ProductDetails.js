import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import axios from 'axios';

const ProductDetails = () => {
  const { id } = useParams();
  const [product, setProduct] = useState(null);
  const [userRole, setUserRole] = useState(null);
  const [userId, setUserId] = useState(null);
  const [newImage, setNewImage] = useState(null);
  const [quantity, setQuantity] = useState(1);
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  

  useEffect(() => {
    axios.get(`https://localhost:5260/api/Product/id?productID=${id}`)
      .then((response) => setProduct(response.data))
      .catch((error) => console.error('Error fetching product details:', error));
  }, [id]);
  
  useEffect(() => {
    axios.get('https://localhost:5260/api/User/getme', { withCredentials: true })
      .then ((response) => {
        const user = response.data;
        setUserRole(user.role);
        setUserId(user.id);
        console.log(userId);
        setIsAuthenticated(true);
      })
      .catch((error) => {  
      console.error('Error fetching user details:', error)
      setIsAuthenticated(false);
    });
  }, []);

  // Обработка отправки нового изображения
  const handleImageUpload = async () => {
    if (!newImage) return;

    const formData = new FormData();
    formData.append('productID', id); // Передаём ID продукта
    formData.append('image', newImage); // Передаём файл изображения

    try {
      await axios.put('https://localhost:5260/api/Product/imagePath', formData);
      const updatedImagePath = `${product.imagePath}?timestamp=${new Date().getTime()}`;
      setProduct((prev) => ({ ...prev, imagePath: updatedImagePath }));
      window.location.reload();
    } catch (error) {
      console.error('Ошибка обновления изображения:', error);
    }
  };


  const handleQuantityChange = (e) => {
    const value = Math.min(product.stock, Math.max(1, parseInt(e.target.value) || 1));
    setQuantity(value);
  };

   // Обработка добавления в корзину
   const handleAddToCart = async () => {
    if (!userId) {
      alert('Ошибка: пользователь не авторизован.');
      return;
    }

    const cartItem = {
      productId: product.id,
      name: product.name,
      description: product.description,
      price: product.price,
      userId,
      quantity,
      categoryId: product.categoryId,
      imagePath: product.imagePath,
    };

    try {
      await axios.post('https://localhost:5260/api/CartItem', cartItem, { withCredentials: true });
      alert('Товар успешно добавлен в корзину!');
    } catch (error) {
      console.error('Ошибка добавления в корзину:', error);
      alert('Не удалось добавить товар в корзину.');
    }
  };

  if (!product) {
    return <p>Загрузка...</p>;
  }

  return (
    <div style={{ display: 'flex', alignItems: 'center' }}>
      <div>
        <img 
          src={`https://localhost:5260/images/${product.imagePath}`} 
          alt={product.name} 
          style={{ maxWidth: '400px', maxHeight: '400px', objectFit: 'contain' }}
        />
        {userRole === 'Admin' && (
          <div style={{ marginTop: '10px' }}>
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
      <div style={{ marginLeft: '20px' }}>
        <h1>{product.name}</h1>
        <p>{product.description}</p>
        <p>Цена: {product.price} ₽</p>
        <p>В наличии: {product.stock} шт.</p>
        {isAuthenticated && (
        <div style={{ marginTop: '20px' }}>
          <label>
            Количество:
            <input
              type="number"
              value={quantity}
              onChange={handleQuantityChange}
              style={{ marginLeft: '10px', width: '60px' }}
              min="1"
              max={product.stock}
            />
          </label>
          <button
            onClick={handleAddToCart}
            style={{
              marginLeft: '10px',
              padding: '10px 20px',
              backgroundColor: 'blue',
              color: 'white',
              border: 'none',
              borderRadius: '5px',
              cursor: 'pointer',
            }}
          >
            Положить в корзину
          </button>
        </div>
      )}
      </div>
    </div>
  );
};

export default ProductDetails;

