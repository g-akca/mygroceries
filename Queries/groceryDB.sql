--create database groceryDB
--go

use groceryDB
go

create table Cities (
	[id]			int				IDENTITY (1,1) PRIMARY KEY NOT NULL,
	[name]			nvarchar(50)	NOT NULL,
	[isActive]		int				NOT NULL DEFAULT 1
);

create table Stores (
	[id]			int				IDENTITY (1,1) PRIMARY KEY NOT NULL,
	[name]			nvarchar(255)	NOT NULL,
	[cityID]		int				NOT NULL FOREIGN KEY REFERENCES Cities(id),
	[imageURL]		nvarchar(255)	NULL,
	[description]	nvarchar(255)	NULL,
	[isActive]		int				NOT NULL DEFAULT 1
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
	[managedStore]	int				NULL FOREIGN KEY REFERENCES Stores(id),
	[isActive]		int				NOT NULL DEFAULT 1
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
	[imageURL]		nvarchar(255)	NULL,
	[isActive]		int				NOT NULL DEFAULT 1
);

create table Products (
	[id]			int				IDENTITY (1,1) PRIMARY KEY NOT NULL,
	[name]			nvarchar(255)	NOT NULL,
	[price]			decimal (10,2)	NOT NULL,
	[categoryID]	int				NOT NULL FOREIGN KEY REFERENCES Categories(id),
	[imageURL]		nvarchar(255)	NULL,
	[description]	nvarchar(255)	NULL,
	[isActive]		int				NOT NULL DEFAULT 1
);

create table CartItems (
	[id]			int				IDENTITY (1,1) PRIMARY KEY NOT NULL,
	[cartID]		int				NOT NULL FOREIGN KEY REFERENCES Carts(id) ON DELETE CASCADE,
	[productID]		int				NOT NULL FOREIGN KEY REFERENCES Products(id),
	[quantity]		int				NOT NULL,
	[price]			decimal (10,2)	NOT NULL
);

create table Couriers ( 
	[id]			int				IDENTITY (1,1) PRIMARY KEY NOT NULL,
	[name]			nvarchar(255)	NOT NULL,
	[phone]			nvarchar(255)	NOT NULL,
	[isActive]		int				NOT NULL DEFAULT 1
);

create table Orders (
	[id]			int				IDENTITY (1,1) PRIMARY KEY NOT NULL,
	[userID]		int				NOT NULL FOREIGN KEY REFERENCES Users(id),
	[storeID]		int				NOT NULL FOREIGN KEY REFERENCES Stores(id),
	[courierID]		int				NOT NULL FOREIGN KEY REFERENCES Couriers(id),
	[status]		nvarchar(50)	NOT NULL,
	[totalPrice]	decimal (10,2)	NOT NULL,
	[date]			datetime		NULL,
	[firstName]		nvarchar(255)	NOT NULL,
	[lastName]		nvarchar(255)	NOT NULL,
	[address]		nvarchar(255)	NOT NULL,
	[email]			nvarchar(75)	NOT NULL,
	[phone]			nvarchar(20)	NULL,
	[cityID]		int				NOT NULL FOREIGN KEY REFERENCES Cities(id),
	[isActive]		int				NOT NULL DEFAULT 1
);

create table OrderItems ( 
	[id]			int				IDENTITY (1,1) PRIMARY KEY NOT NULL,
	[orderID]		int				NOT NULL FOREIGN KEY REFERENCES Orders(id) ON DELETE CASCADE,
	[productID]		int				NOT NULL FOREIGN KEY REFERENCES Products(id),
	[quantity]		int				NOT NULL,
	[price]			decimal (10,2)	NOT NULL
);

create table Inquiries (
	[id]			int				IDENTITY (1,1) PRIMARY KEY NOT NULL,
	[name]			nvarchar(255)	NOT NULL,
	[email]			nvarchar(75)	NOT NULL,
	[subject]		nvarchar(75)	NOT NULL,
	[message]		nvarchar(255)	NOT NULL,
	[isActive]		int				NOT NULL DEFAULT 1
);

insert into Cities (name)
values ('Istanbul')

insert into Cities (name)
values ('Ankara')

insert into Stores (name, cityID)
values ('Migros', 1)

insert into Stores (name, cityID)
values ('CarrefourSA', 1)

insert into Stores (name, cityID)
values ('Metro Market', 2)

insert into Stores (name, cityID)
values ('A101', 2)

insert into Users (firstName, lastName, email, password, roles)
values ('Admin', 'Account', 'admin@email.com', 12345, 'A')

insert into Users (firstName, lastName, email, password, roles, managedStore)
values ('Store', 'Manager', 'manager@email.com', 12345, 'S', 1)

insert into Carts (userID)
values (1)

update Users
set cartID = 1
where id = 1;

insert into Carts (userID)
values (2)

update Users
set cartID = 2
where id = 2;

insert into Categories (name, storeID)
values ('Snacks', 1)

insert into Categories (name, storeID)
values ('Drinks', 1)

insert into Categories (name, storeID)
values ('Frozen Food', 1)

insert into Categories (name, storeID)
values ('Meat & Fish', 1)

insert into Categories (name, storeID)
values ('Fruits', 1)

insert into Categories (name, storeID)
values ('Vegetables', 1)

insert into Categories (name, storeID)
values ('Snacks', 2)

insert into Categories (name, storeID)
values ('Drinks', 2)

insert into Categories (name, storeID)
values ('Frozen Food', 2)

insert into Categories (name, storeID)
values ('Meat & Fish', 2)

insert into Categories (name, storeID)
values ('Snacks', 3)

insert into Categories (name, storeID)
values ('Drinks', 3)

insert into Categories (name, storeID)
values ('Frozen Food', 3)

insert into Categories (name, storeID)
values ('Meat & Fish', 3)

insert into Categories (name, storeID)
values ('Fruits', 3)

insert into Categories (name, storeID)
values ('Vegetables', 3)

insert into Categories (name, storeID)
values ('Snacks', 4)

insert into Categories (name, storeID)
values ('Drinks', 4)

insert into Categories (name, storeID)
values ('Meat & Fish', 4)

insert into Categories (name, storeID)
values ('Fruits', 4)

insert into Categories (name, storeID)
values ('Vegetables', 4)

insert into Products (name, price, categoryID)
values ('Potato Chips', 35, 1)

insert into Couriers (name, phone)
values ('Test Driver', '(468)-511-8464')