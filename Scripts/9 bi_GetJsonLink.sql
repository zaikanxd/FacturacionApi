USE Billing
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE bi_GetJsonLink
	@project VARCHAR(100),
	@senderDocumentTypeId INT,
	@senderDocument VARCHAR(11),
	@series VARCHAR(10),
	@correlative INT,
	@issueDate DATETIME,
	@jsonLink VARCHAR(100) OUTPUT
AS

SELECT TOP 1
	@jsonLink = jsonLink
FROM ElectronicReceipt 
WHERE project = @project 
AND senderDocumentTypeId = @senderDocumentTypeId 
AND senderDocument = @senderDocument 
AND series = @series 
AND correlative = @correlative 
AND issueDate = @issueDate
ORDER BY id DESC