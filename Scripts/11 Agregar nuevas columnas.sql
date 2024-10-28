USE Billing

ALTER TABLE ElectronicReceipt 
ADD observation VARCHAR(100) NULL,
 discrepancyRefNumber VARCHAR(50) NULL,
 discrepancyType INT NULL,
 discrepancyDescription VARCHAR(200) NULL