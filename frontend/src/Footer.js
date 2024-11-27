import React from 'react';

const Footer = () => {
    return (
        <footer className="bg-light py-3">
            <div className="container text-center">
                <p>© 2024 Магазин - Все права защищены</p>
                <p>
                    <a href="/privacy-policy">Политика конфиденциальности</a> | 
                    <a href="/terms-of-service"> Условия использования</a>
                </p>
            </div>
        </footer>
    );
};

export default Footer;