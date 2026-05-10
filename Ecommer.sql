IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'ECommerceDB')
BEGIN
    CREATE DATABASE ECommerceDB;
END
GO
USE ECommerceDB;
GO

IF OBJECT_ID('OrderDetail', 'U') IS NOT NULL DROP TABLE OrderDetail;
IF OBJECT_ID('Orders', 'U') IS NOT NULL DROP TABLE Orders;
IF OBJECT_ID('Item', 'U') IS NOT NULL DROP TABLE Item;
IF OBJECT_ID('Agent', 'U') IS NOT NULL DROP TABLE Agent;
IF OBJECT_ID('Users', 'U') IS NOT NULL DROP TABLE Users;
GO


CREATE TABLE Users (
    UserID INT PRIMARY KEY IDENTITY(1,1),
    UserName NVARCHAR(50) UNIQUE,
    Email NVARCHAR(100),
    Password NVARCHAR(50),
    IsLocked BIT DEFAULT 0
);

CREATE TABLE Agent (
    AgentID NVARCHAR(20) PRIMARY KEY,
    AgentName NVARCHAR(100),
    Address NVARCHAR(200)
);

CREATE TABLE Item (
    ItemID NVARCHAR(20) PRIMARY KEY,
    ItemName NVARCHAR(100),
    Size NVARCHAR(50),
    Price DECIMAL(18, 2)
);

CREATE TABLE Orders (
    OrderID NVARCHAR(20) PRIMARY KEY,
    OrderDate DATETIME,
    AgentID NVARCHAR(20),
    FOREIGN KEY (AgentID) REFERENCES Agent(AgentID)
);

CREATE TABLE OrderDetail (
    ID INT PRIMARY KEY IDENTITY(1,1),
    OrderID NVARCHAR(20),
    ItemID NVARCHAR(20),
    Quantity INT,
    UnitAmount DECIMAL(18, 2),
    FOREIGN KEY (OrderID) REFERENCES Orders(OrderID),
    FOREIGN KEY (ItemID) REFERENCES Item(ItemID)
);
GO

INSERT INTO Users (UserName, Email, Password, IsLocked) VALUES 
('baonhi', N'Hồng Bảo Nhi - nhi@tdtu.edu.vn', '123', 0),
('thaonhu', N'Phan Nguyễn Thảo Như - nhu@tdtu.edu.vn', '123', 0),
('minhnghia', N'Trần Minh Nghĩa - nghia@tdtu.edu.vn', '123', 0),
('trungtin', N'Huỳnh Trung Tín - tin@tdtu.edu.vn', '123', 0),
('admin', 'admin@tdtu.edu.vn', '123', 0);

INSERT INTO Agent (AgentID, AgentName, Address) VALUES 
('AG01', N'Đại lý Bảo Nhi', N'Quận 7, TP.HCM'),
('AG02', N'Đại lý Thảo Như', N'Quận 7, TP.HCM'),
('AG03', N'Đại lý Minh Nghĩa', N'Quận 1, TP.HCM'),
('AG04', N'Đại lý Trung Tín', N'Bình Chánh, TP.HCM'),
('AG05', N'Đại lý Đà Nẵng', N'Hải Châu, Đà Nẵng'),
('AG06', N'Đại lý Hà Nội', N'Hoàn Kiếm, Hà Nội'),
('AG07', N'Đại lý Bình Dương', N'Thủ Dầu Một'),
('AG08', N'Đại lý Đồng Nai', N'Biên Hòa'),
('AG09', N'Đại lý Long An', N'Tân An'),
('AG10', N'Đại lý Vũng Tàu', N'TP. Vũng Tàu'),
('AG11', N'Đại lý Nha Trang', N'Khánh Hòa'),
('AG12', N'Đại lý Đà Lạt', N'Lâm Đồng'),
('AG13', N'Đại lý Huế', N'Thừa Thiên Huế'),
('AG14', N'Đại lý Hải Phòng', N'Lê Chân'),
('AG15', N'Đại lý Quảng Ngãi', N'TP. Quảng Ngãi');

INSERT INTO Item (ItemID, ItemName, Size, Price) VALUES 
('IT01', N'Laptop Lenovo IdeaPad', N'14 inch', 15000000),
('IT02', N'Chuột Logitech G304', N'S', 800000),
('IT03', N'Bàn phím cơ Akko', N'Fullsize', 1200000),
('IT04', N'Màn hình Dell Ultrasharp', N'24 inch', 5500000),
('IT05', N'Tai nghe Sony WH-1000XM4', N'L', 6500000),
('IT06', N'Loa Bluetooth Marshall', N'M', 4000000),
('IT07', N'Ổ cứng SSD Samsung', N'500GB', 1500000),
('IT08', N'Ram Kingston HyperX', N'16GB', 1800000),
('IT09', N'Card đồ họa RTX 3060', N'Triple Fan', 9000000),
('IT10', N'Nguồn Corsair 750W', N'Standard', 2000000),
('IT11', N'Vỏ case Case Master', N'ATX', 1300000),
('IT12', N'Tản nhiệt khí Deepcool', N'Tower', 700000),
('IT13', N'Webcam Logitech C922', N'Mini', 2200000),
('IT14', N'Bàn di chuột SteelSeries', N'Large', 500000),
('IT15', N'Cáp HDMI 2.1', N'2 meter', 300000);
GO

SELECT * FROM Users;
SELECT * FROM Agent;
SELECT * FROM Item;