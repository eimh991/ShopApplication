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
  //const [cartItems, setCartItems] = useState([]);
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
    // Получение корзины пользователя
    if (userId) {
      axios.get('https://localhost:5260/api/CartItem', { 
        params: { userId }, 
        withCredentials: true 
      })
        .then((response) => {
          console.log('Cart Items:', response.data);
          //setCartItems(response.data);
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
      await axios.post('https://localhost:5260/api/CartItem', cartItem, { withCredentials: true });
      alert('Товар успешно добавлен в корзину!');
      setInCart(true);
      setCartItemQuantity(quantity);
    } catch (error) {
      console.error('Ошибка добавления в корзину:', error);
      alert('Не удалось добавить товар в корзину.');
    }
  };

  const handleIncreaseQuantity = async () => {
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
          quentity: newQuantity,
        },
        withCredentials: true,
      });
  
      setCartItemQuantity(newQuantity); // Обновляем количество в корзине
    } catch (error) {
      console.error('Ошибка при увеличении количества:', error);
      alert('Не удалось обновить количество товара в корзине.');
    }
  };

  const handleDecreaseQuantity = async () => {
    if(cartItemQuantity <= 1) {
      alert('Нельзя уменьшеть количество товара меньше одного');
      return;
    }
    try {
      const newQuantity = cartItemQuantity - 1;
      console.log(cartItemId + " " + newQuantity);
      await axios.put('https://localhost:5260/api/CartItem/QuentityChange', null, {
        params: {
          cartItemId: cartItemId,
          quentity: newQuantity,
        },
        withCredentials: true,
      });
  
      setCartItemQuantity(newQuantity); // Обновляем количество в корзине
    } catch (error) {
      console.error('Ошибка при увеличении количества:', error);
      alert('Не удалось обновить количество товара в корзине.');
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
          <div
            style={{
            marginTop: '15px',
            padding: '15px',
            backgroundColor: '#d4edda',
            color: '#155724',
            border: '1px solid rgb(123, 231, 148)',
            borderRadius: '5px',
            fontSize: '16px',
            fontWeight: 'bold',
            display: 'inline-block',
          }}
        >
        Товар уже в корзине
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

