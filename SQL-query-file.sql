CREATE DATABASE QLND;
USE QLND;

CREATE TABLE USERS (
	USERNAME varchar(20) NOT NULL PRIMARY KEY,
	PASSWORD varchar(20) NOT NULL,
	PRIORITY int DEFAULT 0
)

CREATE TABLE HISTORY (
	ID int IDENTITY(1,1) PRIMARY KEY,
	DATE datetime,
	USERNAME varchar(20) FOREIGN KEY REFERENCES USERS(USERNAME),
	WHAT nvarchar(255)
)

INSERT INTO USERS
-- mat khau la 123456
VALUES ('admin', 'MTIzNDU2', 1)

INSERT INTO USERS
-- mat khau la 123
VALUES ('guest', 'MTIz', 0)

INSERT INTO HISTORY 
VALUES (GETDATE(), 'admin', N'user admin được khởi tạo');

INSERT INTO HISTORY 
VALUES (GETDATE(), 'guest', N'user guest được khởi tạo');

