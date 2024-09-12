USE Billing

ALTER TABLE ElectronicReceipt
ADD cdrLink VARCHAR(100) NULL, 
canceled BIT NULL,
cancellationReason VARCHAR(200) NULL,
cancellationName VARCHAR(20) NULL

UPDATE ElectronicReceipt SET
canceled = 0

ALTER TABLE ElectronicReceipt
ALTER COLUMN canceled BIT NOT NULL