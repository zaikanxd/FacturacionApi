USE Billing

ALTER TABLE ElectronicReceipt 
ADD observation VARCHAR(100) NULL,
 creditNoteType INT NULL,
 debitNoteType INT NULL,
 discrepancyRefNumber VARCHAR(50) NULL,
 discrepancyDescription VARCHAR(200) NULL

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
