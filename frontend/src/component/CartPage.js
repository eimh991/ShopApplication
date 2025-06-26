import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";
import '../App.css'; 

const CartPage = () => {
  const [cartProducts, setCartProducts] = useState([]);
  const [errorMessage, setErrorMessage] = useState("");
  const navigate = useNavigate();
  const [loading, setLoading] = useState(false);

  const getTotalPrice = () => {
    return cartProducts.reduce((total, product) => {
      return total + product.price * product.quantity;
    }, 0);
  };

  useEffect(() => {
    const fetchCartProducts = async () => {
      try {
        const userResponse = await axios.get("https://localhost:5260/api/User/getme");
        const user = userResponse.data;

        if (!user.id) {
          navigate("/auth");
          return;
        }
        
        const cartResponse = await axios.get(
          `https://localhost:5260/api/CartItem/CartProducts?userId=${user.id}`
        );
        setCartProducts(cartResponse.data);
        console.log(cartResponse.data);
      } catch (error) {
        console.error("Ошибка при загрузке данных корзины:", error);
        navigate("/"); 
      }
    };

    fetchCartProducts();
  }, [navigate]);

  const handlePurchase = async () => {
    const totalCost = getTotalPrice();
    try {
      const userResponse = await axios.get("https://localhost:5260/api/User/getme");
      const user = userResponse.data;

      if (!user.id) {
        navigate("/auth");
        return;
      }

      setLoading(true);
      setErrorMessage("");

      const checkMoneyResponse = await axios.get(
        `https://localhost:5260/api/User/checkmoney?userId=${user.id}&cartCoast=${totalCost}`
      );

      
      if (checkMoneyResponse.data) {
        const createOrderResponse = await axios.post(
          `https://localhost:5260/api/Order?userId=${user.id}`,
          {}
      );
      if (createOrderResponse.status === 200) {
        console.log("Заказ успешно создан!");
        alert("✅ Заказ успешно оформлен!");
        // Можно добавить логику очистки корзины или редиректа
      } else {
        console.error("Ошибка при создании заказа:", createOrderResponse);
        setErrorMessage("❌ Ошибка при оформлении заказа.");
    }
      } else {
        // Если средств не хватает
        setErrorMessage("❌ Недостаточно средств на счете. Пополните баланс через любой удобный сервис.");
      }
    } catch (error) {
      console.error("Ошибка при покупке:", error);
      alert("Произошла ошибка при проверке баланса.");
    } finally {
      setLoading(false);
    }
  };


  
/*
  return (
    <div className="container">
      <h1 className="text-center my-4">Корзина</h1>
      {errorMessage && (
        <div className="alert alert-danger text-center" role="alert">
          {errorMessage} <br />
          <a href="https://www.tinkoff.ru/" target="_blank" rel="noopener noreferrer" className="text-primary">
            🔗 Пополнить через Тинькофф
          </a>
          {" | "}
          <a href="https://qiwi.com/" target="_blank" rel="noopener noreferrer" className="text-primary">
            🔗 Пополнить через QIWI
          </a>
        </div>
      )}
      {cartProducts.length === 0 ? (
      <div className="d-flex justify-content-center align-items-center" style={{ height: '300px' }}>
        <div className="card text-center p-4 shadow" style={{ maxWidth: '400px' }}>
          <div style={{ fontSize: '3rem' }}>🛒</div>
          <h4 className="mt-3">Ваша корзина пуста</h4>
          <p className="text-muted">Добавьте товары, чтобы увидеть их здесь.</p>
        </div>
      </div>
      ) : (
        <>
          {cartProducts.map((product) => (
            <div key={product.productId} className="card my-3">
              <div className="row g-0">
                <div className="col-md-4">
                  <img
                    src={`https://localhost:5260/images/${product.imagePath}`}
                    alt={product.name}
                    className="img-fluid rounded-start"
                    style={{ width: "220px", height: "220px", objectFit: "cover" }}
                  />
                </div>
                <div className="col-md-8">
                  <div className="card-body" style={{ padding: "15px" }}>
                  <h5 className="card-title" style={{ fontSize: "20px" }}>
                    <a 
                      href={`https://localhost:5260/product/${product.productId}`} 
                      target="_blank" 
                      rel="noopener noreferrer"
                      style={{ color: "inherit", textDecoration: "none" }} // Убираем стандартные стили ссылок
                    >
                      {product.productName} 
                    </a>
                  </h5>
                  <p className="card-text" style={{ fontSize: "14px", color: "#777" }}>
                    {product.description}
                  </p>
                    <div className="d-flex justify-content-between align-items-center" style={{ marginTop: "10px" }}>
                      <p className="card-text" style={{ fontSize: "18px", fontWeight: "bold", marginBottom: "0" }}>
                        Количество: <span style={{ color: "#ff5722" }}>{product.quantity}</span>
                      </p>
                      <p className="card-text" style={{ fontSize: "18px", fontWeight: "bold", marginBottom: "0" }}>
                        Цена: <span style={{ color: "#388e3c" }}> { product.price * product.quantity} ₽</span>
                      </p>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          ))}

          <div className="d-flex justify-content-between my-4">
            <h4>Общая цена: <span style={{ color: "#388e3c" }}>{getTotalPrice()} ₽</span></h4>
              <button className="btn btn-primary" onClick={handlePurchase} disabled={loading}>
                {loading? "Обработка ..." : "Купить"}
              </button>
          </div>
        </>
      )}
    </div>
  );
};

export default CartPage;
*/
return (
    <div className="cart-container">
      <div className="cart-header">
        <h1>Ваша корзина</h1>
        {cartProducts.length > 0 && (
          <span className="cart-count">{cartProducts.length} товара</span>
        )}
      </div>

      {errorMessage && (
        <div className="error-message">
          <svg className="error-icon" viewBox="0 0 24 24">
            <path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/>
          </svg>
          <p>{errorMessage}</p>
          <div className="payment-links">
            <a href="https://www.tinkoff.ru/" target="_blank" rel="noopener noreferrer">
              <svg className="payment-icon" viewBox="0 0 24 24">
                <path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm0 18c-4.41 0-8-3.59-8-8s3.59-8 8-8 8 3.59 8 8-3.59 8-8 8zm-1-13h2v6h-2zm0 8h2v2h-2z"/>
              </svg>
              Тинькофф
            </a>
            <a href="https://qiwi.com/" target="_blank" rel="noopener noreferrer">
              <svg className="payment-icon" viewBox="0 0 24 24">
                <path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm0 18c-4.41 0-8-3.59-8-8s3.59-8 8-8 8 3.59 8 8-3.59 8-8 8zm-1-13h2v6h-2zm0 8h2v2h-2z"/>
              </svg>
              QIWI
            </a>
          </div>
        </div>
      )}

      {cartProducts.length === 0 ? (
        <div className="empty-cart">
          <svg className="cart-icon" viewBox="0 0 24 24">
            <path d="M7 18c-1.1 0-1.99.9-1.99 2S5.9 22 7 22s2-.9 2-2-.9-2-2-2zM1 2v2h2l3.6 7.59-1.35 2.45c-.16.28-.25.61-.25.96 0 1.1.9 2 2 2h12v-2H7.42c-.14 0-.25-.11-.25-.25l.03-.12.9-1.63h7.45c.75 0 1.41-.41 1.75-1.03l3.58-6.49c.08-.14.12-.31.12-.48 0-.55-.45-1-1-1H5.21l-.94-2H1zm16 16c-1.1 0-1.99.9-1.99 2s.89 2 1.99 2 2-.9 2-2-.9-2-2-2z"/>
          </svg>
          <h3>Ваша корзина пуста</h3>
          <p>Добавьте товары, чтобы увидеть их здесь</p>
          <button 
            className="continue-shopping-btn"
            onClick={() => navigate("/")}
          >
            Продолжить покупки
          </button>
        </div>
      ) : (
        <>
          <div className="cart-items">
            {cartProducts.map((product) => (
              <div key={product.productId} className="cart-item">
                <div 
                  className="product-image"
                  onClick={() => navigate(`/product/${product.productId}`)}
                >
                  <img
                    src={`https://localhost:5260/images/${product.imagePath}`}
                    alt={product.name}
                  />
                </div>
                <div className="product-info">
                  <h3 
                    className="product-name"
                    onClick={() => navigate(`/product/${product.productId}`)}
                  >
                    {product.productName}
                  </h3>
                  <p className="product-description">{product.description}</p>
                  <div className="product-meta">
                    <div className="quantity-badge">
                      {product.quantity} шт.
                    </div>
                    <div className="price-info">
                      <span className="unit-price">{product.price} ₽/шт.</span>
                      <span className="total-price">{product.price * product.quantity} ₽</span>
                    </div>
                  </div>
                </div>
              </div>
            ))}
          </div>

          <div className="cart-summary">
            <div className="summary-content">
              <div className="total-price">
                <span>Итого:</span>
                <span className="amount">{getTotalPrice()} ₽</span>
              </div>
              <button 
                className="checkout-btn"
                onClick={handlePurchase} 
                disabled={loading}
              >
                {loading ? (
                  <>
                    <svg className="spinner" viewBox="0 0 50 50">
                      <circle cx="25" cy="25" r="20" fill="none" strokeWidth="5"></circle>
                    </svg>
                    Оформление...
                  </>
                ) : "Оформить заказ"}
              </button>
            </div>
          </div>
        </>
      )}
    </div>
  );
};

export default CartPage;