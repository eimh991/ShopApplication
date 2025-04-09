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
    return <div>Loading...</div>;
  }

  return (
    <div>
      <h2>Ваши Заказы</h2>
      {orders.length === 0 ? (
        <p>У вас нет заказов</p>
      ) : (
        <ul>
          {orders.map(order => (
            <li key={order.orderId}>
              <p>Номер заказа: {order.orderId}</p>
              <p>Дата: {new Date(order.orderDate).toLocaleDateString()}</p>
              <p>Общая сумма: {order.totalAmount} руб.</p>
            </li>
          ))}
        </ul>
      )}
    </div>
  );
};

export default OrdersPage;