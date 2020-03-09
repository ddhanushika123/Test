
CREATE DATABASE Student;

USE Student;

CREATE TABLE Mas_user
(
	Id INT NOT NULL,
	Username VARCHAR(10) NOT NULL,
	Password VARCHAR(10) NOT NULL,
	CONSTRAINT Pk_Mas_user PRIMARY KEY(Id) 
);

CREATE TABLE student_details
(
	Code INT NOT NULL,
	FirstName VARCHAR(30) NOT NULL,
	LastName VARCHAR(30) NOT NULL,
	DOB DATE NOT NULL,
	CONSTRAINT Pk_student_details PRIMARY KEY(Code)  
);

INSERT INTO Mas_user (Id,Username,Password) 
VALUES (1001,'admin',12345);

