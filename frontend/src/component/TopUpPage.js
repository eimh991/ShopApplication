import { useState, useEffect } from 'react';
import axios from 'axios';

const TopUpPage = () => {
  const [selectedAmount, setSelectedAmount] = useState('');
  const [generatedCode, setGeneratedCode] = useState('');
  const [codeInput, setCodeInput] = useState('');
  const [message, setMessage] = useState('');
  const [userId, setUserId] = useState(null);

  const topUpOptions = [500, 1000, 2000, 3000, 5000, 10000];

  useEffect(() => {
    const fetchUserId = async () => {
      try {
        const response = await axios.get('https://localhost:5260/api/User/getme');
        setUserId(response.data.id);
      } catch (error) {
        console.error('Ошибка при получении userId:', error);
      }
    };
    fetchUserId();
  }, []);

  const handleGenerateCode = async () => {
    if (!selectedAmount) {
      setMessage('Пожалуйста, выберите сумму');
      return;
    }

    try {
      const response = await axios.post(`https://localhost:5260/api/TopUp`, parseFloat(selectedAmount), {
         headers: 
         {
            'Content-Type': 'application/json'
         }
      });
      setGeneratedCode(response.data);
      setMessage('Код успешно сгенерирован!');
    } catch (error) {
      console.error(error);
      setMessage('Ошибка при генерации кода');
    }
  };

  const handleApplyCode = async () => {
    try {
    const response = await axios.post(`https://localhost:5260/api/TopUp/applyCode`, {
      code: codeInput,
      userId: userId
    }, {
      headers: {
        'Content-Type': 'application/json'
      }
    });
    console.log(codeInput, userId);
      setMessage(response.data);
    } catch (error) {
      console.error(error);
      setMessage('Ошибка при активации кода');
    }
  };

  return (
    <div className="container mt-4">
      <h2>💳 Пополнение баланса</h2>

      <div className="card p-4 shadow mt-3">
        <h5>🔐 Генерация нового кода</h5>

        <select
          className="form-select mb-3"
          value={selectedAmount}
          onChange={(e) => setSelectedAmount(e.target.value)}
        >
          <option value="">Выберите сумму</option>
          {topUpOptions.map((amount) => (
            <option key={amount} value={amount}>
              {amount} ₽
            </option>
          ))}
        </select>

        <button className="btn btn-primary mb-3" onClick={handleGenerateCode}>
          Сгенерировать код
        </button>

        {generatedCode && <div>Ваш код: <strong>{generatedCode}</strong></div>}
      </div>

      <div className="card p-4 shadow mt-3">
        <h5>📝 Активация кода</h5>
        <input
          type="text"
          className="form-control mb-2"
          placeholder="Введите код пополнения"
          value={codeInput}
          onChange={(e) => setCodeInput(e.target.value)}
        />
        <button className="btn btn-success" onClick={handleApplyCode}>
          Применить код
        </button>
      </div>

      {message && <div className="alert alert-info mt-3">{message}</div>}
    </div>
  );
};

export default TopUpPage;