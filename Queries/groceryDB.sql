--create database groceryDB
--go

use groceryDB
go

create table Cities (
	[id]			int				IDENTITY (1,1) PRIMARY KEY NOT NULL,
	[name]			nvarchar(50)	NOT NULL
);

create table Stores (
	[id]			int				IDENTITY (1,1) PRIMARY KEY NOT NULL,
	[name]			nvarchar(255)	NOT NULL,
	[cityID]		int				NOT NULL FOREIGN KEY REFERENCES Cities(id),
	[imageURL]		nvarchar(255)	NULL,
	[description]	nvarchar(255)	NULL
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
	[cartID]		int				UNIQUE NULL,
	[roles]			nvarchar(3)		DEFAULT 'C',
	[managedStore]	int				NULL FOREIGN KEY REFERENCES Stores(id)		
);

create table Carts (
	[id]			int				IDENTITY (1,1) PRIMARY KEY NOT NULL,
	[userID]		int				UNIQUE NOT NULL FOREIGN KEY REFERENCES Users(id) ON DELETE CASCADE
);

alter table Users
add constraint UC foreign key (cartID) references Carts(id)

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

create table Drivers ( 
	[id]			int				IDENTITY (1,1) PRIMARY KEY NOT NULL,
	[name]			nvarchar(255)	NOT NULL,
	[phone]			nvarchar(255)	NOT NULL
);

create table Orders (
	[id]			int				IDENTITY (1,1) PRIMARY KEY NOT NULL,
	[userID]		int				NOT NULL FOREIGN KEY REFERENCES Users(id),
	[storeID]		int				NOT NULL FOREIGN KEY REFERENCES Stores(id),
	[driverID]		int				NOT NULL FOREIGN KEY REFERENCES Drivers(id),
	[status]		nvarchar(50)	NOT NULL,
	[totalPrice]	decimal (10,2)	NOT NULL,
	[date]			datetime		NULL,
	[firstName]		nvarchar(255)	NOT NULL,
	[lastName]		nvarchar(255)	NOT NULL,
	[address]		nvarchar(255)	NOT NULL,
	[email]			nvarchar(75)	NOT NULL,
	[phone]			nvarchar(20)	NULL,
	[cityID]		int				NOT NULL FOREIGN KEY REFERENCES Cities(id)
);

create table OrderItems ( 
	[id]			int				IDENTITY (1,1) PRIMARY KEY NOT NULL,
	[orderID]		int				NOT NULL FOREIGN KEY REFERENCES Orders(id),
	[productID]		int				NOT NULL FOREIGN KEY REFERENCES Products(id),
	[quantity]		int				NOT NULL,
	[price]			decimal (10,2)	NOT NULL
);

insert into Cities (name)
values ('Istanbul')

insert into Stores (name, cityID)
values ('Test Store 1', 1)

insert into Stores (name, cityID)
values ('Test Store 2', 1)

insert into Users (firstName, lastName, email, password, roles)
values ('Admin', 'Acc', 'admin@email.com', 12345, 'A')

insert into Users (firstName, lastName, email, password, roles, managedStore)
values ('Store', 'Manager', 'strmng@email.com', 12345, 'S', 1)

insert into Carts (userID)
values (1)

update Users
set cartID = 1
where id = 1;

insert into Categories (name, storeID)
values ('Snacks', 1)

insert into Products (name, price, categoryID)
values ('Ketchup Flavoured Potato Chips', 35, 1)

insert into Drivers (name, phone)
values ('Test Driver', '(468)-511-8464')