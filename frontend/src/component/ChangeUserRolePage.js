import { useEffect, useState } from 'react';
import axios from 'axios';

const ChangeUserRolePage = () => {
  const [users, setUsers] = useState([]);
  const [selectedRoles, setSelectedRoles] = useState({});
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchUsers = async () => {
      try {
        const res = await axios.get('https://localhost:5260/api/User/all?search=');
        setUsers(res.data);
        setSelectedRoles(Object.fromEntries(res.data.map(user => [user.userId, user.role])));
      } catch (err) {
        console.error('Ошибка при получении пользователей', err);
      } finally {
        setLoading(false);
      }
    };

    fetchUsers();
  }, []);

  const handleChangeRole = async (userId) => {
  const newRole = selectedRoles[userId];
  console.log("userId:", userId);
  console.log("newRole:", newRole);

  try {
    await axios.put('https://localhost:5260/api/User/changerole', {
      userId: userId,
      newRole: newRole
    });

    alert('Роль успешно изменена');
  } catch (err) {
    alert('Ошибка при изменении роли');
    console.error(err);
  }
};

  const handleSelectChange = (userId, value) => {
    setSelectedRoles({ ...selectedRoles, [userId]: value });
  };

  if (loading) return <div className="text-center mt-5">Загрузка пользователей...</div>;

  return (
    <div className="container mt-5">
      <h2>🔧 Смена ролей пользователей</h2>
      <table className="table table-bordered mt-4">
        <thead className="table-light">
          <tr>
            <th>ID</th>
            <th>Имя</th>
            <th>Email</th>
            <th>Текущая роль</th>
            <th>Новая роль</th>
            <th>Действие</th>
          </tr>
        </thead>
        <tbody>
          {users.map(user => (
            <tr key={user.userId}>
              <td>{user.userId}</td>
              <td>{user.userName}</td>
              <td>{user.email}</td>
              <td>{user.role}</td>
              <td>
                <select
                  value={selectedRoles[user.userId]}
                  onChange={(e) => handleSelectChange(user.userId, e.target.value)}
                  className="form-select"
                >
                    <option value="">Выберите роль</option>
                    <option value="User">User</option>
                    <option value="Manager">Manager</option>
                    <option value="Admin">Admin</option>
                </select>
              </td>
              <td>
                <button
                  className="btn btn-primary"
                  onClick={() => handleChangeRole(user.userId)}
                >
                  Изменить
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default ChangeUserRolePage;