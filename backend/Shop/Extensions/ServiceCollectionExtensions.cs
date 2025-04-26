using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using Shop.Service;
using Shop.Service.PaymentService;
using Shop.Repositories;
using Shop.Interfaces;
using Shop.Data;
using Shop.Infrastructure;
using Shop.Model;

namespace Shop.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastucture(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>();

            var redisConnection = configuration.GetValue<string>("Redis:ConnectionString") ?? "localhost:6379";
            services.AddSingleton<IConnectionMultiplexer>(sp =>
            ConnectionMultiplexer.Connect(redisConnection));
            services.AddScoped<IDatabase>(sp =>
            {
                var multeplexer = sp.GetRequiredService<IConnectionMultiplexer>();
                return multeplexer.GetDatabase();
            });

            services.AddScoped<IJwtProvider, JwtProvider>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICartItemService, CartItemSercive>();
            services.AddScoped<IOrderService, OrderService>();
            

            services.AddScoped<IRepository<User>, UserRepository>();
            services.AddScoped<IRepository<Product>, ProductRepository>();
            services.AddScoped<IRepository<Category>, CategoryRepository>();
            services.AddScoped<IRepositoryWithUser<CartItem>, CartItemRepository>();
            services.AddScoped<IRepositoryWithUser<Shop.Model.Order>, OrderRepository>();
            services.AddScoped<ICartItemCleaner, CartItemRepository>();
            services.AddScoped<IUserBalanceUpdater,UserRepository>();

            services.AddHttpClient<QiwiPaymentService>();
            services.AddHttpClient<TinkoffPaymentService>();

            services.AddScoped<IPaymentService>(sp => sp.GetRequiredService<QiwiPaymentService>());
            services.AddScoped<IPaymentService>(sp => sp.GetRequiredService<TinkoffPaymentService>());

            return services;
        }
    }
}
