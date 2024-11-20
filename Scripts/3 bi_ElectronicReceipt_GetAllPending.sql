USE Billing
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE bi_ElectronicReceipt_GetAllPending
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
	cdrLink,
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
WHERE acceptedBySunat = 0
AND (numberResends IS NULL OR numberResends < 9)
AND canceled = 0