using BookStoreApp.Business.Abstract;
using BookStoreApp.Business.Concrete.Managers;
using BookStoreApp.Core.CrossCuttingConcerns.Caching;
using BookStoreApp.Core.CrossCuttingConcerns.Caching.Redis;
using BookStoreApp.DataAccess.Abstract;
using BookStoreApp.DataAccess.Concrete.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<IBookDal, EfBookDal>();
builder.Services.AddScoped<IBookService, BookManager>();
builder.Services.AddScoped<ICartDal, EfCartDal>();
builder.Services.AddScoped<ICartService, CartManager>();
builder.Services.AddScoped<IUserDal, EfUserDal>();
builder.Services.AddScoped<IUserService, UserManager>();
builder.Services.AddScoped<DbContext, BookstoreContext>();
builder.Services.AddSingleton<ICacheService, RedisCacheManager>();
builder.Services.AddScoped<IReviewDal,EfReviewDal>();
builder.Services.AddScoped<IReviewService, ReviewManager>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder.WithOrigins("http://localhost:56852", "http://localhost:56853")  // Frontend URLs
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); // Allow cookies and authentication information
    });
});
builder.Services.AddControllers();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddDbContext<BookstoreContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Redis
var redisConnectionString = builder.Configuration.GetConnectionString("RedisConnection") ?? "localhost";
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnectionString));

builder.Services.AddHttpClient();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AllowAllOrigins"); // Enable CORS policy

app.UseAuthorization();

app.MapControllers();

app.Run();
