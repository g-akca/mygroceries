create database marketDB

use marketDB

create table Users (
	[userID]		int				IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[userName]		nvarchar(50)	NOT NULL,
	[userSurname]	nvarchar(50)	NOT NULL,
	[email]			nvarchar(75)	UNIQUE NOT NULL,
	[password]		nvarchar(30)	NOT NULL,
	[phone]			nvarchar(20)	NULL
);