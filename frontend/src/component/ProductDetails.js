import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import axios from 'axios';
import '../App.css'; 

const ProductDetails = () => {
  const { id } = useParams();
  const [product, setProduct] = useState(null);
  const [userRole, setUserRole] = useState(null);
  const [userId, setUserId] = useState(null);
  const [newImage, setNewImage] = useState(null);
  const [quantity, setQuantity] = useState(1);
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [inCart, setInCart] = useState(false);
  const [cartItemQuantity, setCartItemQuantity] = useState(0);
  const [cartItemId, setCartItemId] = useState(null);

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
        setIsAuthenticated(true);
      })
      .catch((error) => {  
      console.error('Error fetching user details:', error)
      setIsAuthenticated(false);
    });
  }, []);


  useEffect(() => {
    if (userId) {
      axios.get('https://localhost:5260/api/CartItem', { 
        params: { userId }, 
        withCredentials: true 
      })
        .then((response) => {
          const cartItem = response.data.find(item => item.productId === parseInt(id));
          if (cartItem) {
            setInCart(true);
            setCartItemQuantity(cartItem.quantity);
            setCartItemId(cartItem.cartItemId);
          }
        })
        .catch((error) => console.error('Error fetching cart items:', error));
    }
  }, [userId, id]);

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
      productId: product.productId,
      name: product.name,
      description: product.description,
      price: product.price,
      userId,
      quantity,
      categoryId: product.categoryId,
      imagePath: product.imagePath,
    };
  

    try {
      const response  = await axios.post('https://localhost:5260/api/CartItem', cartItem, { withCredentials: true });
      const newCartItem = response.data;
      console.log("Добавленный товар в корзину:", newCartItem);
      setInCart(true);
      setCartItemQuantity(quantity);
      setCartItemId(newCartItem );
    } catch (error) {
      console.error('Ошибка добавления в корзину:', error);
      if (error.response) {
        console.error('Ошибка ответа сервера:', error.response.data); // Логируем подробности ошибки от сервера
      }
      alert('Не удалось добавить товар в корзину.');
    }
  };

  const handleIncreaseQuantity = async () => {
    if (!cartItemId) {
      alert('Ошибка: товар не найден в корзине.');
      return;
    }
    if (cartItemQuantity >= product.stock) {
      alert('Нельзя добавить больше товара, чем есть в наличии.');
      return;
    }
    try {
      const newQuantity = cartItemQuantity + 1;
      console.log(cartItemId + " " + newQuantity);
      await axios.put('https://localhost:5260/api/CartItem/QuentityChange', null, {
        params: {
          cartItemId: cartItemId,
          quantity: newQuantity,
        },
        withCredentials: true,
      });
  
      setCartItemQuantity(newQuantity); 
    } catch (error) {
      console.error('Ошибка при увеличении количества:', error);
      alert('Не удалось обновить количество товара в корзине.');
    }
  };

  const handleDecreaseQuantity = async () => {
    if (!cartItemId) {
      alert('Ошибка: товар не найден в корзине.');
      return;
    }
    if(cartItemQuantity <= 1) {
      alert('Нельзя уменьшить количество меньше чем  один товар');
      return;
    }
    try {
      const newQuantity = cartItemQuantity - 1;
      console.log(cartItemId + " " + newQuantity);
      await axios.put('https://localhost:5260/api/CartItem/QuentityChange', null, {
        params: {
          cartItemId: cartItemId,
          quantity: newQuantity,
        },
        withCredentials: true,
      });
  
      setCartItemQuantity(newQuantity); 
    } catch (error) {
      console.error('Ошибка при увеличении количества:', error);
      alert('Не удалось обновить количество товара в корзине.');
    }
  };

  const handleRemoveFromCart = async () => {
    try {
      await axios.delete('https://localhost:5260/api/CartItem', {
        params: { cartItemId },
        withCredentials: true,
      });
      alert('Товар удалён из корзины!');
      setInCart(false);
      setCartItemQuantity(0);
      setCartItemId(null);
    } catch (error) {
      console.error('Ошибка удаления товара из корзины:', error);
      alert('Не удалось удалить товар из корзины.');
    }
  };

  if (!product) {
    return <p>Загрузка...</p>;
  }
/*
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
          {inCart ? (
          <div style={{ textAlign: 'center', marginTop: '20px' }}>
          <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', gap: '10px' }}>
          <button
            onClick={handleDecreaseQuantity}
            style={{
              width: '50px',
              height: '50px',
              fontSize: '24px',
              fontWeight: 'bold',
              backgroundColor: '#f44336',
              color: 'white',
              border: 'none',
              borderRadius: '5px',
              cursor: 'pointer',
            }}
          >
          -
          </button>
            <span style={{ fontSize: '18px', fontWeight: 'bold' }}>{cartItemQuantity}</span>
            <button
            onClick={handleIncreaseQuantity}
            style={{
              width: '50px',
              height: '50px',
              fontSize: '24px',
              fontWeight: 'bold',
              backgroundColor: '#4CAF50',
              color: 'white',
              border: 'none',
              borderRadius: '5px',
              cursor: 'pointer',
            }}
          >
          +
        </button>
        </div>
        <div style={{ display: 'flex', alignItems: 'center', gap: '10px', marginTop: '15px' }}>
          <div
            style={{
            backgroundColor: '#d4edda',
            color: '#155724',
            border: '1px solid rgb(123, 231, 148)',
            borderRadius: '5px',
            fontSize: '16px',
            fontWeight: 'bold',
            display: 'flex',
            width: '200px',
            height: '50px',
            alignItems: 'center',
            justifyContent: 'center',
          }}
        >
        Товар уже в корзине
        </div>
        <button
            onClick={handleRemoveFromCart}
              style={{
                width: '200px',
                height: '50px',
                fontSize: '16px',
                fontWeight: 'bold',
                backgroundColor: '#f55e40',
                color: 'white',
                border: 'none',
                borderRadius: '5px',
                cursor: 'pointer',
                display: 'flex',
                alignItems: 'center',
                justifyContent: 'center',
                }}
              >
              Удалить из корзины
              </button>
        </div>
      </div>
      ) : (
        <div>
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
        )}
      </div>
    </div>
  );
};

export default ProductDetails;
*/
if (!product) {
    return (
      <div className="loading-container">
        <div className="loading-spinner"></div>
        <p>Загрузка данных о товаре...</p>
      </div>
    );
  }

  return (
    <div className="product-detail-container">
      <div className="product-image-section">
        <img 
          src={`https://localhost:5260/images/${product.imagePath}`} 
          alt={product.name} 
          className="product-main-image"
        />
        
        {userRole === 'Admin' && (
          <div className="admin-image-upload">
            <h3>Изменить изображение:</h3>
            <label className="file-upload-label">
              <input
                type="file"
                accept="image/*"
                onChange={(e) => setNewImage(e.target.files[0])}
                className="file-upload-input"
              />
              <span className="file-upload-button">Выбрать файл</span>
              {newImage && <span className="file-name">{newImage.name}</span>}
            </label>
            <button 
              onClick={handleImageUpload} 
              className="upload-button"
              disabled={!newImage}
            >
              Обновить изображение
            </button>
          </div>
        )}
      </div>
      
      <div className="product-info-section">
        <h1 className="product-title">{product.name}</h1>
        <p className="product-description">{product.description}</p>
        
        <div className="price-stock-container">
          <span className="product-price">{product.price} ₽</span>
          <span className={`product-stock ${product.stock > 0 ? 'in-stock' : 'out-of-stock'}`}>
            {product.stock > 0 ? `В наличии: ${product.stock} шт.` : 'Нет в наличии'}
          </span>
        </div>
        
        {isAuthenticated && (
          <div className="cart-controls">
            {inCart ? (
              <div className="cart-management">
                <div className="quantity-controls">
                  <button
                    onClick={handleDecreaseQuantity}
                    className="quantity-button minus"
                    disabled={cartItemQuantity <= 1}
                  >
                    -
                  </button>
                  <span className="quantity-value">{cartItemQuantity}</span>
                  <button
                    onClick={handleIncreaseQuantity}
                    className="quantity-button plus"
                    disabled={cartItemQuantity >= product.stock}
                  >
                    +
                  </button>
              </div>
              <div className="cart-status-buttons">
                <div className="in-cart-badge">
                  <svg className="cart-icon" viewBox="0 0 24 24">
                    <path d="M7 18c-1.1 0-1.99.9-1.99 2S5.9 22 7 22s2-.9 2-2-.9-2-2-2zM1 2v2h2l3.6 7.59-1.35 2.45c-.16.28-.25.61-.25.96 0 1.1.9 2 2 2h12v-2H7.42c-.14 0-.25-.11-.25-.25l.03-.12.9-1.63h7.45c.75 0 1.41-.41 1.75-1.03l3.58-6.49c.08-.14.12-.31.12-.48 0-.55-.45-1-1-1H5.21l-.94-2H1zm16 16c-1.1 0-1.99.9-1.99 2s.89 2 1.99 2 2-.9 2-2-.9-2-2-2z"/>
                  </svg>
                  Товар в корзине
                </div>
              <button
                onClick={handleRemoveFromCart}
                className="remove-from-cart-button"
              >
                Удалить из корзины
              </button>
              </div>
              </div>
            ) : (
              <div className="add-to-cart-container">
                <div className="quantity-selector">
                  <label>Количество:</label>
                  <input
                    type="number"
                    value={quantity}
                    onChange={handleQuantityChange}
                    min="1"
                    max={product.stock}
                    className="quantity-input"
                  />
                </div>
                  <button
                  onClick={handleAddToCart}
                  className="product-page-cart-button"
                  disabled={product.stock <= 0}
                  >
                    <svg className="product-cart-icon" viewBox="0 0 24 24">
                      <path d="M7 18c-1.1 0-1.99.9-1.99 2S5.9 22 7 22s2-.9 2-2-.9-2-2-2zM1 2v2h2l3.6 7.59-1.35 2.45c-.16.28-.25.61-.25.96 0 1.1.9 2 2 2h12v-2H7.42c-.14 0-.25-.11-.25-.25l.03-.12.9-1.63h7.45c.75 0 1.41-.41 1.75-1.03l3.58-6.49c.08-.14.12-.31.12-.48 0-.55-.45-1-1-1H5.21l-.94-2H1zm16 16c-1.1 0-1.99.9-1.99 2s.89 2 1.99 2 2-.9 2-2-.9-2-2-2z"/>
                    </svg>
                  Добавить в корзину
                  </button>
              </div>
            )}
          </div>
        )}
      </div>
    </div>
  );
};

export default ProductDetails;
