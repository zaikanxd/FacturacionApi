CREATE DATABASE Billing

USE Billing

CREATE TABLE ElectronicReceipt (
 id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
 project VARCHAR(100) NOT NULL,
 format VARCHAR(20) NOT NULL,
 senderDocumentTypeId INT NOT NULL,
 senderDocument VARCHAR(11) NOT NULL,
 senderName VARCHAR(100) NOT NULL,
 series VARCHAR(10) NOT NULL,
 correlative INT NOT NULL,
 receiptTypeId INT NOT NULL,
 recipientDocumentTypeId INT NOT NULL,
 recipientDocument VARCHAR(11) NOT NULL,
 recipientName VARCHAR(100) NOT NULL,
 discount DECIMAL(14,2) NULL,
 subtotal DECIMAL(14,2) NOT NULL,
 totalIGV DECIMAL(14,2) NOT NULL,
 total DECIMAL(14,2) NOT NULL,
 acceptedBySunat BIT NOT NULL,
 sunatDescription VARCHAR(200) NULL,
 qrCode VARCHAR(200) NOT NULL,
 pdfLink VARCHAR(100) NOT NULL,
 xmlLink VARCHAR(100) NOT NULL,
 issueDate DATETIME NOT NULL,
 issueTime VARCHAR(20) NOT NULL,
 currency VARCHAR(50),
 errorMessage VARCHAR(200) NULL,
 cdrTicketNumber VARCHAR(50) NULL,
 userCreated VARCHAR(50) NULL,
 creationDate DATETIME NOT NULL,
 updateDate DATETIME NULL,
 numberResends INT NULL,
 cdrLink VARCHAR(100) NULL,
 canceled BIT NOT NULL,
 cancellationReason VARCHAR(200) NULL,
 cancellationName VARCHAR(20) NULL,
 canceledPdfLink VARCHAR(100) NULL,
 canceledXmlLink VARCHAR(100) NULL,
 canceledCdrLink VARCHAR(100) NULL,
 canceledTicketNumber VARCHAR(50) NULL,
 jsonLink VARCHAR(100) NULL,
 observation VARCHAR(100) NULL,
 creditNoteType INT NULL,
 debitNoteType INT NULL,
 discrepancyRefNumber VARCHAR(50) NULL,
 discrepancyDescription VARCHAR(200) NULL
)

CREATE TABLE ElectronicReceiptDet (
id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
electronicReceiptId INT REFERENCES ElectronicReceipt(id) NOT NULL,
description VARCHAR(100) NOT NULL,
additionalDescription VARCHAR(100) NULL,
productCode VARCHAR(50) NULL,
quantity INT NOT NULL,
unitSunatCode VARCHAR(10) NOT NULL,
unitPrice DECIMAL(14, 2) NOT NULL,
discount DECIMAL(14, 2) NOT NULL,
igv DECIMAL(14, 2) NOT NULL,
total DECIMAL(14, 2) NOT NULL
)

CREATE TYPE ElectronicReceiptDet AS TABLE(description VARCHAR(100), additionalDescription VARCHAR(100) NULL, productCode VARCHAR(50) NULL, quantity INT, unitSunatCode VARCHAR(10), unitPrice DECIMAL(14, 2), discount DECIMAL(14, 2), igv DECIMAL(14, 2), total DECIMAL(14, 2))

CREATE TABLE ReceiptPaymentDet (
 id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
 electronicReceiptId INT REFERENCES ElectronicReceipt(id) NOT NULL,
 accountType VARCHAR(50) NOT NULL,
 payment DECIMAL(14, 2) NOT NULL,
 change DECIMAL(14, 2) NOT NULL,
 date DATE NOT NULL,
 operationNum VARCHAR(50) NULL
)

CREATE TYPE ReceiptPaymentDet AS TABLE(accountType VARCHAR(50), payment DECIMAL(14, 2), change DECIMAL(14, 2), date DATE, operationNum VARCHAR(50) NULL)

CREATE TABLE [User] (
 id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
 username VARCHAR(50) NOT NULL,
 password VARCHAR(200) NOT NULL,
 status BIT NOT NULL,
 names VARCHAR(100) NULL,
 lastName VARCHAR(100) NULL,
 motherLastName VARCHAR(100) NULL,
 userCreated VARCHAR(50) NOT NULL,
 creationDate DATETIME NOT NULL,
 userUpdated VARCHAR(50) NULL,
 updateDate DATETIME NULL,
)