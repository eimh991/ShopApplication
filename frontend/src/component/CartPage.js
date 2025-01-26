import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";

const CartPage = () => {
  const [cartProducts, setCartProducts] = useState([]);
  const [userId, setUserId] = useState(null);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchCartProducts = async () => {
      try {
        const userResponse = await axios.get("https://localhost:5260/api/User/getme");
        const user = userResponse.data;
        setUserId(user.id);
        console.log(user.id);
        if (!userId) {
          navigate("/"); // Перенаправление на главную, если пользователь не авторизован
          return;
        }
        

        // Запрос на получение товаров корзины
        const cartResponse = await axios.get(
          `https://localhost:5260/api/Cart/GetAllCartProduct?userId=${userId}`
        );
        console.log(cartResponse.data);
        setCartProducts(cartResponse.data);
      } catch (error) {
        console.error("Ошибка при загрузке данных корзины:", error);
        navigate("/"); // Перенаправление при ошибке авторизации
      }
    };

    fetchCartProducts();
  }, [navigate]);

  return (
    <div className="container">
      <h1 className="text-center my-4">Корзина</h1>
      {cartProducts.length === 0 ? (
        <p>Ваша корзина пуста.</p>
      ) : (
        cartProducts.map((product) => (
          <div key={product.productId} className="card my-3">
            <div className="row g-0">
              <div className="col-md-4">
                <img
                  src={`https://localhost:5260/${product.imageUrl}`}
                  alt={product.name}
                  className="img-fluid rounded-start"
                />
              </div>
              <div className="col-md-8">
                <div className="card-body">
                  <h5 className="card-title">{product.name}</h5>
                  <p className="card-text">{product.description}</p>
                  <p className="card-text">
                    Количество: <strong>{product.quantity}</strong>
                  </p>
                  <p className="card-text">
                    Цена: <strong>{product.price} ₽</strong>
                  </p>
                </div>
              </div>
            </div>
          </div>
        ))
      )}
    </div>
  );
};

export default CartPage;