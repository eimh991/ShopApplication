import React from 'react';
import ProductList from './component/ProductList';

const HomePage = () => {
    return (
        <div>
            <header className="jumbotron">
                <h1>Добро пожаловать в наш магазин!</h1>
            </header>
            <div>
                <ProductList />
            </div>
        </div>
    );
};

export default HomePage;

