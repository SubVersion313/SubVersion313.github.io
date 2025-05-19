-- Create database
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'El_Harrifa')
BEGIN
    CREATE DATABASE El_Harrifa;
END
GO

USE El_Harrifa;
GO

-- Create Users table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Users')
BEGIN
    CREATE TABLE Users (
        id INT IDENTITY(1,1) PRIMARY KEY,
        nameuser VARCHAR(100) NOT NULL,
        email VARCHAR(100) NOT NULL UNIQUE,
        passworduser VARCHAR(255) NOT NULL,
        address VARCHAR(255),
        is_admin BIT DEFAULT 0,
        created_at DATETIME DEFAULT GETDATE()
    );
END
GO

-- Create Categories table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Categories')
BEGIN
    CREATE TABLE Categories (
        id INT IDENTITY(1,1) PRIMARY KEY,
        name VARCHAR(100) NOT NULL UNIQUE,
        description TEXT
    );
END
GO

-- Create Products table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Products')
BEGIN
    CREATE TABLE Products (
        id INT IDENTITY(1,1) PRIMARY KEY,
        name VARCHAR(100) NOT NULL,
        price DECIMAL(10,2) NOT NULL,
        image_url VARCHAR(255),
        category VARCHAR(100),
        details NVARCHAR(MAX),
        seller_id INT,
        created_at DATETIME DEFAULT GETDATE(),
        FOREIGN KEY (category) REFERENCES Categories(name),
        FOREIGN KEY (seller_id) REFERENCES Users(id)
    );
END
GO

-- Create Cart table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Cart')
BEGIN
    CREATE TABLE Cart (
        id INT IDENTITY(1,1) PRIMARY KEY,
        user_id INT,
        product_id INT,
        quantity INT DEFAULT 1,
        created_at DATETIME DEFAULT GETDATE(),
        FOREIGN KEY (user_id) REFERENCES Users(id),
        FOREIGN KEY (product_id) REFERENCES Products(id)
    );
END
GO

-- Create Reviews table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Reviews')
BEGIN
    CREATE TABLE Reviews (
        id INT IDENTITY(1,1) PRIMARY KEY,
        user_id INT,
        product_id INT,
        rating INT CHECK (rating >= 1 AND rating <= 5),
        comment TEXT,
        created_at DATETIME DEFAULT GETDATE(),
        FOREIGN KEY (user_id) REFERENCES Users(id),
        FOREIGN KEY (product_id) REFERENCES Products(id)
    );
END
GO

-- Create Orders table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Orders')
BEGIN
    CREATE TABLE Orders (
        id INT IDENTITY(1,1) PRIMARY KEY,
        user_id INT,
        total_amount DECIMAL(10,2) NOT NULL,
        status VARCHAR(50) DEFAULT 'Pending',
        created_at DATETIME DEFAULT GETDATE(),
        FOREIGN KEY (user_id) REFERENCES Users(id)
    );
END
GO

-- Create OrderItems table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'OrderItems')
BEGIN
    CREATE TABLE OrderItems (
        id INT IDENTITY(1,1) PRIMARY KEY,
        order_id INT,
        product_id INT,
        quantity INT NOT NULL,
        price DECIMAL(10,2) NOT NULL,
        FOREIGN KEY (order_id) REFERENCES Orders(id),
        FOREIGN KEY (product_id) REFERENCES Products(id)
    );
END
GO

-- Create Shipping table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Shipping')
BEGIN
    CREATE TABLE Shipping (
        id INT IDENTITY(1,1) PRIMARY KEY,
        order_id INT,
        address VARCHAR(255) NOT NULL,
        status VARCHAR(50) DEFAULT 'Pending',
        tracking_number VARCHAR(100),
        created_at DATETIME DEFAULT GETDATE(),
        FOREIGN KEY (order_id) REFERENCES Orders(id)
    );
END
GO

-- Create Payments table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Payments')
BEGIN
    CREATE TABLE Payments (
        id INT IDENTITY(1,1) PRIMARY KEY,
        order_id INT,
        amount DECIMAL(10,2) NOT NULL,
        payment_method VARCHAR(50) NOT NULL,
        status VARCHAR(50) DEFAULT 'Pending',
        created_at DATETIME DEFAULT GETDATE(),
        FOREIGN KEY (order_id) REFERENCES Orders(id)
    );
END
GO
