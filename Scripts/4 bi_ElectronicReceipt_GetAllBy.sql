USE Billing
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE bi_ElectronicReceipt_GetAllBy
@filter VARCHAR(500) = NULL
AS

SELECT *
FROM (
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
) T
WHERE
project LIKE '%' + @filter + '%'
OR format LIKE '%' + @filter + '%'
OR senderDocument LIKE '%' + @filter + '%'
OR senderName LIKE '%' + @filter + '%'
OR recipientDocument LIKE '%' + @filter + '%'
OR recipientName LIKE '%' + @filter + '%'
OR CONVERT(VARCHAR, discount) LIKE '%' + @filter + '%'
OR CONVERT(VARCHAR, subtotal) LIKE '%' + @filter + '%'
OR CONVERT(VARCHAR, totalIGV) LIKE '%' + @filter + '%'
OR CONVERT(VARCHAR, total) LIKE '%' + @filter + '%'
OR sunatDescription LIKE '%' + @filter + '%'
OR cdrTicketNumber LIKE '%' + @filter + '%'
OR userCreated LIKE '%' + @filter + '%'
OR CONVERT(VARCHAR, creationDate) LIKE '%' + @filter + '%'
OR CONVERT(VARCHAR, updateDate) LIKE '%' + @filter + '%'
OR @filter IS NULL
ORDER BY id DESC