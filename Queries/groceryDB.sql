--create database groceryDB
--go

use groceryDB
go

create table Cities (
	[id]			int				IDENTITY (1,1) PRIMARY KEY NOT NULL,
	[name]			nvarchar(50)	NOT NULL
);

create table Users (
	[id]			int				IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[firstName]		nvarchar(255)	NOT NULL,
	[lastName]		nvarchar(255)	NOT NULL,
	[email]			nvarchar(75)	UNIQUE NOT NULL,
	[password]		nvarchar(30)	NOT NULL,
	[phone]			nvarchar(20)	NULL,
	[address]		nvarchar(255)	NULL,
	[cityID]		int				NULL FOREIGN KEY REFERENCES Cities(id),
	[cartID]		int				NULL FOREIGN KEY REFERENCES Carts(id),
	[roles]			nvarchar(3)		DEFAULT 'C'
);

create table Carts (
	[id]			int				IDENTITY (1,1) PRIMARY KEY NOT NULL,
	[userID]		int				NOT NULL FOREIGN KEY REFERENCES Users(id) ON DELETE CASCADE
);

create table Stores (
	[id]			int				IDENTITY (1,1) PRIMARY KEY NOT NULL,
	[name]			nvarchar(255)	NOT NULL,
	[cityID]		int				NOT NULL FOREIGN KEY REFERENCES Cities(id),
	[imageURL]		nvarchar(255)	NULL,
	[description]	nvarchar(255)	NULL
);

create table Categories (
	[id]			int				IDENTITY (1,1) PRIMARY KEY NOT NULL,
	[name]			nvarchar(100)	NOT NULL,
	[storeID]		int				NOT NULL FOREIGN KEY REFERENCES Stores(id),
	[imageURL]		nvarchar(255)	NULL
);

create table Products (
	[id]			int				IDENTITY (1,1) PRIMARY KEY NOT NULL,
	[name]			nvarchar(255)	NOT NULL,
	[price]			decimal (10,2)	NOT NULL,
	[categoryID]	int				NOT NULL FOREIGN KEY REFERENCES Categories(id),
	[imageURL]		nvarchar(255)	NULL
);

create table CartItems (
	[id]			int				IDENTITY (1,1) PRIMARY KEY NOT NULL,
	[cartID]		int				NOT NULL FOREIGN KEY REFERENCES Carts(id),
	[productID]		int				NOT NULL FOREIGN KEY REFERENCES Products(id),
	[quantity]		int				NOT NULL,
	[price]			decimal (10,2)	NOT NULL
);

create table Orders (
	[id]			int				IDENTITY (1,1) PRIMARY KEY NOT NULL,
	[userID]		int				NOT NULL FOREIGN KEY REFERENCES Users(id),
	[status]		nvarchar(50)	NOT NULL,
	[totalPrice]	decimal (10,2)	NOT NULL
);

create table OrderItems ( 
	[id]			int				IDENTITY (1,1) PRIMARY KEY NOT NULL,
	[orderID]		int				NOT NULL FOREIGN KEY REFERENCES Orders(id),
	[productID]		int				NOT NULL FOREIGN KEY REFERENCES Products(id),
	[quantity]		int				NOT NULL,
	[price]			decimal (10,2)	NOT NULL
);

create table DeliveryPerson (
	[id]			int				IDENTITY (1,1) PRIMARY KEY NOT NULL,
	[name]			nvarchar(255)	NOT NULL,
	[phone]			nvarchar(20)	NULL,
	[status]		nvarchar(50)	NOT NULL
);

create table Deliveries (
	[id]			int				IDENTITY (1,1) PRIMARY KEY NOT NULL,
	[orderID]		int				NOT NULL FOREIGN KEY REFERENCES Orders(id),
	[delPersonID]	int				NOT NULL FOREIGN KEY REFERENCES DeliveryPerson(id),
	[status]		nvarchar(50)	NOT NULL,
	[deliveryTime]	timestamp		NULL
);

insert into Stores (name, cityID)
values ('Test Store 1', 1)

select * from Stores
select * from Categories

insert into Cities (name)
values ('Turkey')

insert into Categories (name, storeID)
values ('Fruits', 2)

insert into Products (name, price, categoryID)
values ('Ketchup Potato Chips', 35, 2)