import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';


/*const getCookie = (name) => {
    const matches = document.cookie.match(new RegExp(`(?:^|; )${name.replace(/([.$?*|{}()\[\]\\\/\+^])/g, '\\$1')}=([^;]*)`));
    return matches ? decodeURIComponent(matches[1]) : undefined;
  };*/

const ProductsPage = () => {
  const [products, setProducts] = useState([]);
  const [search, setSearch] = useState("");
  const [sortOrder, setSortOrder] = useState(""); // По умолчанию сортировка по возрастанию
  const [page, setPage] = useState(1);
  const [hasMore, setHasMore] = useState(false);
  const navigate = useNavigate();

  /*const isAuthenticated = !!getCookie('ck');*/
  
  useEffect(() => {
    const token = document.cookie.split("; ").find((row) => row.startsWith("ck="));
    if (!token) {
      navigate("/auth");
    }
  }, [navigate]);

  // Функция для загрузки данных с учетом сортировки и фильтрации
  const fetchProducts = async () => {
    try {
      const response = await axios.get(`https://localhost:5260/api/Product`, {
        params: {
          search,
          paginateSize: 9,
          page,
          sortOrder,
        },withCredentials: true,
      });
      
      const data = response.data;
      setProducts(data);

      setHasMore(data.length ===9);
    } catch (error) {
      console.error("Ошибка загрузки продуктов:", error);
      setProducts([]);
      setHasMore(false);
    }
  };

  useEffect(() => {
    fetchProducts();
  }, [page, sortOrder]); // Запрос при изменении страницы или сортировки

  const handleSearch = () => {
    setPage(1); // Сбрасываем на первую страницу при новом поиске
    fetchProducts();
  };
/*
  if (!isAuthenticated) {
    return <Navigate to="/auth" />;
  }

  if (loading) {
    return <p>Загрузка...</p>;
  }
  console.log(products.length);
  if ((products.length === 0)) {
    return <p>Продукты не найдены.</p>;
  }
*/

  return (
    <div className="container mt-4">
      <h1 className="mb-4">Продукты</h1>

      {/* Фильтр и сортировка */}
      <div className="mb-4 d-flex align-items-center">
        <input
          type="text"
          className="form-control me-2"
          placeholder="Поиск по названию"
          value={search}
          onChange={(e) => setSearch(e.target.value)}
        />
        <button className="btn btn-primary me-2" onClick={handleSearch}>
          Искать
        </button>
        <div>
          <button
            className={`btn ${sortOrder === "" ? "btn-secondary" : "btn-outline-secondary"} me-1`}
            onClick={() => setSortOrder("")}
          >
            Сначала дешевые
          </button>
          <button
            className={`btn ${sortOrder === "desc" ? "btn-secondary" : "btn-outline-secondary"}`}
            onClick={() => setSortOrder("desc")}
          >
            Сначала дорогие
          </button>
        </div>
      </div>

      {/* Список продуктов */}
      {products.length > 0 ? (
        <div className="row">
          {products.map((product) => (
            <div key={product.productId} className="col-md-4 mb-4">
              <div className="card">
                <img
                  src={`https://localhost:5260/images/${product.imagePath}`}
                  className="card-img-top"
                  alt={product.name}
                  style={{ height: "200px", objectFit: "cover" }}
                />
                <div className="card-body">
                  <h5 className="card-title">{product.name}</h5>
                  <p className="card-text">{product.description}</p>
                  <p className="card-text">Цена: {product.price} ₽</p>
                  <p className="card-text">В наличии: {product.stock} шт.</p>
                </div>
              </div>
            </div>
          ))}
        </div>
      ) : (
        <p>Продукты не найдены.</p>
      )}

      {/* Пагинация */}
      {products.length > 0 && (
        <nav className="d-flex justify-content-center mt-4">
          <ul className="pagination">
          {page > 1 && (
              <li className="page-item">
                <button
                  className="page-link"
                  onClick={() => setPage((prevPage) => prevPage - 1)}
                >
                  Предыдущая
                </button>
              </li>
            )}
            {hasMore && (
              <li className="page-item">
                <button
                  className="page-link"
                  onClick={() => setPage((prevPage) => prevPage + 1)}
                >
                  Следующая
                </button>
              </li>
            )}
          </ul>
        </nav>
      )}
    </div>
  );
};

export default ProductsPage;