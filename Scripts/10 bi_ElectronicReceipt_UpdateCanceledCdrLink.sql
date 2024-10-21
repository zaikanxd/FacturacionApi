USE Billing
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE bi_ElectronicReceipt_UpdateCanceledCdrLink
	@project VARCHAR(100),
	@nroRUC VARCHAR(11),
	@series VARCHAR(10),
	@correlative INT,
	@cancellationName VARCHAR(20),
	@canceledCdrLink VARCHAR(100),
	@canceledTicketNumber VARCHAR(50)
AS

UPDATE ElectronicReceipt SET
	canceledCdrLink = @canceledCdrLink
WHERE project = @project
AND senderDocument = @nroRUC
AND series = @series
AND correlative = @correlative
AND cancellationName = @cancellationName
AND canceledTicketNumber = @canceledTicketNumber