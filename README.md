<h1 align="left">🍳 Recipe Favorites Backend API</h1>
<p align="left"> <b>Minimal .NET 8 backend for managing favorite recipes with secure JWT authentication, dynamic recipe details from TheMealDB, and MSSQL storage.</b> </p>
<h2 align="left">✨ Features</h2>
<ul align="left"> <li>🔒 <b>JWT Authentication</b> for secure user login & registration</li> <li>❤️ <b>Favorite recipe management</b> (add, remove, list)</li> <li>🥘 <b>Dynamic recipe details</b> fetched from TheMealDB API</li> <li>🚀 <b>Minimal API design</b> using .NET 8</li> <li>🗄️ <b>MSSQL database storage</b></li> </ul>
<h2 align="left">📁 Project Structure</h2>

```
backend/
│
├── AutoMapper/
│   └── MapperProfile.cs
├── Data/
│   └── AppDbContext.cs
├── Models/
│   ├── DTOs/
│   ├── DTOs/Recipe/
│   └── Entities/
├── Repositories/
│   ├── IAccountRepo.cs
│   ├── AccountRepo.cs
│   ├── IFavoriteRepo.cs
│   └── FavoriteRepo.cs
├── Services/
│   ├── IAccountService.cs
│   ├── AccountService.cs
│   ├── IFavoriteService.cs
│   └── FavoriteService.cs
├── Program.cs
├── appsettings.json
└── appsettings.Development.json
```


<h2 align="left">⚡ Setup Instructions</h2>
<ol align="left"> <li> <b>Prerequisites:</b> <ul> <li>.NET 8 SDK</li> <li>SQL Server (Express / Full) 🖥️</li> <li>Postman for API testing</li> <li>Optional: Visual Studio 2022 / VS Code</li> </ul> </li> <li> <b>Clone the Repository:</b> <pre> git clone &lt;repo_url&gt; cd backend </pre> </li> <li> <b>Configure Database Connection:</b> <pre> "ConnectionStrings": { "Default": "Data Source=YOUR_SERVER_NAME;Initial Catalog=RecipeDB;Integrated Security=True;" } </pre> Replace <code>YOUR_SERVER_NAME</code> with your SQL Server instance. </li> <li> <b>Install Dependencies:</b> <pre> dotnet restore </pre> </li> <li> <b>Apply Migrations and Create Database:</b> <pre> dotnet ef migrations add InitialCreate dotnet ef database update </pre> </li> <li> <b>Run the Application:</b> <pre> dotnet run </pre> API runs at: <code>https://localhost:7288</code><br> Swagger UI: <code>https://localhost:7288/swagger</code> </li> </ol>
<h2 align="left">🔗 API Endpoints</h2>
<table> <tr> <th>Method</th> <th>Endpoint</th> <th>Description</th> </tr> <tr> <td>POST</td> <td>/register</td> <td>Register a new user</td> </tr> <tr> <td>POST</td> <td>/login</td> <td>Login & get JWT</td> </tr> <tr> <td>GET</td> <td>/favorites</td> <td>List all favorites for the user</td> </tr> <tr> <td>POST</td> <td>/favorites</td> <td>Add a recipe to favorites</td> </tr> <tr> <td>DELETE</td> <td>/favorites/{mealId}</td> <td>Remove a favorite recipe by MealId</td> </tr> </table> <p align="left"><b>Note:</b> All <code>/favorites</code> routes require JWT authentication 🔐</p>
<h2 align="left">🔑 Authorization</h2>
<p align="left"> Include JWT in request header:<br> <code>Authorization: Bearer &lt;JWT_TOKEN&gt;</code> </p>
<h2 align="left">⚙️ Environment Variables</h2>
<pre> "Jwt": { "Audience": "https://localhost:7288", "Issuer": "https://localhost:7288", "Key": "YOUR_SECRET_KEY_HERE" } </pre> <ul> <li><b>Audience & Issuer:</b> should match API URL</li> <li><b>Key:</b> strong secret string for signing JWT tokens 🔐</li> </ul>
