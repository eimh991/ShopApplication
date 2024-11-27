import React, { useEffect, useState } from 'react';
import axios from 'axios';
import 'bootstrap/dist/css/bootstrap.min.css';

const ProductList = () => {
    const [products, setProducts] = useState([]);

    useEffect(() => {
        axios.get('https://localhost:7049/api/Product/last') 
            .then((response) => {
                setProducts(response.data);
            })
            .catch((error) => {
                console.error("Ошибка загрузки данных:", error.message);
                console.error("Детали ошибки:", error);
            });
    }, []);
    return (
        <div className="container mt-4">
            <h2 className="text-center mb-4">Последние товары</h2>
            <div className="row">
                {products.map((product) => (
                    <div key={product.productId} className="col-md-4">
                        <div className="card h-100">
                            <img
                                src={product.imagePath}
                                className="card-img-top"
                                alt={product.name}
                                style={{ height: '200px', objectFit: 'cover' }}
                            />
                            <div className="card-body">
                                <h5 className="card-title">{product.name}</h5>
                                <p className="card-text">{product.description}</p>
                                <p className="card-text">
                                    <strong>Цена:</strong> {product.price} ₽
                                </p>
                                <p className="card-text">
                                    <strong>В наличии:</strong> {product.stock} шт.
                                </p>
                            </div>
                        </div>
                    </div>
                ))}
            </div>
        </div>
    );
};

export default ProductList;