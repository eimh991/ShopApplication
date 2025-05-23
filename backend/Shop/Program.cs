using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Shop.Extensions;
using Shop.Infrastructure;
using Shop.Service.PaymentService;
using System.Globalization;



var builder = WebApplication.CreateBuilder(args);

var cultureInfo = new CultureInfo("en-US");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

// Add services to the container.
/*
builder.Services.AddAuthentication("CustomAuth")
    .AddCookie("CustomAuth", options =>
    {
        options.LoginPath = "/login"; // ���� ��� ��������������� ��� ���������������� �������
    });
*/

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddDbContext<AppDbContext>();
//builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));
//builder.Services.AddScoped<IJwtProvider,JwtProvider>();
builder.Services.AddHttpClient<QiwiPaymentService>();
builder.Services.AddHttpClient<TinkoffPaymentService>();
//builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
//builder.Services.AddScoped<IUserService, UserService>();
//builder.Services.AddScoped<IProductService, ProductService>();
//builder.Services.AddScoped<ICategoryService, CategoryService>();
//builder.Services.AddScoped<ICartItemService, CartItemSercive>();
//builder.Services.AddScoped<IOrderService, OrderService>();
//builder.Services.AddScoped<IRepository<User>, UserRepository>();
//builder.Services.AddScoped<IRepository<Product>, ProductRepository>();
//builder.Services.AddScoped<IRepository<Category>, CategoryRepository>();
//builder.Services.AddScoped<IRepositoryWithUser<Model.Order>, OrderRepository>();
//builder.Services.AddScoped<IRepositoryWithUser<CartItem>, CartItemRepository>();

//builder.Services.AddScoped<IPaymentService>(sp => sp.GetRequiredService<QiwiPaymentService>());
//builder.Services.AddScoped<IPaymentService>(sp => sp.GetRequiredService<TinkoffPaymentService>());
builder.Services.AddInfrastucture(builder.Configuration);
var jwtOptions = builder.Configuration.GetSection("JwtOptions").Get<JwtOptions>();
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtOptions"));
builder.Services.AddApiAuthentication(Options.Create(jwtOptions));
/*
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("localhost:6379"));
builder.Services.AddScoped<IDatabase>(provider =>
{
    var multiplexer = provider.GetRequiredService<IConnectionMultiplexer>();
    return multiplexer.GetDatabase();
});
*/


builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "������� JWT � ������� Bearer <token>"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
          new OpenApiSecurityScheme {
            Reference = new OpenApiReference {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
            }
          },
            new string[] { }
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMyOrigin", policy =>
    {
        policy.WithOrigins("http://localhost:3000") 
               .AllowCredentials()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();


app.UseStaticFiles();
app.UseDefaultFiles();



app.UseAuthentication();
app.UseAuthorization();


app.UseCors("AllowMyOrigin");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();


app.MapFallbackToFile("index.html");



app.Run();



