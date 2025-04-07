# Weather App

## Tech Stack

- **Frontend**: Vue 3, PrimeVue, Tailwind CSS, Axios  
- **Backend**: ASP.NET Core Web API, Hangfire (Worker grabs weather data hourly and stores in PostgreSQL database)
- **Database**: Entity Framework Core with SQL Server (or any EF-supported DB)

## Setup Instructions

### Frontend

```bash
cd frontend
npm install
npm run dev
```

### Backend (.NET API)

```bash
cd backend
dotnet restore
dotnet run
```

Ensure your `appsettings.json` is configured with your database connection string.

## API Endpoints

- `GET /api/weather` - Get all weather records
- `POST /api/weather` - Add a new weather record
- `DELETE /api/weather/{id}` - Delete a single record
- `DELETE /api/weather/delete-multiple` - Delete multiple records


## Screenshots
![weather app main](https://github.com/user-attachments/assets/74865f67-4c10-40cf-bf9a-c5b6ec1a333e)

![weather app delete](https://github.com/user-attachments/assets/b32847df-4ed6-4a0a-890e-67e6b8d85969)

