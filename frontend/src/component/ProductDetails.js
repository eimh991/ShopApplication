import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import axios from 'axios';

const ProductDetails = () => {
  const { id } = useParams();
  const [product, setProduct] = useState(null);

  useEffect(() => {
    axios.get(`https://localhost:5260/api/Product/id?productID=${id}`)
      .then((response) => setProduct(response.data))
      .catch((error) => 
        console.error('Error fetching product details:', error));
  }, [id]);

  if (!product) {
    return <p>Загрузка...</p>;
  }

  return (
    <div style={{ display: 'flex', alignItems: 'center' }}>
      <div>
        <img 
          src={product.imagePath} 
          alt={product.name} 
          style={{ maxWidth: '600px', maxHeight: '800px', objectFit: 'contain' }}
        />
      </div>
      <div style={{ marginLeft: '20px' }}>
        <h1>{product.name}</h1>
        <p>{product.description}</p>
        <p>Цена: {product.price} ₽</p>
        <p>В наличии: {product.stock} шт.</p>
      </div>
    </div>
  );
};

export default ProductDetails;

