import React from 'react';

const Footer = () => {
    return (
        <footer style={{ backgroundColor: 'rgb(241, 232, 201)' }} className="py-3">
            <div className="container text-center">
                <p>© 2025 Магазин - Все права защищены</p>
                <p>
                    <a href="/privacy-policy">Политика конфиденциальности</a> | 
                    <a href="/terms-of-service"> Условия использования</a>
                </p>
            </div>
        </footer>
    );
};

export default Footer;