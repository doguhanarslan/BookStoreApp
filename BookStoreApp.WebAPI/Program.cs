using BookStoreApp.Business.Abstract;
using BookStoreApp.Business.Concrete.Managers;
using BookStoreApp.Business.ValidationRules.FluentValidation;
using BookStoreApp.Core.CrossCuttingConcerns.Caching.Redis;
using BookStoreApp.Core.CrossCuttingConcerns.Caching;
using BookStoreApp.DataAccess.Abstract;
using BookStoreApp.DataAccess.Concrete.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using Microsoft.Extensions.Options;
using BookStoreApp.Core.Utilities.Config;
using BookStoreApp.Entities.Concrete;
using FluentValidation;
using Microsoft.AspNetCore.Http.Features;

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
builder.Services.AddScoped<IReviewDal, EfReviewDal>();
builder.Services.AddScoped<IReviewService, ReviewManager>();
builder.Services.AddScoped<ICategoryDal, EfCategoryDal>();
builder.Services.AddScoped<ICategoryService, CategoryManager>();
builder.Services.AddScoped<IElasticsearchService, ElasticSearchManager>();
builder.Services.AddScoped<IValidator<BookReview>, ReviewValidator>();

// Configure Elasticsearch
var elasticsearchConfig = builder.Configuration.GetSection("Elasticsearch").Get<ElasticsearchConfig>();
var settings = new ElasticsearchClientSettings(new Uri("https://localhost:9200"))
    .DefaultIndex("books")
    .CertificateFingerprint("8dcfee6fbf4e990f6b5026ea0d9dcdca4fe77920a53afc7d03b6ac335e143381")
    .Authentication(new BasicAuthentication("elastic", "dqNSOZvmnR6ddi7FCwB3"));

var client = new ElasticsearchClient(settings);
builder.Services.AddSingleton(client);
builder.Services.AddSingleton<IElasticsearchService, ElasticSearchManager>();

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
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 10485760;
});
// Configure cookie policy
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.None;
    options.Secure = CookieSecurePolicy.Always;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseCors("AllowAllOrigins"); // Enable CORS policy

app.UseCookiePolicy(); // Apply the cookie policy

app.UseAuthorization();

app.MapControllers();

app.Run();

