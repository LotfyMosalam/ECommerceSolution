# FullStackShop Backend

This is the backend for the **Full Stack E-commerce Assignment**, built with **.NET Core Web API** following **Clean Architecture** principles.

## 🏗️ Project Structure

```

FullStackShop.Backend/
├─ Core/           # Domain entities & interfaces
├─ Application/    # Business logic, DTOs, use cases
├─ Infrastructure/ # EF Core, repositories, DB context, services
└─ Api/            # Controllers, middleware, startup

````

- **Core** → Contains entities (`User`, `Product`) and contracts.
- **Application** → Application services, DTOs, and business rules.
- **Infrastructure** → EF Core DbContext, repositories, JWT service, file storage for product images.
- **Api** → ASP.NET Core Web API project, controllers, DI configuration, middlewares.

## 🚀 Features

- User & Product entities implemented.
- **Authentication & Authorization** with **JWT** and **Refresh Tokens**.
- Product images stored in **local storage**.
- SQL Server database support.
- API secured with JWT authentication.
- Database script included: `Database/FullStackShop.sql`.

## 🔑 Endpoints Overview

- **Auth**
  - `POST /api/auth/login`
  - `POST /api/auth/refresh`
  - `POST /api/auth/register`

- **Products**
  - `GET /api/products`
  - `GET /api/products/{id}`
  - `POST /api/products`
  - `PUT /api/products/{id}`
  - `DELETE /api/products/{id}`

## 🛠️ Setup Instructions

1. Restore the database:
   - Open SQL Server Management Studio (SSMS).
   - Run the script `Database/FullStackShop.sql`.

2. Update the **connection string** in `Api/appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=.;Database=ECommerceDb;Trusted_Connection=True;MultipleActiveResultSets=true"
   }
````

3. Run the API:

   ```bash
   dotnet run --project Api
   ```

   Default URL: `http://localhost:5050`

4. Test with Swagger UI:

   * Navigate to: `http://localhost:5050/swagger`

## 👤 Test User Credentials

* **Username**: `testuser`
* **Password**: `P@ssw0rd123`

## ✅ Notes

* Product images are stored under `wwwroot/images`.
* JWT expires after a short time; refresh token endpoint available.

````

---

# 📌 Frontend — `README.md`

```markdown
# FullStackShop Frontend

This is the frontend for the **Full Stack E-commerce Assignment**, built with **Angular**.

## 🏗️ Project Structure

````

src/app/
├─ core/        # services, models, guards, interceptors
├─ features/
│    ├─ auth/       # login component
│    └─ products/   # product list component
├─ shared/      # shared UI components
└─ app.routes.ts

````

- **Auth** → Login form, token storage, API integration.
- **Products** → Product list display, fetching from backend.
- **Core** → Reusable services (auth service, product service).
- **Shared** → Common UI components.

## 🚀 Features

- **Login** with JWT authentication.
- **Product list** display.
- Angular **signals & RxJS** for state management.
- **HTTP Interceptor** for attaching JWT tokens.
- Clean, modular structure.

## 🛠️ Setup Instructions

1. Install dependencies:
   ```bash
   npm install
````

2. Update backend API URL in `environment.ts`:

   ```ts
   export const environment = {
     apiUrl: 'http://localhost:5000/api'
   };
   ```

3. Run the Angular app:

   ```bash
   ng serve
   ```

4. Navigate to:

   ```
   http://localhost:4200
   ```

## 👤 Test User Credentials

* **Username**: `testuser`
* **Password**: `P@ssw0rd123`

## ✅ Notes

* The login page requires a valid backend running.
* Product images are fetched from the backend (`http://localhost:5000/images/...`).
