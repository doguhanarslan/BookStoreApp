# BookStoreApp

A bookstore application built on **.NET 8** with a layered architecture, an ASP.NET Core
Web API, an MVC UI and a React client — plus Redis caching and Elasticsearch-backed search.

## Architecture

```
BookStoreApp.Core         reusable infrastructure — generic repository, caching, aspects
BookStoreApp.Entities     domain entities
BookStoreApp.DataAccess   EF Core context, repositories, migrations
BookStoreApp.Business     services (managers), DTOs, validators
BookStoreApp.WebAPI       REST API — 7 controllers
BookStoreApp.MvcWebUI     server-rendered MVC interface
BookStoreApp.FrontEnd     React client (axios, react-router-dom)
```

Dependencies point inward: `Business` depends on `DataAccess` abstractions
(`IBookDal`, `ICartDal`, …) rather than on EF Core types, so the persistence layer can be
swapped without touching business logic. Concrete implementations are wired in
`Program.cs` through the built-in DI container.

## What's in it

**Generic repository.** `IEntityRepository<T>` and `EfEntityRepositoryBase<TEntity, TContext>`
give every entity CRUD without a hand-written repository per type; entity-specific
interfaces add only the queries that are actually specific.

**Caching as an aspect.** Redis sits behind `ICacheService` (`RedisCacheManager`,
registered as a singleton). A PostSharp `CacheAspect` applies caching declaratively —
the method being cached does not contain cache code, so the caching policy is visible
in one place instead of being scattered through service methods.

**Elasticsearch** powers book search through `IElasticsearchService` / `ElasticSearchManager`,
kept behind an interface so the API layer never touches the Elastic client directly.

**Authentication.** JWT with **refresh tokens** (added in its own migration), plus users,
roles and a `UserRole` join — role-based authorisation rather than a single admin flag.

**Validation.** FluentValidation validators (`ReviewValidator`) registered in DI and applied
before entities reach the business layer.

## Domain

`Book` · `Author` · `BookAuthor` · `Category` · `BookReview` · `Cart` · `CartItem` ·
`User` · `Role` · `UserRole` — including many-to-many relationships (book↔author,
user↔role) mapped through join entities.

## Stack

`.NET 8` `C#` `ASP.NET Core Web API` `ASP.NET Core MVC` `EF Core` `PostgreSQL` `Redis`
`Elasticsearch` `PostSharp` `FluentValidation` `JWT` `React`

## Running it

Requires PostgreSQL, Redis and Elasticsearch. Set `ConnectionStrings` in
`BookStoreApp.WebAPI/appsettings.json`, then:

```
dotnet ef database update --project BookStoreApp.DataAccess
dotnet run --project BookStoreApp.WebAPI
```

The React client lives in `BookStoreApp.FrontEnd` (`npm install && npm start`).
