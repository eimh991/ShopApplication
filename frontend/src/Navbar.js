import React, { useEffect, useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import axios from 'axios';

const Navbar = () => {
    const [isAuthenticated, setIsAuthenticated] = useState(false);
    const navigate = useNavigate();

    useEffect(() => {
        // Проверка аутентификации пользователя через API
        const checkAuthStatus = async () => {
            try {
                await axios.get('https://localhost:7049/api/Auth/check', { withCredentials: true });
                setIsAuthenticated(true);
            } catch (error) {
                setIsAuthenticated(false);
            }
        };
        checkAuthStatus();
    }, []);

    const handleLogout = async () => {
        try {
            // Выход из аккаунта
            await axios.post('https://localhost:7049/api/Auth/logout', {}, { withCredentials: true });
            setIsAuthenticated(false);
            navigate('/auth'); 
        } catch (error) {
            console.error('Ошибка при выходе из учетной записи:', error);
        }
    };

    return (
        <nav className="navbar navbar-expand-lg navbar-light" style={{ background: 'linear-gradient(90deg, rgba(0, 0, 255, 1) 0%, rgba(255, 255, 255, 1) 100%)' }}>
            <Link className="navbar-brand" to="/">Магазин</Link>
            <button
                className="navbar-toggler"
                type="button"
                data-bs-toggle="collapse"
                data-bs-target="#navbarNav"
                aria-controls="navbarNav"
                aria-expanded="false"
                aria-label="Toggle navigation"
            >
                <span className="navbar-toggler-icon"></span>
            </button>
            <div className="collapse navbar-collapse" id="navbarNav">
                <ul className="navbar-nav ms-auto">
                    <li className="nav-item">
                        <Link className="nav-link" to="/">Главная</Link>
                    </li>
                    <li className="nav-item">
                        <Link className="nav-link" to="/cart">Корзина</Link>
                    </li>
                    {isAuthenticated ? (
                        <li className="nav-item">
                            <button className="btn btn-link nav-link" onClick={handleLogout}>Выход</button>
                        </li>
                    ) : (
                        <li className="nav-item">
                            <Link className="nav-link" to="/auth">Вход</Link>
                        </li>
                    )}
                </ul>
            </div>
        </nav>
    );
};

export default Navbar;

