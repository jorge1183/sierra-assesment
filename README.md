# Sierra assessment project

## Project description
This project implements a restful API that allows to perform create, retrieve and list operations over customer, products and orders resources. A simple react client application was also created in order to interact with the api.


## Launching the application using docker
### Requirements
  1. MS Sql Server installed
  2. Docker installed
  3. Entity Framework Core cli installed

### Steps
1. In order to create the database, run the following command using a SSMS or another tool connected to the MSSQL server target instance. 
>`CREATE DATABASE sierra_db;`

**Note:** Consider that (localdb) is not supported by containers, in such case try with a different sql server instance.
  
2. Open a cmd window
3. Clone repository
4. Cd to solution directory
5. Run migrations
>`dotnet ef database update --project sierra/sierra.csproj --connection "Server=172.22.144.1,1433;Database=sierra_db;User Id=sa;Password=dulcine@12;TrustServerCertificate=true;"`

6. (Optional) Run sql script located in "sierra/DbScripts/InsertDummyData.sql" in order to pre-populate Customer and Product tables
7. Build docker image with following command
>`docker build -f "./sierra/Dockerfile" -t sierra-orders .`

8. Run docker image container
>`docker run -e ASPNETCORE_URLS="http://+" -p 5019:80 -e ConnectionStrings__sierra_db="Server=172.22.144.1,1433;Database=sierra_db;User Id=sa;Password=dulcine@12;TrustServerCertificate=true;" --name SierraOrders sierra-orders`

9. After ensuring that the container is UP, open a broser window and go to [http://localhost:5019/](http://localhost:5019/).


## API definition

### **Session controller**
#### **Supported operations:**
- **Login**: Returns a JWT that is needed to perform POST operations on Customer, Product and Order controllers
  - Method: **POST**
  - Path: `/api/session/login?username=[username]&password=[password]`

__Note__: No user catalog was implemented so username and password is currently configured in appsettings (admin and password respectively).
  

### **Customer controller**

#### **Model Fields**
|   name  |  type    |  max length |
| ------- |:--------:|:-----------:|
| id      | int      |             |
| name    | string   | 20          |

#### **Supported operations:**
- **Create**
  - Method: **POST**
  - Path: `api/customer`
  - Requires bearer token

**Example Request:**

Type: **CustomerRequest**
```
{
    "name": "Customer 1"
}
```
**Example Response:**

Type: **CustomerResponse**
```
{
    "id": 1,
    "name": "Customer 1"
}
```

- **Update**
  - Method: **PUT**
  - Path: `api/customer/[customerId]`
  - Requires bearer token

**Example Request:**

Type: **CustomerRequest**
```
{
    "name": "Customer 1"
}
```
**Example Response:**

Type: **CustomerResponse**
```
{
    "id": 1,
    "name": "Customer 1"
}
```

- **Retrieve**
  - Method: **GET**
  - Path: `api/customer/[customerId]`
  
**Example request:** `api/customer/1`

**Example response:**

Type: **CustomerResponse**
```
{
    "id": 1,
    "name": "Jorge Morales"
}
```

- **List**
  - Method: **GET**
  - Path: `api/customer`

**Example request:** `api/customer`

**Example response:**

Type: Array of **CustomerResponse**
```
[
    {
        "id": 1,
        "name": "Customer 1"
    },{
        "id": 2,
        "name": "Customer 2"
    }
]
```

### **Product controller**

#### **Model Fields**
|   name  |  type    |  max length |
| ------- |:--------:|:-----------:|
| id      | int      |             |
| name    | string   | 20          |
| price   | decimal  |             |

#### **Supported operations:**
- **Create**
  - Method: **POST**
  - Path: `api/product`
  - Requires bearer token

**Example Request:**

Type: **ProductRequest**
```
{
    "name": "Product 1",
    "price": 56.5
}
```
**Example Response:**

Type: **ProductResponse**
```
{
    "id": 1,
    "name": "Product 1",
    "price": 56.5
}
```
    
- **Update**
  - Method: **PUT**
  - Path: `api/product/[productId]`
  - Requires bearer token

**Example Request:**

Type: **ProductRequest**
```
{
    "name": "Product 1",
    "price": 56.5
}
```
**Example Response:**

Type: **ProductResponse**
```
{
    "id": 1,
    "name": "Product 1",
    "price": 56.5
}
```
    
- **Retrieve**
  - Method: **GET**
  - Path: `api/product/[productId]`
  
**Example request:** `api/product/1`

**Example response:**

Type: **ProductResponse**
```
{
    "id": 1,
    "name": "Product 1",
    "price": 56.5
}
```

- **List**
  - Method: **GET**
  - Path: `api/product`

**Example request:** `api/product`

**Example response:**

Type: Array of **ProductResponse**
```
[
    {
        "id": 1,
        "name": "Product 1",
        "price": 56.5
    },{
        "id": 2,
        "name": "Product 2",
        "price": 25.99
    }
]
```
### **Order controller**

#### **Model Fields**
|   name         |  type    |
| -------------- |:--------:|
| id             | int      |
| customerId(FK) | int      |
| productId(FK)  | int      |
| price          | decimal  |
| quantity       | int      |
| total          | decimal  |

#### **Supported operations:**
- **Create**
  - Method: **POST**
  - Path: `api/order`
  - Requires bearer token

**Example Request:**

Type: **OrderRequest**
```
{
    "customerId": 1,
    "productId": 1,
    "quantity": 3
}
```
**Example Response:**

Type: **OrderResponse**
```
{
    "id": 1,
    "customer": {
        "id": 1,
        "name": "Customer 1"
    },
    "product": {
        "id": 1,
        "name": "Product 1",
        "price": 56.50
    },
    "price": 56.50,
    "quantity": 2,
    "total": 113
}
```
    
- **Retrieve**
  - Method: **GET**
  - Path: `api/order/[orderId]`
  
**Example request:** `api/order/1`

**Example response:**

Type: **OrderResponse**
```
{
    "id": 1,
    "customer": {
        "id": 1,
        "name": "Customer 1"
    },
    "product": {
        "id": 1,
        "name": "Product 1",
        "price": 56.50
    },
    "price": 56.50,
    "quantity": 2,
    "total": 113
}
```

- **List**
  - Method: **GET**
  - Path: `api/order`

**Example request:** `api/order`

**Example response:**

Type: Array of **OrderResponse**
```
[
    {
    "id": 1,
    "customer": {
        "id": 1,
        "name": "Customer 1"
    },
    "product": {
        "id": 1,
        "name": "Product 1",
        "price": 56.50
    },
    "price": 56.50,
    "quantity": 2,
    "total": 113
    }, {
        "id": 2,
        "customer": {
            "id": 1,
            "name": "Customer 1"
        },
        "product": {
            "id": 2,
            "name": "Product 2",
            "price": 25.99
        },
        "price": 25.99,
        "quantity": 3,
        "total": 77.97
    }
]
```
