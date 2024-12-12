import React from 'react';
import { useNavigate } from 'react-router-dom';

const ProductCard = ({ product }) => {
  const navigate = useNavigate();

  const handleCardClick = () => {
    console.log(`Navigating to product with ID: ${product.productId}`);
    navigate(`/product/${product.productId}`); 
  };

  return (
    <div
      onClick={handleCardClick}
      style={{
        border: '1px solid #ccc',
        padding: '16px',
        cursor: 'pointer',
        backgroundColor: '#f9f9f9',
      }}
    >
      <h3>{product.title}</h3>
      <p>{product.description}</p>
      <p>Цена: {product.price} ₽</p>
    </div>
  );
};

export default ProductCard;