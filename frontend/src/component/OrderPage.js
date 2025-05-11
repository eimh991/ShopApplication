import React, { useEffect, useState } from 'react';
import axios from 'axios';

const OrdersPage = () => {
  const [orders, setOrders] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchOrders = async () => {
      try {
        const userResponse = await axios.get('https://localhost:5260/api/User/getme');
        const userId = userResponse.data.id;

        const ordersResponse = await axios.get(`https://localhost:5260/api/Order?userId=${userId}`);
        setOrders(ordersResponse.data);
      } catch (error) {
        console.error('Error fetching orders:', error);
      } finally {
        setLoading(false);
      }
    };

    fetchOrders();
  }, []);

  if (loading) {
    return <div className="text-center mt-4">Загрузка...</div>;
  }

  const getImageUrl = (path) => {
    return path.startsWith('http') ? path : `https://localhost:5260//images/${path}`;
  };

  return (
    <div className="container mt-4">
      <h2 className="mb-4">Ваши заказы</h2>
      {orders.length === 0 ? (
        <p>У вас нет заказов</p>
      ) : (
        <div className="row">
          {orders.map(order => (
            <div className="col-md-6 mb-4" key={order.orderId}>
              <div className="card shadow-sm">
                <div className="card-body">
                  <h5 className="card-title">Заказ #{order.orderId}</h5>
                  <p className="card-text"><strong>Дата:</strong> {new Date(order.orderDate).toLocaleDateString()}</p>
                  <p className="card-text"><strong>Сумма:</strong> {order.totalAmount} руб.</p>
                  <hr />
                  <h6>Товары в заказе:</h6>
                  {order.orderItemDTOs.map(item => (
                    <div key={item.orderItemId} className="d-flex mb-2 border-bottom pb-2">
                      <img
                        src={getImageUrl(item.productImageUrl)}
                        alt={item.productName}
                        style={{ width: '60px', height: '60px', objectFit: 'cover', marginRight: '10px' }}
                      />
                      <div>
                        <p className="mb-1"><strong>{item.productName}</strong></p>
                        <p className="mb-0">Цена: {item.price} руб.  {item.quantity} шт.</p>
                      </div>
                    </div>
                  ))}
                </div>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default OrdersPage;