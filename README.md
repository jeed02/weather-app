# Weather App

## Tech Stack

- **Frontend**: Vue 3, PrimeVue, Tailwind CSS, Axios  
- **Backend**: ASP.NET Core Web API  
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

## 📡 API Endpoints

- `GET /api/weather` - Get all weather records
- `POST /api/weather` - Add a new weather record
- `DELETE /api/weather/{id}` - Delete a single record
- `DELETE /api/weather/delete-multiple` - Delete multiple records


## Screenshots
![weather app main](https://github.com/user-attachments/assets/74865f67-4c10-40cf-bf9a-c5b6ec1a333e)

![weather app delete](https://github.com/user-attachments/assets/8025ec8b-6c14-44e1-b0aa-0888b2a28d45)
