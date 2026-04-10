# DeviceManagementSystem

Device Management System is a full-stack application for managing users, devices, and user-device assignments.

## Project Structure

- `DeviceManagementSystem/`: ASP.NET Core Web API (`net10.0`), authentication, controllers
- `DeviceManagementSystem.Application/`: application services, commands, DTO mappers
- `DeviceManagementSystem.Domain/`: core entities and domain models
- `DeviceManagementSystem.Infrastructure/`: SQL Server repositories and data access
- `DeviceManagementSystem.AngularClient/`: Angular 21 frontend
- `scripts/`: SQL scripts used by Docker DB initialization and seed
- `docker-compose.yml`: full local stack (frontend, backend, database, init scripts, reverse proxy)
- `nginx.conf`: reverse proxy for `/` (frontend) and `/api` (backend)

## Prerequisites

Choose one of the run modes below.

### Docker mode (recommended)

- Docker Desktop with Docker Compose

### Manual mode (without Docker)

- .NET SDK 10
- Node.js 22+ and npm
- SQL Server (Express/Developer) + SSMS or sqlcmd

## Run Locally With Docker (Recommended)

```env
MSSQL_SA_PASSWORD=YourStrong!Passw0rd
ASPNETCORE_JWT__KEY=THIS_IS_A_DEV_ONLY_SECRET_KEY_CHANGE_IN_PRODUCTION_2026
GEMINI_API_KEY=your_gemini_key_here
```

2. Build and start everything:

```powershell
docker compose up --build
```

3. Open the app:

- `http://localhost:8081`

4. Stop services (also removed DB volume):

```powershell
docker compose down -v
```

## Run Locally Without Docker

### 1) Initialize SQL Server database

Run these scripts in order against your SQL Server instance:
1. `init_db.sql` (schema)
2. `add_mock_data.sql` (seed data)

PowerShell example:

```powershell
$env:Database__ConnectionString="Data Source=YOUR_SERVER;Initial Catalog=DeviceManagementDB;Integrated Security=True;TrustServerCertificate=True;"
$env:Jwt__Key="THIS_IS_A_DEV_ONLY_SECRET_KEY_CHANGE_IN_PRODUCTION_2026"
$env:JWT_SIGNING_KEY="THIS_IS_A_DEV_ONLY_SECRET_KEY_CHANGE_IN_PRODUCTION_2026"
$env:GEMINI_API_KEY="your_gemini_key_here"
```

### 3) Run backend API

From repository root:

```powershell
dotnet run --project DeviceManagementSystem/DeviceManagementSystem.csproj --launch-profile https
```

Backend (and Swagger in Development) will be available at:

- `https://localhost:7250`
- `https://localhost:7250/swagger`



### 4) Run frontend

```powershell
cd DeviceManagementSystem.AngularClient
npm install
npm start
```

### 5) Run nginx reverse proxy

Nginx is required to proxy `/api` requests to the backend and serve the frontend as in production. You must have nginx installed locally.

**Cross-platform instructions:**

- [Download nginx](https://nginx.org/en/download.html) for your OS (Windows, Linux, macOS).
- Copy the provided `nginx-local.conf` from the repo root to your nginx `conf` directory, or run nginx with the config directly:

	- **Windows example:**
		```powershell
		# From repo root, if nginx is in PATH
		nginx -c $(Get-Location)\nginx.conf
		```
	- **Linux/macOS example:**
		```sh
		nginx -c $(pwd)/nginx.conf
		```

Nginx will listen on `http://localhost:8081` and proxy requests to the frontend and backend.

---

Open:

- `http://localhost:8081`

## Seeded Credentials (from SQL seed)

- Admin user:
	- Email: `admin@email.com`
	- Password: `admin`

## Notes

- `GEMINI_API_KEY` is required only for AI description generation endpoints.
- Frontend uses relative API paths (`/api/...`). All API proxying is handled by nginx (in both Docker and manual modes); no Angular proxy config is needed.
- When running locally without Docker, nginx must be running for the app to work at `http://localhost:8081`.
