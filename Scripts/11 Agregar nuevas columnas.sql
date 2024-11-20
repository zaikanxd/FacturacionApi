USE Billing

ALTER TABLE ElectronicReceipt 
ADD observation VARCHAR(100) NULL,
 creditNoteType INT NULL,
 debitNoteType INT NULL,
 discrepancyRefNumber VARCHAR(50) NULL,
 discrepancyDescription VARCHAR(200) NULL