USE Billing
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE bi_ElectronicReceipt_Insert
	@project VARCHAR(100),
	@format VARCHAR(20),
	@senderDocumentTypeId INT,
	@senderDocument VARCHAR(11),
	@senderName VARCHAR(100),
	@series VARCHAR(10),
	@correlative INT,
	@receiptTypeId INT,
	@recipientDocumentTypeId INT,
	@recipientDocument VARCHAR(11),
	@recipientName VARCHAR(100),
	@discount DECIMAL(14,2) = NULL,
	@subtotal DECIMAL(14,2),
	@totalIGV DECIMAL(14,2),
	@total DECIMAL(14,2),
	@acceptedBySunat BIT,
	@sunatDescription VARCHAR(200) = NULL,
	@qrCode VARCHAR(200),
	@pdfLink VARCHAR(100),
	@xmlLink VARCHAR(100),
	@issueDate DATETIME,
	@issueTime VARCHAR(20),
	@currency VARCHAR(50),
	@errorMessage VARCHAR(200) = NULL,
    @cdrTicketNumber VARCHAR(50) = NULL,
	@userCreated VARCHAR(50) = NULL,
	@cdrLink VARCHAR(100) = NULL,
	@jsonLink VARCHAR(100) = NULL,
	@observation VARCHAR(100) = NULL,
	@discrepancyType INT = NULL,
	@discrepancyRefNumber VARCHAR(50) = NULL,
	@discrepancyDescription VARCHAR(200) = NULL,
	@electronicReceiptDet ElectronicReceiptDet READONLY,
	@receiptPaymentDet ReceiptPaymentDet READONLY
AS

DECLARE @electronicReceiptId INT

INSERT INTO ElectronicReceipt (
	project,
	format,
	senderDocumentTypeId,
	senderDocument,
	senderName,
	series,
	correlative,
	receiptTypeId,
	recipientDocumentTypeId,
	recipientDocument,
	recipientName,
	discount,
	subtotal,
	totalIGV,
	total,
	acceptedBySunat,
	sunatDescription,
	qrCode,
	pdfLink,
	xmlLink,
	issueDate,
	issueTime,
	currency,
	errorMessage,
	cdrTicketNumber,
	userCreated,
	creationDate,
	canceled,
	cdrLink,
	jsonLink,
	observation,
	creditNoteType,
	debitNoteType,
	discrepancyRefNumber,
	discrepancyDescription
)
VALUES(
	@project,
	@format,
	@senderDocumentTypeId,
	@senderDocument,
	@senderName,
	@series,
	@correlative,
	@receiptTypeId,
	@recipientDocumentTypeId,
	@recipientDocument,
	@recipientName,
	@discount,
	@subtotal,
	@totalIGV,
	@total,
	@acceptedBySunat,
	@sunatDescription,
	@qrCode,
	@pdfLink,
	@xmlLink,
	@issueDate,
	@issueTime,
	@currency,
	@errorMessage,
	@cdrTicketNumber,
	@userCreated,
	GETDATE(),
	0,
	@cdrLink,
	@jsonLink,
	@observation,
	IIF(@receiptTypeId = 7, @discrepancyType, NULL),
	IIF(@receiptTypeId = 8, @discrepancyType, NULL),
	@discrepancyRefNumber,
	@discrepancyDescription
)

SET @electronicReceiptId = @@IDENTITY

INSERT INTO ElectronicReceiptDet (
	electronicReceiptId,
	description, 
	additionalDescription,
	productCode,
	quantity,
	unitSunatCode,
	unitPrice,
	discount,
	igv,
	total
)
SELECT
	@electronicReceiptId,
	erd.description, 
	erd.additionalDescription,
	erd.productCode,
	erd.quantity,
	erd.unitSunatCode,
	erd.unitPrice,
	erd.discount,
	erd.igv,
	erd.total
FROM @electronicReceiptDet erd

INSERT INTO ReceiptPaymentDet (
	electronicReceiptId,
	accountType,
	payment,
	change,
	date,
	operationNum
)
SELECT
	@electronicReceiptId,
	rpd.accountType,
	rpd.payment,
	rpd.change,
	rpd.date,
	rpd.operationNum
FROM @receiptPaymentDet rpd