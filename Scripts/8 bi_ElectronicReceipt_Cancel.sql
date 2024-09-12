USE Billing
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE bi_ElectronicReceipt_Cancel
	@project VARCHAR(100),
	@nroRUC VARCHAR(11),
	@series VARCHAR(10),
	@correlative INT,
	@cancellationReason VARCHAR(200),
	@cancellationName VARCHAR(20),
	@canceledPdfLink VARCHAR(100) = NULL,
	@canceledXmlLink VARCHAR(100) = NULL,
	@canceledCdrLink VARCHAR(100) = NULL,
	@canceledTicketNumber VARCHAR(50) = NULL
AS

UPDATE ElectronicReceipt SET
	canceled = 1,
	cancellationReason = @cancellationReason,
	cancellationName = @cancellationName,
	canceledPdfLink = @canceledPdfLink,
	canceledXmlLink = @canceledXmlLink,
	canceledCdrLink = @canceledCdrLink,
	canceledTicketNumber = @canceledTicketNumber,
	updateDate = GETDATE()
WHERE project = @project
AND senderDocument = @nroRUC
AND series = @series
AND correlative = @correlative