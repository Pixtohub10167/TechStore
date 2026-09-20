-- Схема БД TechStore (MS SQL Server)
CREATE TABLE Categories (
    Id               INT IDENTITY(1,1) PRIMARY KEY,
    Name             NVARCHAR(100) NOT NULL,
    ParentCategoryId INT NULL REFERENCES Categories(Id)
);

CREATE TABLE Suppliers (
    Id    INT IDENTITY(1,1) PRIMARY KEY,
    Name  NVARCHAR(150) NOT NULL,
    Inn   CHAR(12) NOT NULL,
    Phone NVARCHAR(20) NULL
);

CREATE TABLE Products (
    Id         INT IDENTITY(1,1) PRIMARY KEY,
    Name       NVARCHAR(200) NOT NULL,
    Price      DECIMAL(18,2) NOT NULL CHECK (Price >= 0),
    StockQty   INT NOT NULL CHECK (StockQty >= 0),
    CategoryId INT NOT NULL REFERENCES Categories(Id),
    SupplierId INT NOT NULL REFERENCES Suppliers(Id)
);

CREATE TABLE Customers (
    Id           INT IDENTITY(1,1) PRIMARY KEY,
    FullName     NVARCHAR(150) NOT NULL,
    Email        NVARCHAR(100) NOT NULL UNIQUE,
    Phone        NVARCHAR(20) NULL,
    RegisteredAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);

CREATE TABLE Addresses (
    Id         INT IDENTITY(1,1) PRIMARY KEY,
    CustomerId INT NOT NULL REFERENCES Customers(Id),
    City       NVARCHAR(80) NOT NULL,
    Street     NVARCHAR(150) NOT NULL,
    PostalCode CHAR(6) NOT NULL
);

CREATE TABLE Employees (
    Id       INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(150) NOT NULL,
    Role     NVARCHAR(50) NOT NULL,
    Login    NVARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE Orders (
    Id          INT IDENTITY(1,1) PRIMARY KEY,
    CustomerId  INT NOT NULL REFERENCES Customers(Id),
    AddressId   INT NOT NULL REFERENCES Addresses(Id),
    EmployeeId  INT NULL REFERENCES Employees(Id),
    CreatedAt   DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    Status      INT NOT NULL DEFAULT 0,
    TotalAmount DECIMAL(18,2) NOT NULL DEFAULT 0
);

CREATE TABLE OrderItems (
    Id        INT IDENTITY(1,1) PRIMARY KEY,
    OrderId   INT NOT NULL REFERENCES Orders(Id) ON DELETE CASCADE,
    ProductId INT NOT NULL REFERENCES Products(Id),
    Quantity  INT NOT NULL CHECK (Quantity > 0),
    UnitPrice DECIMAL(18,2) NOT NULL
);

CREATE TABLE Payments (
    Id      INT IDENTITY(1,1) PRIMARY KEY,
    OrderId INT NOT NULL UNIQUE REFERENCES Orders(Id),
    Method  INT NOT NULL,
    Amount  DECIMAL(18,2) NOT NULL,
    PaidAt  DATETIME2 NULL
);

CREATE TABLE Deliveries (
    Id          INT IDENTITY(1,1) PRIMARY KEY,
    OrderId     INT NOT NULL UNIQUE REFERENCES Orders(Id),
    CarrierName NVARCHAR(100) NOT NULL,
    TrackNumber NVARCHAR(50) NULL,
    ShippedAt   DATETIME2 NULL
);

CREATE TABLE OrderStatusHistories (
    Id         INT IDENTITY(1,1) PRIMARY KEY,
    OrderId    INT NOT NULL REFERENCES Orders(Id) ON DELETE CASCADE,
    EmployeeId INT NULL REFERENCES Employees(Id),
    OldStatus  INT NOT NULL,
    NewStatus  INT NOT NULL,
    ChangedAt  DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);

CREATE TABLE Reviews (
    Id         INT IDENTITY(1,1) PRIMARY KEY,
    ProductId  INT NOT NULL REFERENCES Products(Id),
    CustomerId INT NOT NULL REFERENCES Customers(Id),
    Rating     TINYINT NOT NULL CHECK (Rating BETWEEN 1 AND 5),
    Text       NVARCHAR(1000) NULL,
    CONSTRAINT UQ_Review UNIQUE (ProductId, CustomerId)
);
