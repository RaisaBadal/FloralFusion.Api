**FloralFusion.Api** <br>
FloralFusion.Api is an ASP.NET Core Web API designed to manage a flower shop's inventory, orders, and customer interactions.<br> This API provides endpoints for handling various operations related to flowers, 
orders, and customers.<br>

# Table of Contents
- Features
- Installation
- Usage
- Endpoints
-Contributing
-License
- Features
- Manage flower inventory (CRUD operations)
- Handle customer orders
- Customer management
- Real-time notifications for order status
- Secure authentication and authorization
- Installation
-Prerequisites
- .NET 8 SDK
- SQL Server
- Steps
- Clone the repository:

```sh
Copy code
git clone https://github.com/RaisaBadal/FloralFusion.Api.git
```
```sh
cd FloralFusion.Api
```

# Set up the database:

- Update the appsettings.json file with your SQL Server connection string.
- Run the following command to apply migrations:

```sh
dotnet ef database update
```

#Run the application:

```sh
dotnet run
```

# Usage
- Running the API

To start the API, run the following command:
```sh
dotnet run
```
The API will be available at https://localhost:5001 (or http://localhost:5000).

# Swagger UI
- To explore and test the API endpoints, navigate to https://localhost:5001/swagger in your browser.

# Endpoints
- Flowers
- GET /api/flowers - Get all flowers
- GET /api/flowers/{id} - Get a specific flower by ID
- POST /api/flowers - Create a new flower
- PUT /api/flowers/{id} - Update a flower
- DELETE /api/flowers/{id} - Delete a flower
- Orders
- GET /api/orders - Get all orders
- GET /api/orders/{id} - Get a specific order by ID
- POST /api/orders - Create a new order
- PUT /api/orders/{id} - Update an order
- DELETE /api/orders/{id} - Delete an order
- Customers
- GET /api/customers - Get all customers
- GET /api/customers/{id} - Get a specific customer by ID
- POST /api/customers - Create a new customer
- PUT /api/customers/{id} - Update a customer
- DELETE /api/customers/{id} - Delete a customer
# Contributing
Contributions are welcome! Please:
- fork this repository
- submit pull requests.
-  Ensure that your code adheres to the existing coding standards and includes appropriate tests.

# Fork the repository
- Create a new branch (git checkout -b feature/your-feature)
- Commit your changes (git commit -m 'Add some feature')
- Push to the branch (git push origin feature/your-feature)
- Open a pull request
# License
- This project is licensed under the MIT License. See the LICENSE file for details.

Feel free to modify this template to better suit the specific details and needs of your project.
