import React, { useEffect, useState } from 'react';
import axios from 'axios';
import ProductCard from './ProductCard';
import '../App.css';


const MainPage = () => {
  const [products, setProducts] = useState([]);

    useEffect(() => {
    axios.get('https://localhost:5260/api/products')
      .then((response) => setProducts(response.data))
      .catch((error) => console.error('Error fetching products:', error));
  }, []);


  return (
    <div className="container">
      <h1 className="title">Последние товары</h1>
      <div className="productsGrid">
        {products.map((product) => (
          <ProductCard key={product.productId} product={product} />
        ))}
      </div>
    </div>
  );
};

export default MainPage;