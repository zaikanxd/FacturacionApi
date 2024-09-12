USE Billing
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE bi_ElectronicReceipt_GetOne
@id INT

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
	IIF(canceled = 1, canceledPdfLink, pdfLink) pdfLink,
	xmlLink,
	issueDate,
	issueTime,
	currency,
	errorMessage,
	IIF(canceled = 1, canceledTicketNumber, cdrTicketNumber) cdrTicketNumber,
	userCreated,
	creationDate,
	updateDate,
	IIF(canceled = 1, canceledCdrLink, cdrLink) cdrLink,
	canceled,
	cancellationReason,
	cancellationName,
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
WHERE id = @id