import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import axios from 'axios';

const ProductDetails = () => {
  const { id } = useParams();
  const [product, setProduct] = useState(null);

  useEffect(() => {
    // Загрузка данных продукта с сервера
    axios.get(`https://localhost:5260/api/product/${id}`)
      .then((response) => setProduct(response.data))
      .catch((error) => console.error('Error fetching product details:', error));
  }, [id]);

  if (!product) {
    return <p>Загрузка...</p>;
  }

  return (
    <div>
      <h1>{product.title}</h1>
      <p>{product.description}</p>
      <p>Цена: {product.price} ₽</p>
      <p>В наличии: {product.stock} шт.</p>
    </div>
  );
};

export default ProductDetails;