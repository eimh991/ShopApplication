import React from 'react';

const ProductCard = ({ product }) => {
    return (
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
    );
};

export default ProductCard;