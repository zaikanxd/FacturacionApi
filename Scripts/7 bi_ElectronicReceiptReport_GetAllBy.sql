USE Billing
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE bi_ElectronicReceiptReport_GetAllBy
@date DATE,
@project VARCHAR(100),
@senderDocument VARCHAR(11)
AS

SELECT
	id,
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
	updateDate,
	(CASE
		WHEN senderDocumentTypeId = 6 THEN 'RUC'
		WHEN senderDocumentTypeId = 1 THEN 'DNI'
		ELSE '-'
	END) senderDocumentType,
	(CASE
		WHEN receiptTypeId = 3 THEN 'BOLETA'
		WHEN receiptTypeId = 1 THEN 'FACTURA'
		ELSE '-'
	END) receiptType,
	(CASE
		WHEN recipientDocumentTypeId = 6 THEN 'RUC'
		WHEN recipientDocumentTypeId = 1 THEN 'DNI'
		ELSE '-'
	END) recipientDocumentType
FROM ElectronicReceipt
WHERE project = @project
AND senderDocument = @senderDocument
AND issueDate = @date
ORDER BY id DESC