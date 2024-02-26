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
	@userCreated VARCHAR(50) = NULL
AS

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
	creationDate
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
	GETDATE()
)