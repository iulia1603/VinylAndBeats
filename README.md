# VinylAndBeats

VinylAndBeats is a web application, a music marketplace where users can browse, publish, sell and review music-related products such as vinyl records, CDs, instruments and accessories.

The project uses a hybrid frontend approach:
- ASP.NET Core MVC + Razor Views for most pages
- Angular for the product reviews page, which consumes the REST API

---

## Tech Stack

- **Backend:** ASP.NET Core MVC, ASP.NET Core Web API
- **Database:** PostgreSQL + Entity Framework Core
- **Authentication:** ASP.NET Core Identity with Admin and User roles
- **API Auth:** JWT Bearer authentication for protected API endpoints
- **Frontend:** Razor Views + Bootstrap + Angular component for reviews
- **Architecture:** Repository Pattern + Service Layer + DTOs + ViewModels + Dependency Injection
- **Logging:** Serilog + custom request logging middleware
- **API Documentation:** Swagger / OpenAPI

---

## Features

- User registration and login
- Role-based authorization with Admin and User
- Product marketplace with create, edit, delete and details pages
- Product categories and tags
- Shopping cart 
- Order placement and stock update
- Product reviews
- Angular reviews page consuming the REST API
- Admin-only API endpoints for creating categories and tags
- Global exception handling middleware
- Client-side and server-side form validation
- Swagger UI for testing API endpoints

---

## Getting Started

### Prerequisites

Make sure you have installed:

- .NET SDK
- PostgreSQL
- Node.js and npm
- Angular CLI, optional if you want to run the Angular project separately

---

## Run Locally

### 1. Clone the repository

```bash
git clone https://github.com/YOUR_USERNAME/VinylAndBeats.git
cd VinylAndBeats
```

Replace `YOUR_USERNAME` with your GitHub username.

---

### 2. Configure the database connection

Open:

```txt
VinylAndBeats/appsettings.json
```

Set your PostgreSQL connection string:

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=vinylandbeats;Username=postgres;Password=your_password"
}
```

---

### 3. Apply migrations

From the backend project folder:

```bash
cd VinylAndBeats
dotnet ef database update
```

The application also uses `SeedData.cs` to create initial data such as roles, an admin account, categories, tags and example products.

---

### 4. Run the backend

```bash
dotnet restore
dotnet build
dotnet run
```

The application will run locally, for example at:

```txt
https://localhost:7161
```

Swagger is available at:

```txt
https://localhost:7161/swagger
```

---

## Run Angular Separately

The Angular project is located in:

```txt
vinylandbeats-app/
```

To run it separately:

```bash
cd vinylandbeats-app
npm install
npm start
```

or:

```bash
ng serve
```

The Angular development server usually runs at:

```txt
http://localhost:4200
```

The built Angular reviews page is also served by the ASP.NET Core backend from:

```txt
VinylAndBeats/wwwroot/spa/
```

The Angular page is opened from a product details page using:

```txt
/spa/index.html?productId={id}
```

---


## Screenshots


### Home Page

![Home Page](screenshots/HomePage.png)

### Products Page

![Products Page](screenshots/ProductsPage.png)

### Product Details

![Product Details](screenshots/ProductDetails.png)

### Authentification

![Authentification](screenshots/Auth.png)

### Register

![Register](screenshots/Register.png)

### Create Product

![Create Product](screenshots/CreateProduct.png)

### My Products

![My Products](screenshots/MyProducts.png)

### Cart

![Cart](screenshots/Cart.png)

### Orders

![Orders](screenshots/Orders.png)

### Create Review

![Create Review](screenshots/CreateReview.png)

### Angular Reviews Page

![Angular Reviews Page](screenshots/AngularReviewsPage.png)

### Swagger UI

![Swagger UI](screenshots/SwaggerUI.png)
