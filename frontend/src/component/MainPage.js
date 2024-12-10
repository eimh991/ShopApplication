import React, { useEffect, useState } from 'react';
import axios from 'axios';
import ProductCard from './ProductCard';

const MainPage = () => {
  const [products, setProducts] = useState([]);

  useEffect(() => {
    // Загрузка списка продуктов с сервера
    axios.get('https://localhost:5260/api/products')
      .then((response) => setProducts(response.data))
      .catch((error) => console.error('Error fetching products:', error));
  }, []);

  return (
    <div>
      <h1>Последние товары</h1>
      <div style={{ display: 'flex', flexWrap: 'wrap', gap: '16px' }}>
        {products.map((product) => (
          <ProductCard key={product.id} product={product} />
        ))}
      </div>
    </div>
  );
};

export default MainPage;