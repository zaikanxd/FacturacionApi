USE Billing

ALTER TABLE ElectronicReceipt
ADD cdrLink VARCHAR(100) NULL, 
canceled BIT NULL,
cancellationReason VARCHAR(200) NULL,
cancellationName VARCHAR(20) NULL,
canceledPdfLink VARCHAR(100) NULL,
canceledXmlLink VARCHAR(100) NULL,
canceledCdrLink VARCHAR(100) NULL,
canceledTicketNumber VARCHAR(50) NULL

UPDATE ElectronicReceipt SET
canceled = 0

ALTER TABLE ElectronicReceipt
ALTER COLUMN canceled BIT NOT NULL

