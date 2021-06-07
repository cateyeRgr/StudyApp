
--create database StudyApp

use StudyApp
go

IF OBJECT_ID('[User]') IS NOT NULL
  DROP TABLE [User];
GO


IF OBJECT_ID('Session') IS NOT NULL
  DROP TABLE Session;
GO

IF OBJECT_ID('Subject') IS NOT NULL
  DROP TABLE Subject;
GO

IF OBJECT_ID('Item') IS NOT NULL
  DROP TABLE Item;
GO

IF OBJECT_ID('Item_Session') IS NOT NULL
  DROP TABLE Item_Session;
GO




CREATE TABLE [User] (
[User_ID] INT IDENTITY PRIMARY KEY,
[User_Name] varchar(100) NOT NULL,
Hash varchar(100) NOT NULL,
Salt varchar(100) NOT NULL,
);


CREATE TABLE Session (
Session_ID INT IDENTITY PRIMARY KEY,
Session_Date DATETIME  NOT NULL, 
Session_Points DECIMAL(5,2) NOT NULL,
[User_ID] int,
CONSTRAINT fk_session_user FOREIGN KEY ([User_ID]) REFERENCES [User]([User_ID])
ON DELETE CASCADE,
);

CREATE TABLE Subject (
Subject_ID INT IDENTITY PRIMARY KEY,
Subject_Name varchar(100),
);

Create Table Item (
Item_ID int IDENTITY PRIMARY KEY,
Item_Name varchar(100),
Item_Content varchar(100) NOT NULL,
Subject_ID int,
CONSTRAINT fk_item_subject FOREIGN KEY (Subject_ID) REFERENCES Subject(Subject_ID)
ON DELETE CASCADE,
);


CREATE TABLE ItemSession(
    Item_ID int NOT NULL,
	CONSTRAINT item_subject FOREIGN KEY (Item_ID) REFERENCES Item(Item_ID)
	on delete CASCADE,
    Session_ID int NOT NULL,
    CONSTRAINT item_session FOREIGN KEY (Session_ID) REFERENCES Session(Session_ID)
	on delete CASCADE,
    UNIQUE (Item_ID, Session_ID)
	);








