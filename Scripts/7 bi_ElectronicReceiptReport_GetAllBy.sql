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
	observation,
	creditNoteType,
	debitNoteType,
	discrepancyRefNumber,
	discrepancyDescription,
	(CASE
		WHEN senderDocumentTypeId = 1 THEN 'DNI'
		WHEN senderDocumentTypeId = 6 THEN 'RUC'
		ELSE '-'
	END) senderDocumentType,
	(CASE
		WHEN receiptTypeId = 1 THEN 'FACTURA'
		WHEN receiptTypeId = 3 THEN 'BOLETA'
		WHEN receiptTypeId = 7 THEN 'NOTA DE CRÉDITO'
		WHEN receiptTypeId = 8 THEN 'NOTA DE DÉBITO'
		ELSE '-'
	END) receiptType,
	(CASE
		WHEN recipientDocumentTypeId = 1 THEN 'DNI'
		WHEN recipientDocumentTypeId = 6 THEN 'RUC'
		ELSE '-'
	END) recipientDocumentType
FROM ElectronicReceipt
WHERE project = @project
AND senderDocument = @senderDocument
AND issueDate = @date
ORDER BY id DESC