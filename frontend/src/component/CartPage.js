import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";

const CartPage = () => {
  const [cartProducts, setCartProducts] = useState([]);
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
        console.log(user.id);

        if (!user.id) {
          navigate("/auth"); // Перенаправление на главную, если пользователь не авторизован
          return;
        }
        

        // Запрос на получение товаров корзины
        const cartResponse = await axios.get(
          `https://localhost:5260/api/CartItem/CartProducts?userId=${user.id}`
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

      const checkMoneyResponse = await axios.get(
        `https://localhost:5260/api/User/checkmoney?userId=${user.id}&cartCoast=${totalCost}`
      );

      if (checkMoneyResponse.data) {
        // Если средств достаточно, ничего не делаем
        console.log("Баланс достаточен для покупки.");
        // Логика дальнейших действий будет добавлена позже
      } else {
        // Если средств не хватает
        alert("Недостаточно средств на счете.");
      }
    } catch (error) {
      console.error("Ошибка при покупке:", error);
      alert("Произошла ошибка при проверке баланса.");
    } finally {
      setLoading(false);
    }
  };


  

  return (
    <div className="container">
      <h1 className="text-center my-4">Корзина</h1>
      {cartProducts.length === 0 ? (
        <p>Ваша корзина пуста.</p>
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
                    <h5 className="card-title" style={{ fontSize: "20px" }}>{product.name}</h5>
                    <p className="card-text" style={{ fontSize: "14px", color: "#777" }}>
                      {product.description}
                    </p>
                    <div className="d-flex justify-content-between align-items-center" style={{ marginTop: "10px" }}>
                      <p className="card-text" style={{ fontSize: "18px", fontWeight: "bold", marginBottom: "0" }}>
                        Количество: <span style={{ color: "#ff5722" }}>{product.quantity}</span>
                      </p>
                      <p className="card-text" style={{ fontSize: "18px", fontWeight: "bold", marginBottom: "0" }}>
                        Цена: <span style={{ color: "#388e3c" }}>{product.price} ₽</span>
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