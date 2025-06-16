import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';



const ProductsPage = () => {
  const [products, setProducts] = useState([]);
  const [search, setSearch] = useState("");
  const [sortOrder, setSortOrder] = useState(""); // По умолчанию сортировка по возрастанию
  const [page, setPage] = useState(1);
  const [categories, setCategories] = useState([]);
  const [selectedCategory, setSelectedCategory] = useState("");
  const [hasMore, setHasMore] = useState(false);
  const [isLoading, setIsLoading] = useState(true);
  const [totalPages, setTotalPages] = useState(1);
  const navigate = useNavigate();
  

  /*const isAuthenticated = !!getCookie('ck');*/
  
  useEffect(() => {
    const token = document.cookie.split("; ").find((row) => row.startsWith("ck="));
    if (!token) {
      navigate("/auth");
    }
  }, [navigate]);

  useEffect(() => {
    const fetchCategories = async () => {
      try {
        const response = await axios.get("https://localhost:5260/api/Category", {
          withCredentials: true,
        });
        setCategories(response.data);
      } catch (error) {
        console.error("Ошибка при загрузке категорий:", error);
      }
    };
  
    fetchCategories();
  }, []);

  // Функция для загрузки данных с учетом сортировки и фильтрации
  const fetchProducts = async () => {
    try {
      const response = await axios.get(`https://localhost:5260/api/Product`, {
        params: {
          search,
          paginateSize: 6,
          page,
          sortOrder,
          categoryName: selectedCategory,
        },
        withCredentials: true,
      });
      
      const data = response.data;
      setProducts(data);
      setHasMore(data.length === 6);
      setTotalPages(prev => {
      if (data.length === 6) {
        return page + 1 > prev ? page + 1 : prev;
      }
      return page;
    });
    } catch (error) {
      console.error("Ошибка загрузки продуктов:", error);
      setProducts([]);
      setHasMore(false);
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    fetchProducts();
  }, [page, sortOrder, selectedCategory]); 

  const handleSearch = () => {
    setPage(1); 
    fetchProducts();
  };

/*
  return (
    <div className="container mt-4">
      <h1 className="mb-4">Продукты</h1>

      {/* Фильтр и сортировка *//*}
/*      <div className="mb-4 d-flex align-items-center">
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

        <select
          className="form-select me-2"
          style={{ maxWidth: '200px' }}
          value={selectedCategory}
          onChange={(e) => setSelectedCategory(e.target.value)}
        >
          <option value="">Все категории</option>
          {categories.map((category) => (
            <option key={category.categoryId} value={category.categoryName}>
              {category.categoryName}
            </option>
          ))}
        </select>

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

      {/* Список продуктов *//*}
      {products.length > 0 ? (
        <div className="row">
          {products.map((product) => (
            <div key={product.productId} className="col-md-4 mb-4"
            onClick={() => 
              { navigate(`/product/${product.productId}`);}}
              style={{ cursor: 'pointer' }}>
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

      {/* Пагинация *//*}
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

export default ProductsPage;*/
return (
    <div className="container mx-auto px-4 py-6 max-w-6xl">
      <h1 className="text-2xl font-semibold text-gray-800 mb-6 border-b pb-2">Каталог товаров</h1>

      {/* Фильтры */}
      <div className="bg-white rounded-lg border border-gray-200 p-4 mb-6 shadow-sm">
        <div className="flex flex-col md:flex-row gap-4 items-start md:items-end">
          <div className="flex-1 w-full">
            <label className="block text-sm font-medium text-gray-700 mb-1">Поиск</label>
            <div className="flex">
              <input
                type="text"
                className="flex-grow px-3 py-2 text-sm border border-r-0 border-gray-300 rounded-l-md focus:ring-1 focus:ring-blue-500 focus:border-blue-500 outline-none"
                placeholder="Введите название..."
                value={search}
                onChange={(e) => setSearch(e.target.value)}
                onKeyPress={(e) => e.key === 'Enter' && handleSearch()}
              />
              <button 
                className="bg-blue-100 text-blue-800 px-4 py-2 text-sm rounded-r-md hover:bg-blue-200 transition-colors flex items-center border border-blue-200 font-medium"
                onClick={handleSearch}
              >
                Найти
              </button>
            </div>
          </div>

          <div className="w-full md:w-auto">
            <label className="block text-sm font-medium text-gray-700 mb-1">Категория</label>
            <select
              className="w-full text-sm border border-gray-300 rounded-md px-3 py-2 focus:ring-1 focus:ring-blue-500 focus:border-blue-500 outline-none"
              value={selectedCategory}
              onChange={(e) => setSelectedCategory(e.target.value)}
            >
              <option value="">Все категории</option>
              {categories.map((category) => (
                <option key={category.categoryId} value={category.categoryName}>
                  {category.categoryName}
                </option>
              ))}
            </select>
          </div>

          <div className="w-full md:w-auto">
            <label className="block text-sm font-medium text-gray-700 mb-1">Сортировка</label>
            <div className="flex space-x-2">
              <button
                className={`px-3 py-2 text-sm rounded-md border ${sortOrder === "" ? "bg-blue-50 border-blue-200 text-blue-800" : "border-gray-300 hover:bg-gray-50"}`}
                onClick={() => setSortOrder("")}
              >
                Дешевле
              </button>
              <button
                className={`px-3 py-2 text-sm rounded-md border ${sortOrder === "desc" ? "bg-blue-50 border-blue-200 text-blue-800" : "border-gray-300 hover:bg-gray-50"}`}
                onClick={() => setSortOrder("desc")}
              >
                Дороже
              </button>
            </div>
          </div>
        </div>
      </div>

      {/* Загрузка */}
      {isLoading ? (
        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-5">
          {[...Array(6)].map((_, i) => (
            <div key={i} className="bg-gray-100 rounded-lg h-80 animate-pulse"></div>
          ))}
        </div>
      ) : products.length > 0 ? (
        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-5">
          {products.map((product) => (
            <div 
              key={product.productId} 
              className="bg-white rounded-lg border border-gray-200 overflow-hidden hover:shadow-sm transition-shadow cursor-pointer flex flex-col h-full"
              onClick={() => navigate(`/product/${product.productId}`)}
            >
              <div className="relative h-48 bg-gray-50 flex items-center justify-center p-4 border-b">
                <img
                  src={`https://localhost:5260/images/${product.imagePath}`}
                  className="max-h-full max-w-full object-contain"
                  alt={product.name}
                  style={{ maxHeight: '160px' }}
                  onError={(e) => {
                    e.target.onerror = null; 
                    e.target.src = 'https://via.placeholder.com/300?text=No+Image';
                  }}
                />
                {product.stock <= 0 && (
                  <div className="absolute inset-0 bg-black bg-opacity-40 flex items-center justify-center">
                    <span className="text-white text-sm font-medium bg-red-500 px-2 py-1 rounded">
                      Нет в наличии
                    </span>
                  </div>
                )}
              </div>
              <div className="p-4 flex flex-col flex-grow">
                <h3 className="font-medium text-gray-800 mb-2 line-clamp-2">{product.name}</h3>
                <p className="text-gray-600 text-sm mb-3 line-clamp-2 flex-grow">{product.description}</p>
                <div className="flex justify-between items-center mt-auto">
                  <span className="font-bold text-lg text-gray-900">{product.price} ₽</span>
                  <span className={`text-xs px-2 py-1 rounded-full ${product.stock > 0 ? 'bg-green-100 text-green-800' : 'bg-gray-100 text-gray-800'}`}>
                    {product.stock > 0 ? `${product.stock} шт.` : 'Под заказ'}
                  </span>
                </div>
              </div>
            </div>
          ))}
        </div>
      ) : (
        <div className="bg-white rounded-lg border border-gray-200 p-8 text-center">
          <div className="text-gray-300 mb-4">
            <svg xmlns="http://www.w3.org/2000/svg" className="h-12 w-12 mx-auto" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1} d="M9.172 16.172a4 4 0 015.656 0M9 10h.01M15 10h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z" />
            </svg>
          </div>
          <h3 className="text-lg font-medium text-gray-700 mb-1">Товары не найдены</h3>
          <p className="text-gray-500 text-sm">Попробуйте изменить параметры поиска</p>
        </div>
      )}
      {!isLoading && products.length > 0 && (
  <div className="mt-8 flex justify-center">
    <div className="flex items-center space-x-2">
      {page > 1 && (
        <button onClick={() => setPage(1)} className="px-3 py-1 text-sm border rounded-md">
          В начало
        </button>
      )}
      {page > 1 && (
        <button onClick={() => setPage(p => p - 1)} className="px-3 py-1 text-sm border rounded-md">
          Назад
        </button>
      )}
      <span className="px-3 py-1 text-sm bg-blue-100 rounded-md">
        {page} / {totalPages}
      </span>
      {hasMore && (
        <button onClick={() => setPage(p => p + 1)} className="px-3 py-1 text-sm border rounded-md">
          Вперед
        </button>
      )}
    </div>
  </div>
  )}
    </div>
  );
};

export default ProductsPage;